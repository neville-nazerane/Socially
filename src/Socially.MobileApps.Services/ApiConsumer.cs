using Socially.MobileApps.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;


namespace Socially.MobileApps.Services
{
    public class ApiConsumer : IApiConsumer
    {
        private readonly HttpClient _client;

        private const string accountPath = "api/account";

        public ApiConsumer(HttpClient client)
        {
            _client = client;
        }

        public async Task<ApiResponse<bool>> VerifyAccountEmailAsync(string email)
        {
            var res = await _client.GetAsync($"{accountPath}/verifyEmail/{email}");
            return await res.CreateResponseAsync<bool>();
        }

        public async Task<ApiResponse<bool>> VerifyAccountUsernameAsync(string userName)
        {
            var res = await _client.GetAsync($"{accountPath}/verifyUsername/{userName}");
            return await res.CreateResponseAsync<bool>();
        }

        public async Task<ApiResponse<bool>> SignUpAsync(SignUpModel model)
        {
            var res = await _client.PostAsJsonAsync($"{accountPath}/signup", model);
            return await res.CreateResponseAsync<bool>();
        }

        //public Task LoginAsync(LoginModel model) => _client.PostAsJsonAsync($"{accountPath}/login", model);

    }
}
