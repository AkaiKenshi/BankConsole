
using BankConsole.Contracts;
using BankConsole.Contracts.DTOs.Accounts;
using BankConsole.Contracts.DTOs.Customers;
using BankConsole.Contracts.Models;
using BankConsole.Contracts.Processor;
using BankConsole.Options;
using System.Text.RegularExpressions;

namespace BankConsole;

internal class Program
{
    private static void Main(string[] args)
    {
        ApiHelper.InnitalizeClient();

        bool KeepRunningApp = true;
        while (KeepRunningApp)
        {
            try
            {
                OptionHelper.RunCommand(
                        "Select a Option",
                        ("Create User Account", () => CreateUserAccount()),
                        ("LogIn to User Account", () => LogInUserAccount()),
                        ("Pass Time", () => PassTime()),
                        ("Exit Application", () => KeepRunningApp = Confirmate()));
            }
            catch (OperationCanceledException) { Console.WriteLine("returning to start..."); }

            static bool Confirmate() => OptionHelper.RunCommand("Are you sure you want to exit the application", 
                ("Yes", () => false),
                ("No", () => true));
        }
    }

    //Command Functions
    private static void LogInUserAccount()
    {
        try
        {

            var username = GetUserInput("write your Username: ", "Invalid Username, Try Again");
            var password = GetPassword("Enter password: ");

            var customer = CustomerProcessor.LoginUserAsync(new GetCustomerLoginRequestDTO(username, password)).Result;

            Console.WriteLine($"\nWelcome {customer.FirstName} {customer.LastName}\n");

            AccountOptions(customer);
        }
        catch (AggregateException ex)
        {
            if (ex.InnerException is not HttpRequestException)
            {
                throw ex.InnerException!;
            }
            Console.WriteLine("Invalid Login");
            LogInUserAccount();
        }
    }
    private static void CreateUserAccount()
    {
        var id = GetAvailableId();
        var username = GetAvailableUsername();
        var firstName = GetUserInput("Write your first name: ", "Invalid name, Try again");
        var lastName = GetUserInput("Write your last name: ", "Invalid last name, Try again");
        var email = GetAvailableEmail();
        var password = GetValidPassword();

        var createRequest = new CreateCustomerRequestDTO(id, username, firstName, lastName, email, password);
        var customer = CustomerProcessor.RegisterUserAsync(createRequest).Result;

        Console.WriteLine($"\nWelcome {customer.FirstName} {customer.LastName}\n");

        AccountOptions(customer);
    }
    private static void AccountOptions(Customer customer)
    {
        var KeepRunning = true;
        while (KeepRunning)
        {
            OptionHelper.RunCommand(
                "Would you like to: ",
                ("Create New Bank Account", () => CreateBankAccount(customer)),
                ("Access Bank Account", () => AccessBankAccount(customer)),
                ("Go back", () => KeepRunning = false));
        }
    }
    private static void CreateBankAccount(Customer customer)
    {

        var account = OptionHelper.RunCommand(
            "What Type of account would you like to create",
            ("Checking Account", () => chekingAccount()),
            ("Savings Account", () => savingsAccount()),
            ("Fixed Term Investment Account", () => longTermInvestmentAccount()));

        Console.WriteLine($"\nAccount {account.Id} created with balance: {account.Balance:C}\n");

        AccountActions(customer, account);

        Account chekingAccount()
        {
            var moneyAmmounut = GetValidAmountOfMoney("Enter the initial balance: ", "invalid Balance");
            var createRequest = new CreateCheckingAccountRequestDTO(moneyAmmounut);
            return AccountProcessor.
                CreateAccountAsync<CreateCheckingAccountRequestDTO>(createRequest, "/api/Account/createCheckingAccount", customer.Token).Result;
        }
        Account savingsAccount()
        {
            var moneyAmmounut = GetValidAmountOfMoney("Enter the initial balance: ", "invalid Balance");
            var createRequest = new CreateSavingsAccountRequestDTO(moneyAmmounut);
            return AccountProcessor.
                CreateAccountAsync<CreateSavingsAccountRequestDTO>(createRequest, "/api/Account/createSavingsAccount", customer.Token).Result;
        }
        Account longTermInvestmentAccount()
        {
            var moneyAmmounut = GetValidAmountOfMoney("Enter the initial balance: ", "invalid Balance");
            var timeAmmounnt = GetValidAmmountOfTime("Enter the term: ", "invalid Term");
            var createRequest = new CreateSavingsAccountRequestDTO(moneyAmmounut);
            return AccountProcessor.
                CreateAccountAsync<CreateSavingsAccountRequestDTO>(createRequest, "/api/Account/createSavingsAccount", customer.Token).Result;
        }
    }
    private static void AccessBankAccount(Customer customer)
    {
        try
        {
            var accountId = GetUserInput("Enter the account number: ", "invalid account");
            var account = AccountProcessor.GetAccountFromIdAsync(accountId, customer.Token).Result;

            Console.WriteLine($"\nAccess to account {account.Id} granted, balance: {account.Balance:C}\n");

            AccountActions(customer, account);
        }
        catch (AggregateException ex)
        {
            if (ex.InnerException is not HttpRequestException)
            {
                throw ex.InnerException!;
            }
            Console.WriteLine("Invalid access to account");
            AccessBankAccount(customer);
        }
    }
    private static void AccountActions(Customer customer, Account account)
    {
        var keepGoing = true;
        while (keepGoing) {
            OptionHelper.RunCommand(
                "What would you like to do: ",
                ("Deposit Money", () => DepositMoney(customer, account)), 
                ("Withdraw Money",  () => WithdrawMoney(customer, account)),
                ("Transfer Money", () => TransferMoney(customer, account)),
                ("Exit", () => keepGoing = false));
        }
    }
    private static void DepositMoney(Customer customer, Account account)
    {
        try
        {
            var url = "/api/Account/UpdateDepositBalance";
            var money = GetValidAmountOfMoney("Enter Deposit amount: ", "Invalid Amount");
            var depoistDto = new UpdateDepositBalanceRequestDTO(account.Id, money);
            AccountProcessor.TransactionBalanceAsync(depoistDto, url, customer.Token).Wait();
            account.Balance += money;

            Console.WriteLine($"\nSuccessful deposit the new balance is {account.Balance:C}\n");
        }
        catch (AggregateException ex)
        {
            if (ex.InnerException is not HttpRequestException) { throw ex.InnerException!; }
            Console.WriteLine(ex.InnerException.Message);

        }
    }
    private static void WithdrawMoney(Customer customer, Account account)
    {
        try
        {
            var url = "/api/Account/UpdateWithdrawBalance";
            var money = GetValidAmountOfMoney("Enter Withdraw amount: ", "Invalid Amount");
            var depoistDto = new UpdateWithdrawBalanceRequestDTO(account.Id, money);
            AccountProcessor.TransactionBalanceAsync(depoistDto, url, customer.Token).Wait();
            account.Balance -= money;

            Console.WriteLine($"\nSuccessful withdraw the new balance is {account.Balance:C}\n");
        }
        catch (AggregateException ex)
        {
            if (ex.InnerException is not HttpRequestException) { throw ex.InnerException!; }
            Console.WriteLine(ex.InnerException.Message);

        }
    }
    private static void TransferMoney(Customer customer, Account account)
    {
        try
        {
            var url = "/api/Account/UpdateTransferBalance";
            var money = GetValidAmountOfMoney("Enter Transfer amount: ", "Invalid Amount");
            var targetAccount = GetValidAccount("Enter Target Account: ", "Invalid Account");
            var depoistDto = new UpdateTransferBalanceRequestDTO(account.Id, targetAccount, money);
            AccountProcessor.TransactionBalanceAsync(depoistDto, url, customer.Token).Wait();
            account.Balance -= money;

            Console.WriteLine($"\nSuccessful transfer the new balance is {account.Balance:C}\n");
        }
        catch (AggregateException ex)
        {
            if (ex.InnerException is not HttpRequestException) { throw ex.InnerException!; }
            Console.WriteLine(ex.InnerException.Message);

        }
    }
    private static void PassTime()
    {
        var time = GetValidAmmountOfTime("Time to Pass: ", "Invalid amount");
        TimeProcessor.PassTime(time).Wait();  
    }
    #region HelperFunctions
    private static string GetUserInput(string promnt, string promntOnFail)
    {
        Console.Write(promnt);
        var userInput = Console.ReadLine();

        if (String.IsNullOrWhiteSpace(userInput))
        {
            Console.WriteLine(promntOnFail);
            return GetUserInput(promnt, promntOnFail);
        }
        return userInput;
    }
    private static string GetAvailableId()
    {
        var userInput = GetUserInput("Write your Id: ", "Invalid id, Try Again");

        if (userInput.Length != 10 || !int.TryParse(userInput, out _))
        {
            Console.WriteLine("Invalid id, Try Again");
            return GetAvailableId();
        }
        else if (!CustomerProcessor.IsCustomerIdAvailableAsync(userInput).Result)
        {
            Console.WriteLine("invalid Id or already taken");
            return OptionHelper.RunCommand(
                "Try Again",
                ("Yes", () => GetAvailableId()),
                ("No", () => throw new OperationCanceledException()));
        }

        return userInput;
    }
    private static string GetAvailableUsername()
    {
        var userInput = GetUserInput("Write your Username: ", "Invalid username, Try Again");
        if (!CustomerProcessor.IsCustomerUsernameAvailableAsync(userInput).Result)
        {
            Console.WriteLine("Invalid username or already taken");
            return OptionHelper.RunCommand(
                "Try Again",
                ("Yes", () => GetAvailableUsername()),
                ("No", () => throw new OperationCanceledException()));
        }
        return userInput;
    }
    private static string GetAvailableEmail()
    {
        var userInput = GetUserInput("Write your Email: ", "Invalid Email, Try Again");
        if (!CustomerProcessor.IsCustomerUsernameAvailableAsync(userInput).Result)
        {
            Console.WriteLine("Invalid Email or already taken");
            return OptionHelper.RunCommand(
                "Try Again",
                ("Yes", () => GetAvailableEmail()),
                ("No", () => throw new OperationCanceledException()));
        }
        return userInput;
    }
    public static string GetPassword(string promt)
    {
        Console.Write(promt);
        string password = "";
        ConsoleKeyInfo key;
        do
        {
            key = Console.ReadKey(true);

            if (key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Tab && key.Key != ConsoleKey.Backspace)
            {
                password += key.KeyChar;
                Console.Write("*");
            }
            else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password[..^1];
                Console.Write("\b \b");
            }
        } while (key.Key != ConsoleKey.Enter);
        Console.WriteLine();

