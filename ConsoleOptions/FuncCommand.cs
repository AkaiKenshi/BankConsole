using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleOptions;
public readonly struct FuncCommand<T>
{
    private readonly Func<T> onSelected;
    public string Prompt { get; init; }
    public FuncCommand(string prompt, Func<T> onSelected)
    {
        Prompt = prompt;
        this.onSelected = onSelected;
    }

    public T RunCommand() => onSelected.Invoke();
}