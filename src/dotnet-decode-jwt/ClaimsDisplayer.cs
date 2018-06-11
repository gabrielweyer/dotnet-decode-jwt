using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DotNet.Decode.Jwt
{
    public class ClaimsDisplayer
    {
        private readonly IConsole _console;

        private const string ExpirationTimeKeyName = "exp";
        private const string NotBeforeKeyName = "nbf";
        private const string IssuedAtKeyName = "iat";

        private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

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
                    _console.WriteLine(string.Empty);
                    _console.ForegroundColor = ConsoleColor.Yellow;
                    _console.WriteLine($"Expiration Time ({ExpirationTimeKeyName}): {FormatDateTime(claims, ExpirationTimeKeyName)}");
                    _console.WriteLine($"Not Before ({NotBeforeKeyName}): {FormatDateTime(claims, NotBeforeKeyName)}");
                    _console.WriteLine($"Issued At ({IssuedAtKeyName}): {FormatDateTime(claims, IssuedAtKeyName)}");
                    _console.ForegroundColor = ConsoleColor.Green;
                    _console.WriteLine(string.Empty);
                    _console.WriteLine("Claims are:");
                    _console.WriteLine(string.Empty);
                    _console.ResetColor();

                    _console.WriteLine(JsonConvert.SerializeObject(claims, Formatting.Indented));
                }
            }
            finally
            {
                _console.ResetColor();
            }
        }

        private static string FormatDateTime(JObject claims, string key)
        {
            if (!claims.TryGetValue(key, out var token)) return "N/A";

            var timestamp = token.Value<int>();

            var utcTime = epoch.AddSeconds(timestamp);

            var timeZone = TimeZoneInfo.Local;
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, timeZone);

            return $"{FormatDateTime(utcTime)} UTC / {FormatDateTime(localTime)} {timeZone.DisplayName}";
        }

        private static string FormatDateTime(DateTime date)
        {
            return $"{date.ToLongDateString()} {date:HH:mm:ss}";
        }
    }
}
