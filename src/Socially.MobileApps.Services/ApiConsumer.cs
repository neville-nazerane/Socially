using Socially.Core.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Socially.MobileApps.Services
{
    public class ApiConsumer
    {
        private readonly HttpClient _client;

        private const string accountPath = "api/account";

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

        public Task SignUpAsync(SignUpModel model) => _client.PostAsJsonAsync($"{accountPath}/signup", model);

        public Task LoginAsync(LoginModel model) => _client.PostAsJsonAsync($"{accountPath}/login", model);

    }
}
