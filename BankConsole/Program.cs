
using BankGetData.Customer;
using ConsoleOptions;
using System.Globalization;

namespace BankConsole;

internal class Program
{
    private static void Main(string[] args)
    {
        bool KeepRunningApp = true;
        while (KeepRunningApp)
        {
            OptionHelper.RunActionCommand(
                    "Select a Option",
                    new ActionCommand("Create User Account", () => CreateUserAccount()),
                    new ActionCommand("LogIn to User Account", () => LogInUserAccount()),
                    new ActionCommand("Pass Time", () => throw new NotImplementedException()),
                    new ActionCommand("Exit Application", () => KeepRunningApp = false));
        }
    }

    //Command Functions
    private  static void LogInUserAccount()
    {
        Console.WriteLine("Please Input your user name");
        var username = GetUserName();

        if (!ValidateUserPassword(username))
        {
            //TODO handle Invalid Password
            return;
        }
        //TODO handle Login Request
    }
    private static void CreateUserAccount()
    {
        Console.WriteLine("Please Input your ID");
        var id = GetAvailableID();
        Console.WriteLine("Please Input your user name");
        var username = GetAvailableUserName();
        Console.WriteLine("Please Input your name");
        var name = GetValidUserInput("Name");
        Console.WriteLine("Please Input your last name");
        var lastName = GetValidUserInput("Last Name");
        Console.WriteLine("Please Input your password");
        var password = GetValidPassword();

        Customer.CreateCustomer(id, username, name, lastName, password).GetAwaiter().GetResult(); 
    }


    //Helper Function
    private static string GetValidUserInput(string InputType)
    {
        var userInput = Console.ReadLine();
        if (userInput is null )
        {
            Console.WriteLine($"Invalid {InputType}, Try Again");
            return GetValidUserInput(InputType);
        }
        return userInput;
    }
    private static string GetUserName()
    {
        var username = Console.ReadLine();
        if (string.IsNullOrEmpty(username))
        {
            Console.WriteLine("Invalid User Name, Try Again");
            return GetUserName();
        } 
        else if (Customer.FindIfUserNameAvailable(username).GetAwaiter().GetResult())
        {
            Console.WriteLine("User Name Already Taken, Try Again");
            return GetUserName();
        }
        return username;
    }
    private static string GetAvailableID()
    {
        var id = Console.ReadLine();
        if (string.IsNullOrEmpty(id))
        {
            Console.WriteLine("Invalid ID"); 
            return GetAvailableID();
        }
        else if (!Customer.IsValidID(id).GetAwaiter().GetResult())
        {
            Console.WriteLine("user with this id already exists");
            return GetAvailableID();
        }
        return id;
    }
    private static string GetAvailableUserName()
    {
        var username = Console.ReadLine();
        if(string.IsNullOrEmpty(username))
        {
            Console.WriteLine("Invalid User Name");
            return GetAvailableUserName();
        }
        else if (!Customer.FindIfUserNameAvailable(username).GetAwaiter().GetResult())
        {
            Console.WriteLine("User Name Already Taken, please try another one");
            return GetAvailableUserName();
        }
        return username;
    }
    private static string GetValidPassword()
    {
        var password = Console.ReadLine();
        if (string.IsNullOrEmpty(password) || !Customer.ValidatePassword(password))
        {
            Console.WriteLine("Invalid Password, please try again");
            return GetValidPassword();
        }
        return password;
    }
    private static bool ValidateUserPassword(string username)
    {
        Console.WriteLine("Please Input your Password");
        var password = Console.ReadLine();

        if (password is null || !Customer.IsValidPassword(username, password).GetAwaiter().GetResult())
        {
            Console.WriteLine("Invalid Password");
            return OptionHelper.RunFuncCommand(
                "Try Again?",
                new FuncCommand<bool>("Yes", () => ValidateUserPassword(username)),
                new FuncCommand<bool>("No", () => false));
        }
        return true;
    }

}