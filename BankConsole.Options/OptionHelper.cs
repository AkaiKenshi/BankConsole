using System.Security.Cryptography.X509Certificates;

namespace BankConsole.Options;

public static class OptionHelper
{
    public static void RunActionCommand(string initialPrompt, params ActionCommand[] options)
    {
        if (options == null || options.Length == 0)
        {
            throw new ArgumentException("At least one argument is required.", nameof(options));
        }

        Console.WriteLine(initialPrompt);

        for (int i = 1; i <= options.Length; i++) Console.WriteLine("   " + i + ": " + options[i - 1].Prompt);

        var userInput = GetUserSelection(options.Length);
        options[userInput - 1].RunCommand();

    }

    public static T RunFuncCommand<T>(string initialPrompt, params FuncCommand<T>[] options)
    {
        if (options == null || options.Length == 0)
        {
            throw new ArgumentException("At least one argument is required.", nameof(options));
        }

        Console.WriteLine(initialPrompt);

        for (int i = 1; i <= options.Length; i++) Console.WriteLine("   " + i + ": " + options[i - 1].Prompt);

        var userInput = GetUserSelection(options.Length);
        return options[userInput - 1].RunCommand();

    }

    private static int GetUserSelection(int lenghtOption)
    {
        var userInput = Console.ReadLine();
        var numericUserInput = 0;
        if (userInput == null || !int.TryParse(userInput, out numericUserInput) || numericUserInput > lenghtOption || numericUserInput <= 0)
        {
            Console.WriteLine("Please Select a valid Option");
            return GetUserSelection(lenghtOption);
        }
        return numericUserInput;
    }
}
