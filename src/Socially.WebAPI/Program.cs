using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NetCore.Jwt;
using Socially.Core.Entities;
using Socially.Server.DataAccess;
using Socially.Server.Managers;
using Socially.Server.Services.Models;
using Socially.WebAPI.Endpoints;
using Socially.WebAPI.Middlewares;
using Socially.WebAPI.Services;
using Socially.WebAPI.Utils;

var builder = WebApplication.CreateBuilder(args);

// SERVICES
var services = builder.Services;

var configuration = builder.Configuration;

services.AddCors();
services.AddApplicationInsightsTelemetry(o => o.ConnectionString = configuration["appinsights"]);
services.AddDbContext<ApplicationDbContext>(c => c.UseSqlServer(configuration.GetConnectionString("db")));
services.AddIdentity<User, UserRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();
services.AddHealthChecks()
        .AddDbContextCheck<ApplicationDbContext>();
//services.AddControllers();

//services.AddSwaggerDocument();
services.AddAuthentication("complete")
        .AddJwtBearerCompletely(o =>
        {
            var configs = configuration.GetRequiredSection("authOptions");
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = configs["issuer"],
                ValidAudiences = configs["audiences"].Split(","),
                RequireExpirationTime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configs["secret"]))
            };
        });
services.AddAuthorization(o =>
    o.DefaultPolicy = new AuthorizationPolicyBuilder()
                            .AddAuthenticationSchemes("complete")
                            .RequireAuthenticatedUser()
                            .Build()
);

// managers
services.AddTransient<IUserProfileManager, UserProfileManager>();

// services
services.AddTransient<IUserService, UserService>()
        .AddScoped<CurrentContext>();

// swagger
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

// MIDDLEWARES
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(new CustomExceptionHandler());

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseCurrentSetup();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/", () => "Hello from a socially app");
    endpoints.MapHealthChecks("/health");

    endpoints.MapCustom<AccountEndpoints>();
    endpoints.MapCustom<ProfileEndpoints>();

});


await app.RunAsync();

public partial class Program { }

