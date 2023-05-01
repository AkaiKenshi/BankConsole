
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
                OptionHelper.RunActionCommand(
                        "Select a Option",
                        new ActionCommand("Create User Account", () => CreateUserAccount()),
                        new ActionCommand("LogIn to User Account", () => LogInUserAccount()),
                        new ActionCommand("Pass Time", () => throw new NotImplementedException()),
                        new ActionCommand("Exit Application", () => KeepRunningApp = false));
            }
            catch (OperationCanceledException) { Console.WriteLine("returning to start..."); }
        }
    }

    //Command Functions
    private static void LogInUserAccount()
    {
        try
        {

            Console.WriteLine("Please Input your user name");
            var username = GetUserInput("write your Username: ", "Invalid Username, Try Again");
            Console.WriteLine("Write Your Password");
            var password = GetPassword("Enter password: ");

            var customer = CustomerProcessor.LoginUserAsync(new GetCustomerLoginRequestDTO(username, password)).Result;
            AccessAccount(customer);
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
        AccessAccount(customer);
    }

    private static void AccessAccount(Customer customer)
    {
        var KeepRunning = true;
        while (KeepRunning)
        {
            OptionHelper.RunActionCommand(
                "Would you like to: ",
                new ActionCommand("Create New Bank Account", () => CreateBankAccount(customer)),
                new ActionCommand("Access Bank Account", () => AccessBankAccount(customer)),
                new ActionCommand("Go back", () => KeepRunning = false));
        }
    }

    private static void CreateBankAccount(Customer customer)
    {

        var account = OptionHelper.RunFuncCommand(
            "What Type of account would you like to create",
            new FuncCommand<Account>("Checking Account", () => chekingAccount()),
            new FuncCommand<Account>("Savings Account", () => savingsAccount()),
            new FuncCommand<Account>("Fixed Term Investment Account", () => longTermInvestmentAccount())
            );


        Account chekingAccount()
        {
            var moneyAmmounut = GetValidAmmountOfMoney("Enter the initial balance: ", "invalid Balance");
            var createRequest = new CreateCheckingAccountRequestDTO(moneyAmmounut);
            return AccountProcessor.
                CreateAccountAsync<CreateCheckingAccountRequestDTO>(createRequest, "/api/Account/createCheckingAccount", customer.Token).Result;
        }
        Account savingsAccount()
        {
            var moneyAmmounut = GetValidAmmountOfMoney("Enter the initial balance: ", "invalid Balance");
            var createRequest = new CreateSavingsAccountRequestDTO(moneyAmmounut);
            return AccountProcessor.
                CreateAccountAsync<CreateSavingsAccountRequestDTO>(createRequest, "/api/Account/createSavingsAccount", customer.Token).Result;
        }
        Account longTermInvestmentAccount()
        {
            var moneyAmmounut = GetValidAmmountOfMoney("Enter the initial balance: ", "invalid Balance");
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
            return OptionHelper.RunFuncCommand(
                "Try Again",
                new FuncCommand<string>("Yes", () => GetAvailableId()),
                new FuncCommand<string>("No", () => throw new OperationCanceledException()));
        }

        return userInput;
    }
    private static string GetAvailableUsername()
    {
        var userInput = GetUserInput("Write your Username: ", "Invalid username, Try Again");
        if (!CustomerProcessor.IsCustomerUsernameAvailableAsync(userInput).Result)
        {
            Console.WriteLine("Invalid username or already taken");
            return OptionHelper.RunFuncCommand(
                "Try Again",
                new FuncCommand<string>("Yes", () => GetAvailableUsername()),
                new FuncCommand<string>("No", () => throw new OperationCanceledException()));
        }
        return userInput;
    }
    private static string GetAvailableEmail()
    {
        var userInput = GetUserInput("Write your Email: ", "Invalid Email, Try Again");
        if (!CustomerProcessor.IsCustomerUsernameAvailableAsync(userInput).Result)
        {
            Console.WriteLine("Invalid Email or already taken");
            return OptionHelper.RunFuncCommand(
                "Try Again",
                new FuncCommand<string>("Yes", () => GetAvailableEmail()),
                new FuncCommand<string>("No", () => throw new OperationCanceledException()));
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

    public static double GetValidAmmountOfMoney(string promt, string promtOnFaliure)
    {
        if (!Double.TryParse(GetUserInput(promt, promtOnFaliure), out var result) || result < 0)
        {
            return GetValidAmmountOfMoney(promt, promtOnFaliure);
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
                $"\nit needs at least 1 uppercase letter, 1 lowercase letter, 1 number and a special character");
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
    #endregion
}

