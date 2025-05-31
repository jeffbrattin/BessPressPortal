namespace BessPressPortal.Client.Services
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Text;
    using System.Text.Json;

    namespace BessPressPortal.Client.Services
    {
        public static class JwtParser
        {
            public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
            {
                var claims = new List<Claim>();
                try
                {
                    var payload = jwt.Split('.')[1]; // Get the payload (second part of JWT)
                    var jsonBytes = ConvertFromBase64Url(payload);
                    var json = Encoding.UTF8.GetString(jsonBytes);
                    var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(json);

                    foreach (var pair in keyValuePairs)
                    {
                        claims.Add(new Claim(pair.Key, pair.Value.ToString()));
                    }
                }
                catch
                {
                    // Handle invalid JWT gracefully
                    return Array.Empty<Claim>();
                }

                return claims;
            }

            private static byte[] ConvertFromBase64Url(string base64)
            {
                base64 = base64.Replace('-', '+').Replace('_', '/');
                switch (base64.Length % 4)
                {
                    case 2: base64 += "=="; break;
                    case 3: base64 += "="; break;
                }
                return Convert.FromBase64String(base64);
            }
        }
    }


}
