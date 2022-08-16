using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Socially.WebAPI.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Socially.WebAPI.Utils
{
    public class TokenInfo
    {
        public JwtBearerOptions Options { get; set; }

        public TokenInfo()
        {
            Options = new JwtBearerOptions();
        }

        public string GenerateToken(TokenRequest request)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                IssuedAt = DateTime.UtcNow,
                Audience = request.Audience,
                Issuer = Options.TokenValidationParameters.ValidIssuer,
                Subject = new ClaimsIdentity(request.Claims),
                Expires = DateTime.UtcNow.Add(request.ExpireIn),
                SigningCredentials = new SigningCredentials(Options.TokenValidationParameters.IssuerSigningKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var tk = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(tk);
        }

        public ClaimsPrincipal GetPrinciple(string tokenStr)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            ClaimsPrincipal principle = null;
            lock (Options.TokenValidationParameters)
            {
                bool validateExpiary = Options.TokenValidationParameters.ValidateLifetime = false;
                principle = tokenHandler.ValidateToken(tokenStr, Options.TokenValidationParameters, out var validatedToken);
                Options.TokenValidationParameters.ValidateLifetime = validateExpiary;
            }
            return principle;
        }
    }
}
