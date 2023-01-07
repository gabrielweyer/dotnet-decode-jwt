namespace Gabo.DotNet.DecodeJwt.Infrastructure;

internal class NonThreadSafeConsole : IConsole
{
    internal const ConsoleColor BoringColor = ConsoleColor.Gray;
    internal const ConsoleColor DullColor = ConsoleColor.DarkGray;
    internal const ConsoleColor InfoColor = ConsoleColor.Yellow;
    internal const ConsoleColor FancyColor = ConsoleColor.Green;
    internal const ConsoleColor ErrorColor = ConsoleColor.Red;

    public void WriteBoringLine(string value)
    {
        WriteColoredLine(BoringColor, value);
    }

    public void WriteDullLine(string value)
    {
        WriteColoredLine(DullColor, value);
    }

    public void WriteInfoLine(string value)
    {
        WriteColoredLine(InfoColor, value);
    }

    public void WriteFancyLine(string value)
    {
        WriteColoredLine(FancyColor, value);
    }

    public void WriteErrorLine(string value)
    {
        WriteColoredLine(ErrorColor, value);
    }

    private static void WriteColoredLine(ConsoleColor color, string value)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(value);
        Console.ResetColor();
    }
}
