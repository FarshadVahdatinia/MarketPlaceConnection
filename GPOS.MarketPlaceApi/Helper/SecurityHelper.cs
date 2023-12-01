using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace GPOS.MarketPlaceApi.Helper
{
    public static class SecurityHelper
    {
        public static string Sec = "****";
        public static string SecForEncryption = "**********";
        public static string AesKey = "*****************";

        public static TokenValidationParameters GetTokenValidationParameters(string audience, string issuer)
        {
            return new TokenValidationParameters()
            {
                ValidateIssuer = true,
                RequireExpirationTime = false,
                ValidateAudience = true,
                ValidAudience = audience,
                ValidIssuer = issuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Sec)),
                ClockSkew = TimeSpan.Zero,

            };
        }
    }
}
