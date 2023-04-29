
using BankConsole.Contracts;
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

        Console.WriteLine("Returning to Start");

        }

    private static void CreateUserAccount()
    {
        Console.WriteLine("Please Input your ID");
        var id = GetAvailableId();
        Console.WriteLine("Please Input your user name");
        //var username = GetAvailableUserName();
        Console.WriteLine("Please Input your name");
        //var name = GetValidUserInput("Name");
        Console.WriteLine("Please Input your last name");
        //var lastName = GetValidUserInput("Last Name");
        Console.WriteLine("Please Input your password");
        //var password = GetValidPassword();

        //Customer.CreateCustomer(id, username, name, lastName, password).GetAwaiter().GetResult();
        //AccessAccount(id);
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

    #region HelperFunctions

    private static string GetAvailableId()
    {
        Console.Write("Write your Id: ");
        var userInput = Console.ReadLine();
        
        if (userInput == null || userInput.Length != 10 || !int.TryParse(userInput, out _))
        {
            Console.WriteLine("Invalid id, Try Again"); 
            return GetAvailableId();
        }

        return userInput;
    }

    #endregion
}