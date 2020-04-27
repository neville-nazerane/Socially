using Socially.Core.Models;
using System.Threading.Tasks;

namespace Socially.MobileApps.Services
{
    public interface IApiConsumer
    {
        Task LoginAsync(LoginModel model);
        Task SignUpAsync(SignUpModel model);
        Task<bool> VerifyAccountEmailAsync(string email);
        Task<bool> VerifyAccountUsernameAsync(string userName);
    }
}