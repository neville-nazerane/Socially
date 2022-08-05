using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Socially.WebAPI.Utils
{
    public static class JwtCompleteExtensions
    {

        public static AuthenticationBuilder AddJwtBearerCompletely(this AuthenticationBuilder auth,
                                                                   Action<JwtBearerOptions> configureOptions)
        {
            var info = new TokenInfo();
            auth.Services.AddSingleton(info);
            configureOptions(info.Options);
            return auth.AddJwtBearer("complete", configureOptions);
        }

        public static string GenerateJwtToken(this HttpContext context, Claim[] claims, TimeSpan exipary)
            => context.RequestServices.GetService<TokenInfo>().GenerateToken(claims, exipary);

    }
}
