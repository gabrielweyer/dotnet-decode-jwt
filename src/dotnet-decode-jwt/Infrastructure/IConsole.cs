namespace Gabo.DotNet.DecodeJwt.Infrastructure;

public interface IConsole
{
    void WriteBoringLine(string value);
    void WriteDullLine(string value);
    void WriteInfoLine(string value);
    void WriteFancyLine(string value);
    void WriteErrorLine(string value);
}
