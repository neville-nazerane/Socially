using Socially.Mobile.Logic.Services;

namespace Socially.MobileApp.Services
{
    internal class NavigationControl : INavigationControl
    {

        public Task GoToSignupAsync() => GoToAsync("signup");

        public Task GoToForgotPasswordAsync() => GoToAsync("forgotPassword");

        public Task GoToHomeAsync() => GoToAsync("home");

        public Task GoToLoginAsync() => GoToAsync("login");

        public Task GoToAccountAsync() => GoToAsync("account");

        public Task GoToProfilePostsAsync() => GoToAsync("profile/posts");

        public Task GoToProfileFriendsAsync() => GoToAsync("profile/friends");
        
        public Task GoToProfileRequestsAsync() => GoToAsync("profile/requests");

        static Task GoToAsync(string path, bool animate = false)
            => Shell.Current.GoToAsync($"//MainPage/{path}", animate);

    }
}