        return password;
    }
    public static double GetValidAmountOfMoney(string promt, string promtOnFaliure)
    {
        if (!Double.TryParse(GetUserInput(promt, promtOnFaliure), out var result) || result < 0)
        {
            Console.WriteLine(promtOnFaliure);
            return GetValidAmountOfMoney(promt, promtOnFaliure);
        }
        var stringResult = result.ToString();
        int decimalPlaces = stringResult.Length - stringResult.IndexOf('.') - 1;
        if (decimalPlaces > 2 && stringResult.Contains('.'))
        {
            Console.WriteLine(promtOnFaliure);
            return GetValidAmountOfMoney(promt, promtOnFaliure);
        }
        return result;
    }
    public static int GetValidAmmountOfTime(string promt, string promtOnFaliure)
    {
        if (!int.TryParse(GetUserInput(promt, promtOnFaliure), out var result) || result < 0)
        {
            return GetValidAmmountOfTime(promt, promtOnFaliure);
        }
        return result;
    }
    private static string GetValidPassword()
    {
        string pattern = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*[^a-zA-Z0-9]).{8,}$";

        var password = GetPassword("Enter password: ");
        if (!Regex.IsMatch(password, pattern))
        {
            Console.WriteLine($"Password not strong enough. " +
                $"\nit needs at least 1 uppercase letter, 1 lowercase letter, 1 number and a special character\n");
            return GetValidPassword();
        }

        var confirmation = GetPassword("Enter password again: ");

        if (password != confirmation)
        {
            Console.WriteLine("Passwords doesn't match, try again");
            return GetValidPassword();
        }


        return password!;
    }

    private static string GetValidAccount(string promt, string promtOnFailure)
    {
        var account_id = GetUserInput(promt, promtOnFailure); 
        if (!AccountProcessor.GetAccountExists(account_id).Result)
        {
            Console.WriteLine("Account does not exists"); 
            GetValidAccount(promt, promtOnFailure);
        }
        return account_id;
    }
    #endregion
}

