using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Socially.Server.DataAccess;
using Socially.WebAPI.Endpoints;
using Socially.WebAPI.Middlewares;
using Socially.WebAPI.Utils;
using SendGrid.Extensions.DependencyInjection;
using Socially.Website.Models;
using Socially.Website.Services;
using Socially.Server.Entities;
using Microsoft.Extensions.Azure;
using Socially.WebAPI.Services;
using System.Threading.Tasks;
using Socially.WebAPI.Hubs;

var builder = WebApplication.CreateBuilder(args);

// SERVICES
var services = builder.Services;

var configuration = builder.Configuration;

services.AddCors();
services.AddApplicationInsightsTelemetry(o => o.ConnectionString = configuration["appinsights"]);


services.AddAzBlob(configuration["blobConnString"])
        .AddAzStorage(configuration["storageConnString"])
        .AddSqlServerDbContext(configuration.GetConnectionString("db"))
        .AddSettings(configuration.GetSection("settings"))
        .AddAzSignalR(configuration["signalR"]);


services.AddIdentity<User, UserRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();
services.AddHealthChecks()
        .AddAzureBlobStorage(configuration["blobConnString"])
        .AddSendGrid(configuration["sendGridApiKey"])
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

app.UseAuthentication();
app.UseAuthorization();

app.UseCurrentSetup();

app.MapGet("/", () => "Hello from a socially app").ExcludeFromDescription();
app.MapHealthChecks("/health");

app.MapCustom<AccountEndpoints>();
app.MapCustom<ProfileEndpoints>();
app.MapCustom<UserEndpoints>();
app.MapCustom<ImagesEndpoints>();
app.MapCustom<FriendEndpoints>();
app.MapCustom<PostEndpoints>();

app.MapHub<DataUpdatesHub>("/hubs/dataUpdates")
   .RequireAuthorization();

await using (var scope = app.Services.CreateAsyncScope())
    await scope.ServiceProvider.GetService<InitializeService>().InitAsync();


var signalRPublisher = app.Services.GetService<SignalRPublisher>();

await Task.WhenAll(
    signalRPublisher.KeepRunningAsync(),
    app.RunAsync()
);

public partial class Program { }

