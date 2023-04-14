
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
    private static void LogInUserAccount()
    {
        Console.WriteLine("Please Input your user name");
        var userName = GetUserName();

        if (!ValidateUserPassword(userName))
        {
            Console.WriteLine("Returning to Start");
            return;
        }
        var id = Customer.GetIdByUsername(userName).GetAwaiter().GetResult();
        AccessAccount(id);
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
        AccessAccount(id);
    }

    private static void AccessAccount(string id)
    {
        var KeepRunning = true;
        while (KeepRunning)
        {
            OptionHelper.RunActionCommand(
                "Would you like to: ",
                new ActionCommand("Create New Bank Account", () => CreateUserAccount(id)),
                new ActionCommand("Access Bank Account", () => AccessBankAccount(id)),
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

    //Helper Function
    private static string GetValidUserInput(string InputType)
    {
        var userInput = Console.ReadLine();
        if (userInput is null)
        {
            Console.WriteLine($"Invalid {InputType}, Try Again");
            return GetValidUserInput(InputType);
        }
        return userInput;
    }
    private static string GetUserName()
    {
        var username = Console.ReadLine();
        if (string.IsNullOrEmpty(username) || Customer.FindIfUserNameAvailable(username).GetAwaiter().GetResult())
        {
            Console.WriteLine("Invalid User Name, Try Again");
            return GetUserName();
        }
        return username;
    }
    private static string GetAvailableID()
    {
        try
        {
            var id = Console.ReadLine() ?? throw new NullReferenceException();
            if (Customer.IsValidID(id).GetAwaiter().GetResult())
                return id;

            Console.WriteLine("Id Already On Use");
        }
        catch (NullReferenceException)
        {
            Console.WriteLine("Invalid ID");
        }
        catch (HttpRequestException e)
        {
            if (e.StatusCode == null || (int)e.StatusCode >= 500) Console.WriteLine("Something Wrong Happened");
            else Console.WriteLine("Invalid ID");
        }
        return GetAvailableID();
    }
    private static string GetAvailableUserName()
    {
        var username = Console.ReadLine();
        if (string.IsNullOrEmpty(username))
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
        var password = ReadPassword();
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
        var password = ReadPassword();

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
    private static string ReadPassword()
    {
        string password = "";
        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey(true);

            if (key.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                break;
            }
            else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password.Remove(password.Length - 1);
                Console.Write("\b \b");
            }
            else if (char.IsLetterOrDigit(key.KeyChar) || char.IsSymbol(key.KeyChar) || char.IsPunctuation(key.KeyChar))
            {
                password += key.KeyChar;
                Console.Write("*");
            }
        }

       return password;
    }

}