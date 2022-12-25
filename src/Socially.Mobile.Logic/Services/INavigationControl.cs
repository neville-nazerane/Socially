using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Mobile.Logic.Services
{
    public interface INavigationControl
    {
        Task GoToForgotPasswordAsync();
        Task GoToHomeAsync();
        Task GoToLoginAsync();
        Task GoToSignupAsync();
    }
}
