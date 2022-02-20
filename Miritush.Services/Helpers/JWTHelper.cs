using Microsoft.IdentityModel.Tokens;
using Miritush.DTO;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Miritush.Services.Helpers
{
    public class JWTHelper
    {
        public static AuthResult CreateToken(
            string secret,
            string issuer,
            string userName,
            string role)
        {
            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, role)
                };

            var tokenHandler = new JwtSecurityTokenHandler();
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var tokenDesc = new SecurityTokenDescriptor()
            {
                Audience = "http://miritush.com/NailBook",
                Issuer = issuer,
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddHours(3),
                Subject = new ClaimsIdentity(authClaims),
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenHandler.CreateToken(tokenDesc);
             

            return new AuthResult { Access_token = tokenHandler.WriteToken(token), ExpireIn = token.ValidTo };
        }
    }
}