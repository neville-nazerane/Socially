using Microsoft.AspNetCore.Identity;
using Socially.Core.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Socially.WebAPI.Services
{
    public interface IUserService
    {
        Task<ProfileUpdateModel> GetUpdatableProfileAsync(CancellationToken cancellationToken = default);
        Task<string> LoginAsync(LoginModel model);
        Task SignUpAsync(SignUpModel model, CancellationToken cancellationToken = default);
        Task UpdateProfileAsync(ProfileUpdateModel model, CancellationToken cancellation = default);
        Task<bool> VerifyEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<bool> VerifyUsernameAsync(string userName, CancellationToken cancellationToken = default);
    }
}