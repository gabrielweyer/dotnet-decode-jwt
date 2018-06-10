using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;

namespace DotNet.Decode.Jwt
{
    public static class JwtClaimsDecoder
    {
        private const string ItemElementName = "item";

        public static IDictionary<string, string> GetClaims(string jwt)
        {
            var base64UrlClaimsSet = GetBase64UrlClaimsSet(jwt);
            var base64ClaimsSet = ConvertToBase64(base64UrlClaimsSet);

            return DeserializeClaims(base64ClaimsSet);
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

        private static IDictionary<string, string> DeserializeClaims(byte[] base64ClaimsSet)
        {
            var claims = new Dictionary<string, string>();

            try
            {
                using (var reader = JsonReaderWriterFactory.CreateJsonReader(base64ClaimsSet, XmlDictionaryReaderQuotas.Max))
                {
                    string claimValue = null;
                    var claimValues = new List<string>();
                    var arrayMode = false;

                    var previousNodeType = XmlNodeType.None;
                    var claimNameViaAttribute = string.Empty;

                    while (reader.Read())
                    {
                        if (reader.AttributeCount > 0)
                        {
                            claimNameViaAttribute = reader.GetAttribute(ItemElementName);
                        }

                        if (reader.NodeType == XmlNodeType.Text)
                        {
                            if (arrayMode)
                            {
                                claimValues.Add(reader.Value);
                            }
                            else
                            {
                                claimValue = reader.Value;
                            }
                        }
                        else if (reader.NodeType == XmlNodeType.EndElement && !IsItem(reader.Name))
                        {
                            if (previousNodeType == XmlNodeType.EndElement) break;

                            if (arrayMode)
                            {
                                claims.Add(reader.Name, $"[\"{string.Join("\",\"", claimValues)}\"]");
                                claimValues = new List<string>();
                                arrayMode = false;
                            }
                            else
                            {
                                claims.Add(string.IsNullOrWhiteSpace(claimNameViaAttribute) ? reader.Name : claimNameViaAttribute, claimValue);
                                claimNameViaAttribute = string.Empty;
                            }
                        }
                        else if (reader.NodeType == XmlNodeType.Element && IsItem(reader.Name))
                        {
                            arrayMode = true;
                        }

                        if (!IsItem(reader.Name))
                        {
                            previousNodeType = reader.NodeType;
                        }
                    }
                }
            }
            catch (XmlException e)
            {
                var claimsSet = Encoding.UTF8.GetString(base64ClaimsSet);
                throw new FormatException($"The claims set is not valid JSON: {claimsSet}.", e);
            }

            return claims;
        }

        private static byte[] ConvertToBase64(string base64Url)
        {
            var base64ClaimsSet = base64Url.PadRight(base64Url.Length + (4 - base64Url.Length % 4) % 4, '=');
            return Convert.FromBase64String(base64ClaimsSet);
        }

        private static bool IsItem(string name)
        {
            return ItemElementName.Equals(name);
        }
    }
}
