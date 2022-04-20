namespace Gabo.DotNet.DecodeJwt;

internal static class Program
{
    internal const int SuccessExitCode = 0;
    internal const int FailureExitCode = 1;

    internal static IConsole AbstractedConsole;

    internal static int Main(string[] args)
    {
        AbstractedConsole ??= new NonThreadSafeConsole();

        if (args.Length != 1)
        {
            AbstractedConsole.WriteBoringLine("A single argument should be provided:");
            AbstractedConsole.WriteBoringLine("dotnet decode-jwt eyJhbGciOiJub25lIn0.ewogICAgImlzcyI6ICJiZXN0LWlzc3VlciIsCiAgICAic3ViIjogIm5pY2Utc3ViamVjdCIsCiAgICAiYXVkIjogWyJhdWRpZW5jZS1vbmUiLCAiYXVkaWVuY2UtdHdvIl0sCiAgICAiZXhwIjogMTUyODY5MTM1MCwKICAgICJuYmYiOiAxNTI4NjkwNzUwLAogICAgImlhdCI6IDE1Mjg2OTA3NTAsCiAgICAianRpIjogImMzMTk3ZGNiLWUxMTMtNDc3OC04OTc5LWI5NTZmNjg0MDA3ZiIsCiAgICAiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvZW1haWxhZGRyZXNzIjogImhpQG1lLmNvbSIsCiAgICAic29tZS1udW1iZXIiOiAxMi41NiwKICAgICJuZXN0ZWQtY2xhaW0iOiB7CiAgICAgICAgImhpIjogIkknbSIsCiAgICAgICAgImEiOiAibmVzdGVkIGNsYWltIgogICAgfQp9Cg==.");
            return FailureExitCode;
        }

        var claimsDisplayer = new ClaimsDisplayer(AbstractedConsole, TimeZoneInfo.Local);

        try
        {
            var claims = JwtClaimsDecoder.GetClaims(args[0]);
            claimsDisplayer.DisplayClaims(claims);
            return SuccessExitCode;
        }
        catch (FormatException e)
        {
            AbstractedConsole.WriteErrorLine(e.Message);
            return FailureExitCode;
        }
    }
}
