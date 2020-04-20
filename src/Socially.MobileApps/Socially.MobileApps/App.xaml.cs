using Socially.MobileApps.Pages;
using Socially.MobileApps.ViewModels;
using System;
using Xamarin.FluentInjector;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Socially.MobileApps
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            this.StartInjecting()
                .SetDefaultPage(new MainPage())
                .SetViewModelAssembly(typeof(ViewModelBase).Assembly)
                .Build();

            //MainPage = new MainPage();
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
