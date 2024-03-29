namespace Gabo.DotNet.DecodeJwt.Tests;

public class JwtClaimsDecoderTests
{
    private const string JwtWithoutPeriods = "cQ==";
    private const string JwtWithOnlyOnePeriod = "cQ==.cQ==";

    [Theory]
    [InlineData(JwtWithoutPeriods)]
    [InlineData(JwtWithOnlyOnePeriod)]
    public void GivenJwtWithLessThanTwoDots_WhenGetClaims_ThenThrows(string jwt)
    {
        // Act
        var exception = Record.Exception(() => JwtClaimsDecoder.GetClaims(jwt));

        // Assert
        Assert.IsType<FormatException>(exception);
        Assert.Equal("The JWT should contain two periods.", exception.Message);
    }

    [Fact]
    public void GivenNotBase64EncodedClaimsSet_WhenGetClaims_ThenThrows()
    {
        // Act
        var exception = Record.Exception(() => JwtClaimsDecoder.GetClaims("cQ==.invalid|base|64.cQ=="));

        // Assert
        Assert.IsType<FormatException>(exception);
    }

    [Fact]
    public void GivenClaimsSetIsNotJson_WhenGetClaims_ThenThrows()
    {
        // Act
        var exception = Record.Exception(() => JwtClaimsDecoder.GetClaims("cQ==.cQ==.cQ=="));

        // Assert
        Assert.IsType<FormatException>(exception);
    }

    [Fact]
    public void GivenIatIsNumber_WhenGetClaims_ThenReturnClaims()
    {
        // Arrange
        const string jwt = "eyJhbGciOiJub25lIn0.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.";

        // Act
        var actualClaims = JwtClaimsDecoder.GetClaims(jwt);

        // Assert
        var expectedClaims =
            JsonDocument.Parse(@"{""sub"":""1234567890"",""name"":""John Doe"",""iat"":1516239022}");

        actualClaims.GetRawText().Should().Be(expectedClaims.RootElement.GetRawText());
    }

    [Fact]
    public void GivenAudIsArrayOfString_WhenGetClaims_ThenReturnClaims()
    {
        // Arrange
        const string jwt = "eyJhbGciOiJub25lIn0.eyJhdWQiOlsiYXVkaWVuY2Utb25lIiwiYXVkaWVuY2UtdHdvIl19.";

        // Act
        var actualClaims = JwtClaimsDecoder.GetClaims(jwt);

        // Assert
        var expectedClaims = JsonDocument.Parse(@"{""aud"":[""audience-one"",""audience-two""]}");

        actualClaims.GetRawText().Should().Be(expectedClaims.RootElement.GetRawText());
    }

    [Fact]
    public void GivenAudIsSingleString_WhenGetClaims_ThenReturnClaims()
    {
        // Arrange
        const string jwt = "eyJhbGciOiJub25lIn0.eyJhdWQiOiJhdWRpZW5jZSJ9.";

        // Act
        var actualClaims = JwtClaimsDecoder.GetClaims(jwt);

        // Assert
        var expectedClaims = JsonDocument.Parse(@"{""aud"":""audience""}");

        actualClaims.GetRawText().Should().Be(expectedClaims.RootElement.GetRawText());
    }

    [Fact]
    public void GivenHtmlSensitiveCharacter_WhenGetClaims_ThenReturnUnescapedClaims()
    {
        // Arrange
        const string jwt = "eyJhbGciOiJub25lIn0.eyJoaSI6IkknbSJ9.";

        // Act
        var actualClaims = JwtClaimsDecoder.GetClaims(jwt);

        // Assert
        var expectedClaims = JsonDocument.Parse(@"{""hi"":""I'm""}");

        actualClaims.GetRawText().Should().Be(expectedClaims.RootElement.GetRawText());
    }

    [Fact]
    public void GivenClaimKeyIsXmlNamespace_WhenGetClaims_ThenReturnClaims()
    {
        // Arrange
        const string jwt = "eyJhbGciOiJub25lIn0.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJoaUBtZS5jb20ifQ==.";

        // Act
        var actualClaims = JwtClaimsDecoder.GetClaims(jwt);

        // Assert
        var expectedClaims = JsonDocument.Parse(
            @"{""http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"":""hi@me.com""}");

        actualClaims.GetRawText().Should().Be(expectedClaims.RootElement.GetRawText());
    }
}
