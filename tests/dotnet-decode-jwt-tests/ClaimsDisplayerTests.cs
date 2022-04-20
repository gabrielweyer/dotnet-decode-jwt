using System.Globalization;
using System.Runtime.InteropServices;

namespace Gabo.DotNet.DecodeJwt.Tests;

public class ClaimsDisplayerTests
{
    private readonly ClaimsDisplayer _target;

    private readonly MockConsole _console;
    private readonly string _timeZoneDisplayName;

    public ClaimsDisplayerTests()
    {
        _console = new MockConsole();
        var timeZoneIdentifier = "Australia/Melbourne";

#if NETCOREAPP3_1
        _timeZoneDisplayName = "Australian Eastern Standard Time";
#else
        _timeZoneDisplayName = "Australian Eastern Time (Melbourne)";
#endif

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            timeZoneIdentifier = "AUS Eastern Standard Time";
            _timeZoneDisplayName = "Canberra, Melbourne, Sydney";
        }

        _target = new ClaimsDisplayer(_console, TimeZoneInfo.FindSystemTimeZoneById(timeZoneIdentifier));
    }

    static ClaimsDisplayerTests()
    {
        var cultureInfo = new CultureInfo("en-AU");

        CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
        CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
    }

    [Fact]
    public void GivenNoClaim_WhenDisplayClaims_ThenMessage()
    {
        // Arrange
        var claims = new JsonElement();

        // Act
        _target.DisplayClaims(claims);

        // Assert
        var expected = new List<string>
        {
            "SET FOREGROUND COLOR: DarkGray",
            "WRITE: There was no claims in the JWT.",
            "RESET COLOR"
        };

        _console.Actions.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void GivenAnyClaim_WhenDisplayClaims_ThenDisplayClaims()
    {
        // Arrange
        var claims = JsonDocument.Parse(@"{""iat"":1516239022}");

        // Act
        _target.DisplayClaims(claims.RootElement);

        // Assert
        var expected = new List<string>
        {
            "WRITE: ",
            "SET FOREGROUND COLOR: Yellow",
            "WRITE: Expiration Time (exp): N/A",
            "WRITE: Not Before (nbf): N/A",
            $"WRITE: Issued At (iat): Thursday, 18 January 2018 01:30:22 UTC / Thursday, 18 January 2018 12:30:22 (UTC+10:00) {_timeZoneDisplayName}",
            "SET FOREGROUND COLOR: Green",
            "WRITE: ",
            "WRITE: Claims are:",
            "WRITE: ",
            "RESET COLOR",
            $"WRITE: {{{Environment.NewLine}  \"iat\": 1516239022{Environment.NewLine}}}",
            "RESET COLOR"
        };

        _console.Actions.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void GivenIatIsNotTimestamp_WhenDisplayClaims_ThenDisplayDateAsNotAvailable()
    {
        // Arrange
        var claims = JsonDocument.Parse(@"{""iat"":""hello""}");

        // Act
        _target.DisplayClaims(claims.RootElement);

        // Assert
        var expected = new List<string>
        {
            "WRITE: ",
            "SET FOREGROUND COLOR: Yellow",
            "WRITE: Expiration Time (exp): N/A",
            "WRITE: Not Before (nbf): N/A",
            "WRITE: Issued At (iat): N/A",
            "SET FOREGROUND COLOR: Green",
            "WRITE: ",
            "WRITE: Claims are:",
            "WRITE: ",
            "RESET COLOR",
            $"WRITE: {{{Environment.NewLine}  \"iat\": \"hello\"{Environment.NewLine}}}",
            "RESET COLOR"
        };

        _console.Actions.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void GivenHtmlSensitiveCharacter_WhenDisplayClaims_ThenDoNotEscape()
    {
        // Arrange
        var claims = JsonDocument.Parse(@"{""hi"":""I'm""}");

        // Act
        _target.DisplayClaims(claims.RootElement);

        // Assert
        var expected = new List<string>
        {
            "WRITE: ",
            "SET FOREGROUND COLOR: Yellow",
            "WRITE: Expiration Time (exp): N/A",
            "WRITE: Not Before (nbf): N/A",
            "WRITE: Issued At (iat): N/A",
            "SET FOREGROUND COLOR: Green",
            "WRITE: ",
            "WRITE: Claims are:",
            "WRITE: ",
            "RESET COLOR",
            $"WRITE: {{{Environment.NewLine}  \"hi\": \"I'm\"{Environment.NewLine}}}",
            "RESET COLOR"
        };

        _console.Actions.Should().BeEquivalentTo(expected);
    }
}

internal class MockConsole : IConsole
{
    private readonly List<string> _actions = new List<string>();

    public IReadOnlyList<string> Actions => _actions;

    public ConsoleColor ForegroundColor
    {
        set => _actions.Add($"SET FOREGROUND COLOR: {value}");
    }

    public void WriteLine(string value)
    {
        _actions.Add($"WRITE: {value}");
    }

    public void ResetColor()
    {
        _actions.Add("RESET COLOR");
    }
}
