using Socially.MobileApps.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace Socially.MobileApps.Services
{
    public interface IApiConsumer
    {
        Task<ApiResponse<bool>> SignUpAsync(SignUpModel model);
        Task<ApiResponse<bool>> VerifyAccountEmailAsync(string email);
        Task<ApiResponse<bool>> VerifyAccountUsernameAsync(string userName);
    }
}