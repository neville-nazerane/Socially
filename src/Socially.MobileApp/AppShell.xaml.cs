using CommunityToolkit.Maui.Behaviors;
using Socially.Mobile.Logic.Services;
using Socially.MobileApp.Components;
using Socially.MobileApp.Pages;
using Socially.MobileApp.Utils;

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
            Routing.RegisterRoute("account", typeof(AccountPage));
            Routing.RegisterRoute("profile/posts", typeof(ProfilePostsPage));
            Routing.RegisterRoute("profile/friends", typeof(ProfileFriendsPage));
            Routing.RegisterRoute("profile/requests", typeof(ProfileRequestsPage));

            Routing.RegisterRoute("popups/imagePicker", typeof(ImagePickerPage));
            InitializeComponent();
        }

        protected override async void OnNavigating(ShellNavigatingEventArgs args)
        {
            await ServicesUtil.Get<ICachedContext>().ClearRAMAsync();
            base.OnNavigating(args);
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