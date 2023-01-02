using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Mobile.Logic.Services
{
    public interface INavigationControl
    {
        TaskCompletionSource<string> ImagePopupResponse { get; }

        Task GoToAccountAsync();
        Task GoToForgotPasswordAsync();
        Task GoToHomeAsync();
        Task GoToLoginAsync();
        Task GoToProfileFriendsAsync();
        Task GoToProfilePostsAsync();
        Task GoToProfileImagesAsync();
        Task GoToSignupAsync();
        Task<string> OpenImagePickerAsync();
    }
}
