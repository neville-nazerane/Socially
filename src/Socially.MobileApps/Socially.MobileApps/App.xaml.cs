using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Socially.MobileApps.Config;
using Socially.MobileApps.Contracts;
using Socially.MobileApps.Pages;
using Socially.MobileApps.Services;
using Socially.MobileApps.ViewModels;
using System;
using Xamarin.Essentials;
using Xamarin.FluentInjector;
using Xamarin.Forms;

namespace Socially.MobileApps
{
    public partial class App : Application
    {
        public App()
        {
            ExperimentalFeatures.Enable("AppTheme_Experimental", "Shapes_Experimental");
            InitializeComponent();

            Microsoft.AppCenter.AppCenter
                            .Start(Configs.AppCenter, 
                                    typeof(Analytics), 
                                    typeof(Crashes));

            new ThemeControl(null).Update();
            //FontRegistry.RegisterFonts(FontAwesomeSolid.Font);

            this.StartInjecting()


                .SetViewModelAssembly(typeof(ViewModelBase).Assembly)

                .AddHttpClient<IApiConsumer, ApiConsumer>(c 
                                    => c.BaseAddress = new Uri(Configs.Endpoint))

                .AddTransient<IThemeControl, ThemeControl>()

                .Build();

            //MainPage = new MainPage();
        }


    }
}
