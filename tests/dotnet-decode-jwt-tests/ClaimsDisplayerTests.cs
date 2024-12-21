using System.Globalization;

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
        _timeZoneDisplayName = "Eastern Australia Time (Melbourne)";

        if (OperatingSystem.IsWindows())
        {
            timeZoneIdentifier = "AUS Eastern Standard Time";
            _timeZoneDisplayName = "Canberra, Melbourne, Sydney";
        }

        _target = new ClaimsDisplayer(_console, TimeZoneInfo.FindSystemTimeZoneById(timeZoneIdentifier));
    }

    static ClaimsDisplayerTests()
    {
        var cultureInfo = new CultureInfo("en-US");

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
            "SET FOREGROUND COLOR: Gray",
            "WRITE: ",
            "RESET COLOR",
            "SET FOREGROUND COLOR: Yellow",
            "WRITE: Expiration Time (exp): N/A",
            "RESET COLOR",
            "SET FOREGROUND COLOR: Yellow",
            "WRITE: Not Before (nbf): N/A",
            "RESET COLOR",
            "SET FOREGROUND COLOR: Yellow",
            $"WRITE: Issued At (iat): Thursday, January 18, 2018 01:30:22 UTC / Thursday, January 18, 2018 12:30:22 (UTC+10:00) {_timeZoneDisplayName}",
            "RESET COLOR",
            "SET FOREGROUND COLOR: Green",
            "WRITE: ",
            "RESET COLOR",
            "SET FOREGROUND COLOR: Green",
            "WRITE: Claims are:",
            "RESET COLOR",
            "SET FOREGROUND COLOR: Green",
            "WRITE: ",
            "RESET COLOR",
            "SET FOREGROUND COLOR: Gray",
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
            "SET FOREGROUND COLOR: Gray",
            "WRITE: ",
            "RESET COLOR",
            "SET FOREGROUND COLOR: Yellow",
            "WRITE: Expiration Time (exp): N/A",
            "RESET COLOR",
            "SET FOREGROUND COLOR: Yellow",
            "WRITE: Not Before (nbf): N/A",
            "RESET COLOR",
            "SET FOREGROUND COLOR: Yellow",
            "WRITE: Issued At (iat): N/A",
            "RESET COLOR",
            "SET FOREGROUND COLOR: Green",
            "WRITE: ",
            "RESET COLOR",
            "SET FOREGROUND COLOR: Green",
            "WRITE: Claims are:",
            "RESET COLOR",
            "SET FOREGROUND COLOR: Green",
            "WRITE: ",
            "RESET COLOR",
            "SET FOREGROUND COLOR: Gray",
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
            "SET FOREGROUND COLOR: Gray",
            "WRITE: ",
            "RESET COLOR",
            "SET FOREGROUND COLOR: Yellow",
            "WRITE: Expiration Time (exp): N/A",
            "RESET COLOR",
            "SET FOREGROUND COLOR: Yellow",
            "WRITE: Not Before (nbf): N/A",
            "RESET COLOR",
            "SET FOREGROUND COLOR: Yellow",
            "WRITE: Issued At (iat): N/A",
            "RESET COLOR",
            "SET FOREGROUND COLOR: Green",
            "WRITE: ",
            "RESET COLOR",
            "SET FOREGROUND COLOR: Green",
            "WRITE: Claims are:",
            "RESET COLOR",
            "SET FOREGROUND COLOR: Green",
            "WRITE: ",
            "RESET COLOR",
            "SET FOREGROUND COLOR: Gray",
            $"WRITE: {{{Environment.NewLine}  \"hi\": \"I'm\"{Environment.NewLine}}}",
            "RESET COLOR"
        };

        _console.Actions.Should().BeEquivalentTo(expected);
    }
}
