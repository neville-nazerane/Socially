using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Socially.Apps.Consumer.Services;
using Socially.Apps.Consumer.Utils;
using Socially.Website;
using Socially.Website.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiEndpoint = builder.Configuration["apiEndpoint"];

builder.Services.AddScoped(sp => new HttpClient(sp.GetService<WebHttpHandler>()) { BaseAddress = new Uri(apiEndpoint) });
builder.Services.AddScoped<IApiConsumer>(p => new ApiConsumer(p.GetService<HttpClient>()));
builder.Services
       .AddTransient<WebHttpHandler>()
       .AddSingleton<AuthService>();

await builder.Build().RunAsync();
