using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Socially.Core.Models;
using System.Net.Http.Json;

namespace Socially.WebAPI.IntegrationTests
{
    public class AccountTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public AccountTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task SignupAndLogin()
        {
            var client = _factory.CreateClient();

            const string testEmail = "ya@goo.com";
            const string testUsername = "username";
            const string testPassword = "pasSword!2";

            bool emailExists = bool.Parse(await client.GetStringAsync($"verifyEmail/{testEmail}"));
            bool userExists = bool.Parse(await client.GetStringAsync($"verifyUsername/{testUsername}"));

            Assert.False(emailExists);
            Assert.False(userExists);

            // attempt signin
            var loginModel = new LoginModel { 
                UserName = testUsername,
                Password = testPassword
            };

            var loginResult = await client.PostAsJsonAsync("login", loginModel);

            Assert.Equal(400, (int)loginResult.StatusCode);

            // singing up
            var signupModel = new SignUpModel
            {
                Email = testEmail,
                UserName = testUsername,
                Password = testPassword,
                ConfirmPassword = testPassword
            };

            var signinRes = await client.PostAsJsonAsync("signup", signupModel);
            Assert.True(signinRes.IsSuccessStatusCode,
                                $"Sign up had error code {signinRes.StatusCode} saying '{await signinRes.Content.ReadAsStringAsync()}'");


            emailExists = bool.Parse(await client.GetStringAsync($"verifyEmail/{testEmail}"));
            userExists = bool.Parse(await client.GetStringAsync($"verifyUsername/{testUsername}"));

            Assert.True(emailExists);
            Assert.True(userExists);

            loginResult = await client.PostAsJsonAsync("login", loginModel);

            Assert.True(loginResult.IsSuccessStatusCode,
                               $"Sign in had error code {loginResult.StatusCode} saying '{await loginResult.Content.ReadAsStringAsync()}'");


        }

    }

}
