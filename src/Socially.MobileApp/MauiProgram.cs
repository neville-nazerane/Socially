using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Socially.Apps.Consumer.Services;
using Socially.Apps.Consumer.Utils;
using Socially.Mobile.Logic.Services;
using Socially.Mobile.Logic.ViewModels;
using Socially.MobileApp.Pages;
using Socially.MobileApp.Services;
using Socially.MobileApp.Utils;

namespace Socially.MobileApp
{
    public static partial class MauiProgram
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

            services.AddSingleton<IMessaging, Messaging>()
                    .AddSingleton<INavigationControl, NavigationControl>()
                    .AddSingleton<ISocialLogger, SociallyLogger>();

            AppPageInjections(services);

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

        static partial void AppPageInjections(IServiceCollection services);

    }
}