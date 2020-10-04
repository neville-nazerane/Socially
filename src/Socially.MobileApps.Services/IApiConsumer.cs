using Socially.MobileApps.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace Socially.MobileApps.Services
{
    public interface IApiConsumer
    {
        Task<ApiResponse<bool>> SignUpAsync(SignUpModel model);

        //Task LoginAsync(LoginModel model);
        Task<bool> VerifyAccountEmailAsync(string email);
        Task<bool> VerifyAccountUsernameAsync(string userName);
    }
}