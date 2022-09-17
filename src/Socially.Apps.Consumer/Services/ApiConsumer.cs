using Socially.Apps.Consumer.Utils;
using Socially.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Socially.Apps.Consumer.Services
{
    public class ApiConsumer : IApiConsumer
    {
        private readonly HttpClient _httpClient;

        public ApiConsumer(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public void SetJwt(string jwtHeader)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", jwtHeader);
        }

        public async Task SignupAsync(SignUpModel model,
                                CancellationToken cancellationToken = default)
        {
            var res = await _httpClient.PostAsJsonAsync("signup",
                                                        model,
                                                        cancellationToken);
            res.EnsureSuccessStatusCode();
        }

        public async Task<TokenResponseModel> LoginAsync(LoginModel model,
                                     CancellationToken cancellationToken = default)
        {
            var res = await _httpClient.PostAsJsonAsync("login",
                                                        model,
                                                        cancellationToken);
            res.EnsureSuccessStatusCode();
            return await res.Content.ReadFromJsonAsync<TokenResponseModel>(cancellationToken: cancellationToken);
        }

        public Task<ProfileUpdateModel> GetUpdateProfileAsync(CancellationToken cancellationToken = default)
            => _httpClient.GetFromJsonAsync<ProfileUpdateModel>("profile", cancellationToken);

        public Task<HttpResponseMessage> UpdateProfileAsync(ProfileUpdateModel model, CancellationToken cancellationToken = default)
            => _httpClient.PutAsJsonAsync("profile", model, cancellationToken);

        public Task<HttpResponseMessage> ResetPasswordAsync(PasswordResetModel model, CancellationToken cancellationToken = default)
            => _httpClient.PutAsJsonAsync("profile/resetPassword", model, cancellationToken);

        public Task<HttpResponseMessage> ResetForgottenPasswordAsync(ForgotPasswordModel model, CancellationToken cancellationToken = default)
            => _httpClient.PutAsJsonAsync("resetForgottenPassword", model, cancellationToken);

        public Task<HttpResponseMessage> ForgotPasswordAsync(string email, CancellationToken cancellationToken = default)
            => _httpClient.PostAsync($"forgotPassword/{email}", null, cancellationToken);

        public Task<HttpResponseMessage> UploadAsync(ImageUploadModel model, CancellationToken cancellationToken = default)
            => _httpClient.PostAsync("image", model.MakeForm(), cancellationToken);

        public Task<IEnumerable<string>> GetAllImagesOfUserAsync(CancellationToken cancellationToken = default)
            => _httpClient.GetFromJsonAsync<IEnumerable<string>>("images", cancellationToken);

        public Task<HttpResponseMessage> DeleteImageAsync(string fileName, 
                                                          CancellationToken cancellationToken = default)
            => _httpClient.DeleteAsync($"image/{fileName}", cancellationToken);

    }
}
