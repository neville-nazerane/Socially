﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Socially.WebAPI.Models;
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
            auth.Services.AddSingleton(p =>
            {
                var info = new TokenInfo();
                configureOptions(info.Options);
                return info;
            });
            return auth.AddJwtBearer("complete", o => configureOptions(o));
        }

        public static string GenerateJwtToken(this HttpContext context,
                                              TokenRequest request)
            => context.RequestServices.GetService<TokenInfo>().GenerateToken(request);

    }
}
