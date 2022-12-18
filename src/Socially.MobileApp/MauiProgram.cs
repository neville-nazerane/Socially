using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Socially.Apps.Consumer.Services;
using Socially.Apps.Consumer.Utils;
using Socially.MobileApp.Services;
using Socially.MobileApp.Utils;

namespace Socially.MobileApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {

            // verify configs 
            if (Configs.BaseURL is null)
            {
                throw new Exception("Configuration not set");
            }

            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            var services = builder.Services;

            services.AddSingleton<IAuthAccess, AuthAccess>()
                    .AddSingleton<ApiHttpHandler>()
                    .AddSingleton<IApiConsumer>(p => 
                                        new ApiConsumer(
                                                new HttpClient(p.GetService<ApiHttpHandler>())
                                                {
                                                    BaseAddress = new Uri(Configs.BaseURL)
                                                }));




#if DEBUG
		    builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}