using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Socially.WebAPI.Utils
{
    public class TokenInfo
    {
        public JwtBearerOptions Options { get; set; }

        public string GenerateToken(Claim[] claims, TimeSpan exipary)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                IssuedAt = DateTime.UtcNow,
                Audience = Options.Audience,
                Issuer = Options.ClaimsIssuer,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(exipary),
                SigningCredentials = new SigningCredentials(Options.TokenValidationParameters.IssuerSigningKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var tk = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(tk);
        }
    }
}
