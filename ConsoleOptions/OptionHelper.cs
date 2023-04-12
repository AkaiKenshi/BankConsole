namespace ConsoleOptions;

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
        SelectAcctionCommand(options);

        static void SelectAcctionCommand(ActionCommand[] options)
        {
            try
            {
                var userInput = Console.ReadLine() ?? throw new NullReferenceException();
                var optionSelected = int.Parse(userInput);
                options[optionSelected - 1].RunCommand();
            }
            catch (Exception e) when (e is NullReferenceException || e is FormatException || e is IndexOutOfRangeException)
            {
                Console.WriteLine("please select a valid option");
                SelectAcctionCommand(options);
            }
        }

    }

    public static T RunFuncCommand<T>(string initialPrompt, params FuncCommand<T>[] options)
    {
        if (options == null || options.Length == 0)
        {
            throw new ArgumentException("At least one argument is required.", nameof(options));
        }

        Console.WriteLine(initialPrompt);

        for (int i = 1; i <= options.Length; i++) Console.WriteLine("   " + i + ": " + options[i - 1].Prompt);
        return SelectFuncCommand(options);

        static T SelectFuncCommand(FuncCommand<T>[] options)
        {
            try
            {
                var userInput = Console.ReadLine() ?? throw new NullReferenceException();
                var optionSelected = int.Parse(userInput);
                return options[optionSelected - 1].RunCommand();
            }
            catch (Exception e) when (e is NullReferenceException || e is FormatException || e is IndexOutOfRangeException)
            {
                Console.WriteLine("please select a valid option");
                return SelectFuncCommand(options);
            }
        }

    }
}
