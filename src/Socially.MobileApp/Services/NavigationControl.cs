using Socially.Mobile.Logic.Services;

namespace Socially.MobileApp.Services
{
    internal class NavigationControl : INavigationControl
    {

        public Task GoToSignupAsync() => GoToAsync("signup");

        public Task GoToForgotPasswordAsync() => GoToAsync("forgotPassword");

        public Task GoToHomeAsync() => GoToAsync("home");

        public Task GoToLoginAsync() => GoToAsync("login");

        Task GoToAsync(string path, bool animate = false)
            => Shell.Current.GoToAsync(path, animate);

    }
}
