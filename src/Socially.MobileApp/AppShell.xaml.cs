using Socially.MobileApp.Pages;

namespace Socially.MobileApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            Routing.RegisterRoute("login", typeof(LoginPage));
            Routing.RegisterRoute("home", typeof(HomePage));
            InitializeComponent();
        }

    }
}