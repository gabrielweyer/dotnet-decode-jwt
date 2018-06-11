using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

        public void DisplayClaims(JObject claims)
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

                    _console.WriteLine(JsonConvert.SerializeObject(claims, Formatting.Indented));
                }
            }
            finally
            {
                _console.ResetColor();
            }
        }
    }
}
