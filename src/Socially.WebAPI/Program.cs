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
using Socially.Server.DataAccess;
using Socially.Server.Managers;
using Socially.Server.Managers.Utils;
using Socially.WebAPI.Endpoints;
using Socially.WebAPI.Middlewares;
using Socially.WebAPI.Services;
using Socially.WebAPI.Utils;
using SendGrid.Extensions.DependencyInjection;
using Socially.Website.Models;
using Socially.Website.Services;
using Socially.Server.Entities;

var builder = WebApplication.CreateBuilder(args);

// SERVICES
var services = builder.Services;

var configuration = builder.Configuration;

services.AddCors();
services.AddApplicationInsightsTelemetry(o => o.ConnectionString = configuration["appinsights"]);

services.AddSingleton<IBlobAccess>(p => new BlobAccess(configuration["blobConnString"]));
services.AddDbContext<ApplicationDbContext>(c => c.UseSqlServer(configuration.GetConnectionString("db")));
services.AddSingleton(p =>
{
    var template = new ConfigsSettings();
    configuration.GetSection("settings").Bind(template);
    return template;
});
services.AddIdentity<User, UserRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();
services.AddHealthChecks()
        .AddDbContextCheck<ApplicationDbContext>();

services.AddSendGrid(o => o.ApiKey = configuration["sendGridApiKey"]);

services.AddSocalAuthentication(configuration);

services.AddManagers();
services.AddServices();

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
    endpoints.MapGet("/", () => "Hello from a socially app").ExcludeFromDescription();
    endpoints.MapHealthChecks("/health");

    endpoints.MapCustom<AccountEndpoints>();
    endpoints.MapCustom<ProfileEndpoints>();
    endpoints.MapCustom<UserEndpoints>();
    endpoints.MapCustom<ImagesEndpoints>();
    endpoints.MapCustom<FriendEndpoints>();
    endpoints.MapCustom<PostEndpoints>();

});

await using (var scope = app.Services.CreateAsyncScope())
    await scope.ServiceProvider.GetService<InitializeService>().InitAsync();

await app.RunAsync();

public partial class Program { }

