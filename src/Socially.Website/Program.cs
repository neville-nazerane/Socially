using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Socially.Apps.Consumer.Services;
using Socially.Apps.Consumer.Utils;
using Socially.Website;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiEndpoint = builder.Configuration["apiEndpoint"];

builder.Services.AddScoped(sp => new HttpClient(new ApiHttpHandler()) { BaseAddress = new Uri(apiEndpoint) });
builder.Services.AddScoped<IApiConsumer>(p => new ApiConsumer(p.GetService<HttpClient>()));

await builder.Build().RunAsync();
