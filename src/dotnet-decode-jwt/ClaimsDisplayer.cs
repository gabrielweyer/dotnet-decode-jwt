using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNet.Decode.Jwt
{
    public class ClaimsDisplayer
    {
        private readonly IConsole _console;

        private static readonly IReadOnlyList<string> DateClaims = new List<string> { "exp", "nbf", "iat"};

        public ClaimsDisplayer(IConsole console)
        {
            _console = console;
        }

        public void DisplayClaims(IDictionary<string, string> claims)
        {
            try
            {
                if (claims.Count == 0)
                {
                    _console.ForegroundColor = ConsoleColor.DarkGray;
                    _console.WriteLine("There was no claims in the JWT.");
                }
                else
                {
                    _console.ForegroundColor = ConsoleColor.Green;
                    _console.WriteLine("Claims are:");
                    _console.ResetColor();
                    _console.WriteLine("{");

                    var fieldSeparator = ",";
                    var index = 1;

                    foreach (var claim in claims)
                    {
                        if (index == claims.Count) { fieldSeparator = string.Empty; }

                        _console.WriteLine($"\t\"{claim.Key}\": {GetFormattedValue(claim.Key, claim.Value)}{fieldSeparator}");
                        index++;
                    }

                    _console.WriteLine("}");
                }
            }
            finally
            {
                _console.ResetColor();
            }
        }

        private static string GetFormattedValue(string key, string value)
        {
            return DateClaims.Contains(key) || value[0] == '[' ? value : $"\"{value}\"";
        }
    }
}
