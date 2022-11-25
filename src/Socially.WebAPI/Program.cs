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

await using (var scope = app.Services.CreateAsyncScope())
    await scope.ServiceProvider.GetService<InitializeService>().InitAsync();

await app.RunAsync();

public partial class Program { }

