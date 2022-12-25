using Socially.MobileApp.Pages;

namespace Socially.MobileApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            Routing.RegisterRoute("login", typeof(LoginPage));
            Routing.RegisterRoute("signup", typeof(SignupPage));
            Routing.RegisterRoute("forgotPassword", typeof(ForgotPasswordPage));
            Routing.RegisterRoute("home", typeof(HomePage));
            InitializeComponent();
        }

    }
}