using Socially.Core.Models;
using System;
using System.Collections.Generic;
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

        public Task UpdateProfileAsync(ProfileUpdateModel model, CancellationToken cancellationToken = default)
            => _httpClient.PutAsJsonAsync("profile", model, cancellationToken);

        public Task ResetPasswordAsync(PasswordResetModel model, CancellationToken cancellationToken = default)
            => _httpClient.PutAsJsonAsync("profile/resetPassword", model, cancellationToken);

    }
}
