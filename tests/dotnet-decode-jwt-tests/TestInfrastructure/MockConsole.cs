using Gabo.DotNet.DecodeJwt.Infrastructure;

namespace Gabo.DotNet.DecodeJwt.Tests.TestInfrastructure;

internal sealed class MockConsole : IConsole
{
    private readonly List<string> _actions = new();

    public IReadOnlyList<string> Actions => _actions;

    public void WriteBoringLine(string value)
    {
        WriteColoredLine(NonThreadSafeConsole.BoringColor, value);

    }

    public void WriteDullLine(string value)
    {
        WriteColoredLine(NonThreadSafeConsole.DullColor, value);
    }

    public void WriteInfoLine(string value)
    {
        WriteColoredLine(NonThreadSafeConsole.InfoColor, value);
    }

    public void WriteFancyLine(string value)
    {
        WriteColoredLine(NonThreadSafeConsole.FancyColor, value);
    }

    public void WriteErrorLine(string value)
    {
        WriteColoredLine(NonThreadSafeConsole.ErrorColor, value);
    }

    private void WriteColoredLine(ConsoleColor color, string value)
    {
        _actions.Add($"SET FOREGROUND COLOR: {color}");
        _actions.Add($"WRITE: {value}");
        _actions.Add("RESET COLOR");
    }
}
