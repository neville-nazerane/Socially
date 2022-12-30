using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Mobile.Logic.Services
{
    public interface INavigationControl
    {
        Task GoToAccountAsync();
        Task GoToForgotPasswordAsync();
        Task GoToHomeAsync();
        Task GoToLoginAsync();
        Task GoToProfileFriendsAsync();
        Task GoToProfilePostsAsync();
        Task GoToProfileRequestsAsync();
        Task GoToSignupAsync();
    }
}
