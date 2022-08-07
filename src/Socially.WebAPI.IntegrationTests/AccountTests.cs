using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Socially.Core.Models;
using System.Net.Http.Json;
using System.Net.Http.Headers;

namespace Socially.WebAPI.IntegrationTests
{
    public class AccountTests : IClassFixture<CustomWebApplicationFactory>
    {

        const string path = "";
        private readonly CustomWebApplicationFactory _factory;

        public AccountTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task AccountSetup()
        {
            var client = _factory.CreateClient();

            const string testEmail = "ya@goo.com";
            const string testUsername = "username";
            const string testPassword = "pasSword!2";
            const string loginSource = "tester";

            // attempt signin
            var loginModel = new LoginModel
            {
                UserName = testUsername,
                Password = testPassword,
                Source = loginSource
            };

            var loginResult = await client.PostAsJsonAsync($"{path}/login", loginModel);

            Assert.Equal(400, (int)loginResult.StatusCode);

            // singing up
            var signupModel = new SignUpModel
            {
                Email = testEmail,
                UserName = testUsername,
                Password = testPassword,
                ConfirmPassword = testPassword
            };

            var signinRes = await client.PostAsJsonAsync($"{path}/signup", signupModel);
            Assert.True(signinRes.IsSuccessStatusCode,
                                $"Sign up had error code {signinRes.StatusCode} saying '{await signinRes.Content.ReadAsStringAsync()}'");

            loginResult = await client.PostAsJsonAsync($"{path}/login", loginModel);

            Assert.True(loginResult.IsSuccessStatusCode,
                               $"Sign in had error code {loginResult.StatusCode} saying '{await loginResult.Content.ReadAsStringAsync()}'");


            var failedLoginResult = await client.PostAsJsonAsync($"{path}/login", new LoginModel
            {
                UserName = loginModel.UserName,
                Password = "INVALID",
                Source = loginSource
            });

            Assert.Equal(System.Net.HttpStatusCode.BadRequest, failedLoginResult.StatusCode);

            string token = await loginResult.Content.ReadAsStringAsync();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

            var profile = await client.GetFromJsonAsync<ProfileUpdateModel>("profile");
            
            Assert.NotNull(profile);
            Assert.Null(profile.FirstName);

            profile.FirstName = "UpdatedName";
            profile.LastName = "UpdatedLName";

            await client.PutAsJsonAsync("profile", profile);

            Assert.NotNull(profile.FirstName);
            Assert.NotNull(profile.LastName);
            Assert.Equal("UpdatedName", profile.FirstName);
            Assert.Equal("UpdatedLName", profile.LastName);

        }



    }

}
