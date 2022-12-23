using Socially.Mobile.Logic.Services;

namespace Socially.MobileApp.Services
{
    internal class NavigationControl : INavigationControl
    {

        public Task GoToHomeAsync() => Shell.Current.GoToAsync("home");

        public Task GoToLoginPageAsync() => Shell.Current.GoToAsync("login");

    }
}
