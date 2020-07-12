using AP.MobileToolkit.Fonts;
using Socially.MobileApps.Config;
using Socially.MobileApps.Contracts;
using Socially.MobileApps.Pages;
using Socially.MobileApps.Services;
using Socially.MobileApps.ViewModels;
using System;
using Xamarin.FluentInjector;
using Xamarin.Forms;

namespace Socially.MobileApps
{
    public partial class App : Application
    {
        public App()
        {

            Device.SetFlags(new string[] { "AppTheme_Experimental" });
            InitializeComponent();

            new ThemeControl(null).Update();
            FontRegistry.RegisterFonts(FontAwesomeSolid.Font);

            this.StartInjecting()


                .SetViewModelAssembly(typeof(ViewModelBase).Assembly)

                .AddHttpClient<IApiConsumer, ApiConsumer>(c => c.BaseAddress = new Uri("https://socially.nevillenazerane.com"))

                .AddTransient<IThemeControl, ThemeControl>()

                .Build();

            //MainPage = new MainPage();
        }


    }
}
