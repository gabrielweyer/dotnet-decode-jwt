using System;

namespace DotNet.Decode.Jwt
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("A single argument should be provided:");
                Console.WriteLine("dotnet decode-jwt \"SGVsbG8gV29ybGQh\"");
                return;
            }

            var console = new SimplifiedConsole();
            var claimsDisplayer = new ClaimsDisplayer(console);

            try
            {
                var claims = JwtClaimsDecoder.GetClaims(args[0]);
                claimsDisplayer.DisplayClaims(claims);
            }
            catch (FormatException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
            }
            finally
            {
                Console.ResetColor();
            }
        }
    }
}
