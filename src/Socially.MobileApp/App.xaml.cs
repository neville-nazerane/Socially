using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Socially.Mobile.Logic.Services;
using Socially.MobileApp.Utils;

namespace Socially.MobileApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }

        protected override async void OnStart()
        {
            await ServicesUtil.Get<ICachedContext>().ClearDbAsync();
            base.OnStart();
        }

    }
}