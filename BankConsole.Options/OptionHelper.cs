namespace BankConsole.Options;

public static class OptionHelper
{
    public static void RunCommand(string initialPrompt, params (string prompt, Action onSelected)[] options)
    {
        if (options == null || options.Length == 0)
        {
            throw new ArgumentException("At least one argument is required.", nameof(options));
        }

        Console.WriteLine(initialPrompt);

        ShowOptions(options);

        var userInput = GetUserSelection(options.Length);
        options[userInput - 1].onSelected();
    }

    public static T RunCommand<T>(string initialPrompt, params (string prompt, Func<T> onSelected)[] options)
    {
        if (options == null || options.Length == 0)
        {
            throw new ArgumentException("At least one argument is required.", nameof(options));
        }

        Console.WriteLine(initialPrompt);
        ShowOptions(options);

        var userInput = GetUserSelection(options.Length);
        return options[userInput - 1].onSelected();
    }

    private static void ShowOptions<T>((string prompt, T onSelected)[] options)
    {
        for (int i = 1; i <= options.Length; i++) Console.WriteLine("   " + i + ": " + options[i - 1].prompt);
    }

    private static int GetUserSelection(int lenghtOption)
    {
        Console.Write("-> ");
        var userInput = Console.ReadLine();
        if (userInput == null || !int.TryParse(userInput, out var numericUserInput) || numericUserInput > lenghtOption || numericUserInput <= 0)
        {
            Console.WriteLine("Please Select a valid Option\n");
            return GetUserSelection(lenghtOption);
        }
        Console.WriteLine();
        return numericUserInput;
    }
}
