using System.Text.Encodings.Web;

namespace Gabo.DotNet.DecodeJwt;

internal class ClaimsDisplayer
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
        if (claims.ValueKind == JsonValueKind.Undefined)
        {
            _console.WriteDullLine("There was no claims in the JWT.");
        }
        else
        {
            _console.WriteBoringLine(string.Empty);
            _console.WriteInfoLine($"Expiration Time ({ExpirationTimeKeyName}): {FormatDateTime(claims, ExpirationTimeKeyName)}");
            _console.WriteInfoLine($"Not Before ({NotBeforeKeyName}): {FormatDateTime(claims, NotBeforeKeyName)}");
            _console.WriteInfoLine($"Issued At ({IssuedAtKeyName}): {FormatDateTime(claims, IssuedAtKeyName)}");
            _console.WriteFancyLine(string.Empty);
            _console.WriteFancyLine("Claims are:");
            _console.WriteFancyLine(string.Empty);
            _console.WriteBoringLine(JsonSerializer.Serialize(claims, _serializationOptions));
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
