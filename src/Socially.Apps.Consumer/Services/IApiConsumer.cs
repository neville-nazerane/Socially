using Socially.Core.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Socially.Apps.Consumer.Services
{
    public interface IApiConsumer
    {
        Task<HttpResponseMessage> DeleteImageAsync(string fileName, CancellationToken cancellationToken = default);
        Task<HttpResponseMessage> ForgotPasswordAsync(string email, CancellationToken cancellationToken = default);
        Task<IEnumerable<string>> GetAllImagesOfUserAsync(CancellationToken cancellationToken = default);
        Task<ProfileUpdateModel> GetUpdateProfileAsync(CancellationToken cancellationToken = default);
        Task<TokenResponseModel> LoginAsync(LoginModel model, CancellationToken cancellationToken = default);
        Task<HttpResponseMessage> ResetForgottenPasswordAsync(ForgotPasswordModel model, CancellationToken cancellationToken = default);
        Task<HttpResponseMessage> ResetPasswordAsync(PasswordResetModel model, CancellationToken cancellationToken = default);
        void SetJwt(string jwtHeader);
        Task SignupAsync(SignUpModel model, CancellationToken cancellationToken = default);
        Task<HttpResponseMessage> UpdateProfileAsync(ProfileUpdateModel model, CancellationToken cancellationToken = default);
        Task<HttpResponseMessage> UploadAsync(ImageUploadModel model, CancellationToken cancellationToken = default);
    }
}