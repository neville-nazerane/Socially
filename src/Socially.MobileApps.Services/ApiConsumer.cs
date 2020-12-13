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

        private const string accountPath = "account";

        public ApiConsumer(HttpClient client)
        {
            _client = client;
        }

        public async Task<bool> VerifyAccountEmailAsync(string email)
        {
            string result = await _client.GetStringAsync($"{accountPath}/verifyEmail/{email}");
            return bool.Parse(result);
        }

        public async Task<bool> VerifyAccountUsernameAsync(string userName)
        {
            string result = await _client.GetStringAsync($"{accountPath}/verifyUsername/{userName}");
            return bool.Parse(result);
        }

        public async Task<ApiResponse<bool>> SignUpAsync(SignUpModel model)
        {
            var res = await _client.PostAsJsonAsync($"{accountPath}/signup", model);
            return await res.CreateResponseAsync<bool>();
        }

    }
}
