using Socially.MobileApps.Config;
using Socially.MobileApps.Pages;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Socially.MobileApps
{
    public partial class App : Application
    {
        public App()
        {

            Device.SetFlags(new string[] { "AppTheme_Experimental" });
            InitializeComponent();
            new ThemeControl().Update();
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
