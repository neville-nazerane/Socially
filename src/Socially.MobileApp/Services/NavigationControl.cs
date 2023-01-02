using Socially.Mobile.Logic.Services;
using Socially.MobileApp.Pages;
using Socially.MobileApp.Utils;

namespace Socially.MobileApp.Services
{
    internal class NavigationControl : INavigationControl
    {

        public TaskCompletionSource<string> ImagePopupResponse { get; private set; }

        public Task GoToSignupAsync() => GoToAsync("signup");

        public Task GoToForgotPasswordAsync() => GoToAsync("forgotPassword");

        public Task GoToHomeAsync() => GoToAsync("home");

        public Task GoToLoginAsync() => GoToAsync("login");

        public Task GoToAccountAsync() => GoToAsync("account");

        public Task GoToProfilePostsAsync() => GoToAsync("profile/posts");

        public Task GoToProfileFriendsAsync() => GoToAsync("profile/friends");
        
        public Task GoToProfileImagesAsync() => GoToAsync("profile/images");

        public async Task<string> OpenImagePickerAsync()
        {
            ImagePopupResponse = new();
            var previousRoute = Shell.Current.CurrentState.Location.OriginalString;
            await Shell.Current.Navigation.PushModalAsync(ServicesUtil.Get<ImagePickerPage>());
            //await GoToAsync("/popups/imagePicker");
            var result = await ImagePopupResponse.Task;
            if (Shell.Current.CurrentPage is ImagePickerPage)
                await Shell.Current.Navigation.PopModalAsync();
            ImagePopupResponse = null;
            return result;
        }

        static Task GoToAsync(string path, bool animate = false)
            => Shell.Current.GoToAsync($"//MainPage/{path}", animate);

    }
}
