using CommunityToolkit.Maui.Behaviors;
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
            Routing.RegisterRoute("profile/posts", typeof(ProfilePostsPage));
            Routing.RegisterRoute("profile/friends", typeof(ProfileFriendsPage));
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);
        }

        protected override void OnNavigated(ShellNavigatedEventArgs args)
        {

            Current.CurrentPage.Behaviors.Add(new StatusBarBehavior
            {
                StatusBarColor = Color.FromArgb("#010C80")
            }); ;

            base.OnNavigated(args);
        }

    }
}