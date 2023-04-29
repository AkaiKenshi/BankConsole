
using BankConsole.Contracts;
using BankConsole.Contracts.DTOs.Customers;
using BankConsole.Contracts.Models;
using BankConsole.Contracts.Processor;
using BankConsole.Options;

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
        Console.WriteLine("Please Input your user name");

        Console.WriteLine("Returning to Start");

    }

    private static void CreateUserAccount()
    {
        Console.WriteLine("Please Input your ID");
        var id = GetAvailableId();
        Console.WriteLine("Please Input your user name");
        var username = GetAvailableUsername();
        Console.WriteLine("Please Input your name");
        var firstName = GetUserInput("Write your first name: ","Invalid name, Try again");
        Console.WriteLine("Please Input your last name");
        var lastName = GetUserInput("Write your last name: ","Invalid last name, Try again");
        Console.WriteLine("Please Input your Email");
        var email = GetAvailableEmail();
        Console.WriteLine("Please Input your password");
        var password = GetValidPassword();

        var createRequest  = new CreateCustomerRequestDTO(id, username, firstName, lastName,email, password);
        var customer = new CustomerProcessor().RegisterUser(createRequest).Result; 
        AccessAccount(customer);
    }

    private static void AccessAccount(Customer customer)
    {
        var KeepRunning = true;
        while (KeepRunning)
        {
            OptionHelper.RunActionCommand(
                "Would you like to: ",
                new ActionCommand("Create New Bank Account", () => CreateUserAccount(customer.Token)),
                new ActionCommand("Access Bank Account", () => AccessBankAccount(customer.Token)),
                new ActionCommand("Go back", () => KeepRunning = false));
        }
    }

    private static void CreateUserAccount(string id)
    {
        throw new NotImplementedException();
    }

    private static void AccessBankAccount(string id)
    {
        throw new NotImplementedException();
    }

    #region HelperFunctions
    private static string GetUserInput(string promnt, string promntOnFail)
    {
        Console.Write(promnt);
        var userInput = Console.ReadLine();

        if ( String.IsNullOrWhiteSpace(userInput))
        {
            Console.WriteLine(promntOnFail);
            return GetUserInput(promnt,promntOnFail);
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
        else if (!new CustomerProcessor().IsCustomerIdAvailable(userInput).Result)
        {
            Console.WriteLine("Id already taken");
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
        if (!new CustomerProcessor().IsCustomerUsernameAvailable(userInput).Result)
        {
            Console.WriteLine("username already taken");
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
        if (!new CustomerProcessor().IsCustomerUsernameAvailable(userInput).Result)
        {
            Console.WriteLine("Email already taken");
            return OptionHelper.RunFuncCommand(
                "Try Again",
                new FuncCommand<string>("Yes", () => GetAvailableEmail()),
                new FuncCommand<string>("No", () => throw new OperationCanceledException()));
        }
        return userInput;
    }
    private static string GetValidPassword()
    {
        var userInput = GetUserInput("Write your password: ", "Invalid Password Try again");
        var confirmation = GetUserInput("Write Again your Password: ", "Invalid Password Try again");

        if(userInput != confirmation)
        {
            Console.WriteLine("Passwords dosen't match, try again"); 
            return GetValidPassword();
        }


        return userInput!; 
    }
    #endregion
}