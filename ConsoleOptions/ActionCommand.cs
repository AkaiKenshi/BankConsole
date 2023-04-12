namespace ConsoleOptions;
public readonly struct ActionCommand
{
    private readonly Action onSelected;
    public string Prompt { get; init; }
    public ActionCommand(string prompt, Action onSelected)
    {
        Prompt = prompt;
        this.onSelected = onSelected;
    }

    public void RunCommand() => onSelected?.Invoke();
}

