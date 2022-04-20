namespace Gabo.DotNet.DecodeJwt;

public interface IConsole
{
    ConsoleColor ForegroundColor { set; }
    void WriteLine(string value);
    void ResetColor();
}

internal class SimplifiedConsole : IConsole
{
    public ConsoleColor ForegroundColor
    {
        set => Console.ForegroundColor = value;
    }

    public void WriteLine(string value)
    {
        Console.WriteLine(value);
    }

    public void ResetColor()
    {
        Console.ResetColor();
    }
}
