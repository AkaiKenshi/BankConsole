
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
                new ActionCommand("Create User Account", () => LogInUserAccount()),
                new ActionCommand("LogIn to User Account", () => CreateUserAccount()),
                new ActionCommand("Pass Time", () => throw new NotImplementedException()),
                new ActionCommand("Exit Application", () => KeepRunningApp = false));
        }
    }

    private static void LogInUserAccount()
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
        Console.WriteLine("Please Input your user name");
        var username = GetAvailableUserName();
        var name = GetValidUserInput("Name");
        var lastName = GetValidUserInput("Last Name");
        var id = GetAvailableID();
        var password = GetValidPassword();

        // TODO Create customer in database 
    }

    private static string GetValidUserInput(string InputType)
    {
        var userInput = Console.ReadLine();
        if (userInput is null)
        {
            Console.WriteLine($"Invalid {InputType}, Try Again");
            return GetUserName();
        }
        return userInput;
    }
    private static string GetUserName()
    {
        var username = Console.ReadLine();
        if (username is null || !Customer.FindIfValidUserName(username))
        {
            Console.WriteLine("Invalid User Name, Try Again");
            return GetUserName();
        }
        return username;
    }
    private static string GetAvailableID ()
    {
        var id = Console.ReadLine();
        if (id is null || Customer.isValidID(id))
        {
            Console.WriteLine("user with this id already exists"); 
            return GetAvailableID();
        }
        return id;
    }
    private static string GetAvailableUserName()
    {
        var username = Console.ReadLine();
        if (username is null || Customer.FindIfValidUserName(username))
        {
            Console.WriteLine("User Name Already Taken, please try another one");
            return GetUserName();
        }
        return username;
    }
    private static string GetValidPassword()
    {
        var password = Console.ReadLine();
        if(password is null || !Customer.ValidatePassword(password))
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

        if (password is null || !Customer.IsValidPassword(username, password))
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