using Socially.Mobile.Logic.Services;

namespace Socially.MobileApp.Services
{
    internal class NavigationControl : INavigationControl
    {

        public Task GoToSignupAsync() => NavigationControl.GoToAsync("signup");

        public Task GoToForgotPasswordAsync() => NavigationControl.GoToAsync("forgotPassword");

        public Task GoToHomeAsync() => NavigationControl.GoToAsync("home");

        public Task GoToLoginAsync() => NavigationControl.GoToAsync("login");

        public Task GoToProfilePostsAsync() => NavigationControl.GoToAsync("profile/posts");

        public Task GoToProfileFriendsAsync() => NavigationControl.GoToAsync("profile/friends");


        static Task GoToAsync(string path, bool animate = false)
            => Shell.Current.GoToAsync($"//MainPage/{path}", animate);

    }
}
