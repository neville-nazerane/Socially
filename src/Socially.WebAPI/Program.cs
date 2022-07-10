using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NetCore.Jwt;
using Socially.Core.Entities;
using Socially.Server.DataAccess;
using Socially.Server.Managers;
using Socially.Server.Services;
using Socially.WebAPI.Endpoints;
using Socially.WebAPI.EndpointUtils;
using Socially.WebAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// SERVICES
var services = builder.Services;

var config = builder.Configuration;

services.AddCors();

services.AddDbContext<ApplicationDbContext>(c => c.UseSqlServer(config.GetConnectionString("db")));
services.AddIdentity<User, UserRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>();
services.AddHealthChecks()
        .AddDbContextCheck<ApplicationDbContext>();
//services.AddControllers();

//services.AddSwaggerDocument();
services.AddAuthorization();
services.AddAuthentication(NetCoreJwtDefaults.SchemeName).AddNetCoreJwt();

// managers
services.AddTransient<IUserVerificationManager, UserVerificationManager>();
services.AddTransient<IUserService, UserService>();

// swagger
services.AddEndpointsApiExplorer();
//services.AddOpenApiDocument();

// MIDDLEWARES
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi3();
    //app.UseSwaggerUi3(c =>
    //{
    //    c.ServerUrl = ""
    //});

    //c.SwaggerEndpoint("/swagger/v1/swagger.json",
    //                               $"{builder.Environment.ApplicationName} v1")
}

app.UseExceptionHandler(new CustomExceptionHandler());

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/", c => c.Response.WriteAsync("Hello to the social world"));
    endpoints.MapHealthChecks("/health");

    endpoints.MapCustom<AccountEndpoints>("/account");

});




await app.RunAsync();



//namespace Socially.WebAPI
//{
//    public class Program
//    {
//        public static void Main(string[] args)
//        {
//            CreateHostBuilder(args).Build().Run();
//        }

//        public static IHostBuilder CreateHostBuilder(string[] args) =>
//            Host.CreateDefaultBuilder(args)
//                .ConfigureWebHostDefaults(webBuilder =>
//                {
//                    webBuilder.UseStartup<Startup>();
//                });
//    }
//}


public partial class Program { }