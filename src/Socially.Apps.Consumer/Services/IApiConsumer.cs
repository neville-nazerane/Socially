using Socially.Core.Models;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Socially.Apps.Consumer.Services
{
    public interface IApiConsumer
    {
        Task ForgotPasswordAsync(string email, CancellationToken cancellationToken = default);
        Task<ProfileUpdateModel> GetUpdateProfileAsync(CancellationToken cancellationToken = default);
        Task<TokenResponseModel> LoginAsync(LoginModel model, CancellationToken cancellationToken = default);
        Task<HttpResponseMessage> ResetForgottenPasswordAsync(ForgotPasswordModel model, CancellationToken cancellationToken = default);
        Task<HttpResponseMessage> ResetPasswordAsync(PasswordResetModel model, CancellationToken cancellationToken = default);
        Task SignupAsync(SignUpModel model, CancellationToken cancellationToken = default);
        Task UpdateProfileAsync(ProfileUpdateModel model, CancellationToken cancellationToken = default);
    }
}