namespace Gabo.DotNet.DecodeJwt;

public static class JwtClaimsDecoder
{
    public static JsonElement GetClaims(string jwt)
    {
        var base64UrlClaimsSet = GetBase64UrlClaimsSet(jwt);
        var claimsSet = DecodeBase64Url(base64UrlClaimsSet);

        try
        {
            using var jsonDocument = JsonDocument.Parse(claimsSet);
            return jsonDocument.RootElement.Clone();
        }
        catch (Exception e)
        {
            throw new FormatException(e.Message, e);
        }
    }

    private static string GetBase64UrlClaimsSet(string jwt)
    {
        var firstDotIndex = jwt.IndexOf('.');
        var lastDotIndex = jwt.LastIndexOf('.');

        if (firstDotIndex == -1 || lastDotIndex <= firstDotIndex)
        {
            throw new FormatException("The JWT should contain two periods.");
        }

        return jwt.Substring(firstDotIndex + 1, lastDotIndex - firstDotIndex - 1);
    }

    private static string DecodeBase64Url(string base64Url)
    {
        var base64 = base64Url
            .Replace('-', '+')
            .Replace('_', '/')
            .PadRight(base64Url.Length + (4 - base64Url.Length % 4) % 4, '=');

        return Encoding.UTF8.GetString(Convert.FromBase64String(base64));
    }
}
