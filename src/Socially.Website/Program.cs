using BlazorApplicationInsights;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Socially.Apps.Consumer.Services;
using Socially.Apps.Consumer.Utils;
using Socially.Website;
using Socially.Website.Models;
using Socially.Website.Services;
using System;
using System.Net.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiEndpoint = builder.Configuration["apiEndpoint"];

builder.Services.AddBlazorApplicationInsights();
builder.Services.AddTransient<WebHttpHandler>();
builder.Services.AddScoped(sp => new HttpClient(sp.GetService<WebHttpHandler>()) { BaseAddress = new Uri(apiEndpoint) });
builder.Services.AddScoped<IApiConsumer, ApiConsumer>();

builder.Services.AddAuthorizationCore().AddOptions();
builder.Services
                .AddSingleton(typeof(ICachedStorage<,>), typeof(CachedStorage<,>))

                .AddSingleton<AuthenticationStateProvider, AuthProvider>()
                .AddSingleton(p => (AuthProvider) p.GetService<AuthenticationStateProvider>())
                .AddSingleton(p => (IAuthAccess) p.GetService<AuthenticationStateProvider>())
                .AddScoped<CachedContext>();

await builder.Build().RunAsync();
