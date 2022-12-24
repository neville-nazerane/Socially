using Socially.Mobile.Logic.Services;

namespace Socially.MobileApp.Services
{
    internal class NavigationControl : INavigationControl
    {

        public Task GoToHomeAsync() => GoToAsync("home");

        public Task GoToLoginPageAsync() => GoToAsync("login");

        Task GoToAsync(string path, bool animate = false)
            => Shell.Current.GoToAsync(path, animate);

    }
}
