using Microsoft.AspNetCore.Identity;
using Socially.Core.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Socially.Server.Services
{
    public interface IUserService
    {
        Task<SignInResult> LoginAsync(LoginModel model);
        Task<IdentityResult> SignUpAsync(SignUpModel model);
        Task<bool> VerifyEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<bool> VerifyUsernameAsync(string userName, CancellationToken cancellationToken = default);
    }
}