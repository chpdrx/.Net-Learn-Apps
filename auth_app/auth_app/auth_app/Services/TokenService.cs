using auth_app.Data;
using auth_app.Models;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace auth_app.Services
{
    public class TokenService : ITokenService
    {
        public Token GetToken(string token)
        {
            string secret = FileData.JsonParse().JwtKey;
            var key = Encoding.UTF8.GetBytes(secret);
            var handler = new JwtSecurityTokenHandler();
            var validations = new TokenValidationParameters
            {
                ValidateActor = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
            var claims = handler.ValidateToken(token, validations, out var tokenSecure);
            return new Token { UserId = claims.Identity.Name };
        }
    }
}
