using Socially.Core.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Socially.Apps.Consumer.Services
{
    public interface IApiConsumer
    {
        Task<ProfileUpdateModel> GetUpdateProfileAsync(CancellationToken cancellationToken = default);
        Task<TokenResponseModel> LoginAsync(LoginModel model, CancellationToken cancellationToken = default);
        Task ResetPasswordAsync(PasswordResetModel model, CancellationToken cancellationToken = default);
        Task SignupAsync(SignUpModel model, CancellationToken cancellationToken = default);
        Task UpdateProfileAsync(ProfileUpdateModel model, CancellationToken cancellationToken = default);
    }
}