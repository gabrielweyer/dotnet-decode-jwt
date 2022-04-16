using System.Text.Encodings.Web;

namespace DotNet.Decode.Jwt;

public class ClaimsDisplayer
{
    private readonly IConsole _console;
    private readonly TimeZoneInfo _localTimeZone;

    private const string ExpirationTimeKeyName = "exp";
    private const string NotBeforeKeyName = "nbf";
    private const string IssuedAtKeyName = "iat";

    private static readonly DateTime _epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    private static readonly JsonSerializerOptions _serializationOptions = new JsonSerializerOptions
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        WriteIndented = true
    };

    public ClaimsDisplayer(IConsole console, TimeZoneInfo localTimeZone)
    {
        _console = console;
        _localTimeZone = localTimeZone;
    }

    public void DisplayClaims(JsonElement claims)
    {
        try
        {
            if (claims.ValueKind == JsonValueKind.Undefined)
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

                _console.WriteLine(JsonSerializer.Serialize(claims, _serializationOptions));
            }
        }
        finally
        {
            _console.ResetColor();
        }
    }

    private string FormatDateTime(JsonElement claims, string key)
    {
        if (!claims.TryGetProperty(key, out var token))
        {
            return "N/A";
        }

        if (token.ValueKind != JsonValueKind.Number || !token.TryGetInt32(out var timestamp))
        {
            return "N/A";
        }

        var utcTime = _epoch.AddSeconds(timestamp);

        var localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, _localTimeZone);

        return $"{FormatDateTime(utcTime)} UTC / {FormatDateTime(localTime)} {_localTimeZone.DisplayName}";
    }

    private static string FormatDateTime(DateTime date)
    {
        return $"{date.ToLongDateString()} {date:HH:mm:ss}";
    }
}
