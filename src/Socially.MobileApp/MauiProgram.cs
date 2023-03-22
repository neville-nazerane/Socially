using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Markup;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
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

            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("NoUnderline", (h, v) =>
            {
#if ANDROID
                h.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Colors.Transparent.ToAndroid());
#endif
            });

            Microsoft.Maui.Handlers.EditorHandler.Mapper.AppendToMapping("NoUnderline", (h, v) =>
            {
#if ANDROID
                h.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Colors.Transparent.ToAndroid());
#endif
            });

            // verify configs 
            if (Configs.BaseURL is null)
            {
                throw new Exception("Configuration not set");
            }

            
            var builder = MauiApp.CreateBuilder();

            AppSetup(builder);

            var services = builder.Services;

            // internal services
            services.AddSingleton<IMessaging, Messaging>()
                    .AddSingleton<INavigationControl, NavigationControl>()
                    .AddSingleton<ICachedContext, CachedContext>()
                    .AddSingleton(typeof(ICachedNoSqlStorage<,>), typeof(CachedNoSqlStorage<,>))
                    .AddSingleton<ISocialLogger, SociallyLogger>();

            // calling auto generated function
            AppPageInjections(services);

            // API setup
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

        private static void AppSetup(MauiAppBuilder builder)
        {
            AppCenter.Start(Configs.AppCenter, typeof(Analytics), typeof(Crashes));

            builder
                //.WithApp()
                //    .SetMainPage(new AppShell())
                //    .AddResource("Resources/Styles/Colors.xaml")
                //    .AddResource("Resources/Styles/Styles.xaml")
                //.UseMaui()
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseMauiCommunityToolkitMarkup()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("fa-solid-900.otf", "faSolid");
                    fonts.AddFont("fa-regular-400.otf", "faRegular");
                });
        }

        static partial void AppPageInjections(IServiceCollection services);

    }
}