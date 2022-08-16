using Microsoft.AspNetCore.Components.Authorization;
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

//var apiEndpoint = "https://api.sociallyconnections.com";
var apiEndpoint = "http://localhost:37872";

//var apiEndpoint = builder.Configuration["apiEndpoint"];

builder.Services.AddScoped(sp => new HttpClient(sp.GetService<WebHttpHandler>()) { BaseAddress = new Uri(apiEndpoint) });
builder.Services.AddScoped<IApiConsumer>(p => new ApiConsumer(p.GetService<HttpClient>()));
builder.Services.AddTransient<WebHttpHandler>();

builder.Services.AddAuthorizationCore().AddOptions();
builder.Services.AddSingleton<AuthenticationStateProvider, AuthProvider>()
                .AddSingleton(p => (AuthProvider) p.GetService<AuthenticationStateProvider>());

await builder.Build().RunAsync();
