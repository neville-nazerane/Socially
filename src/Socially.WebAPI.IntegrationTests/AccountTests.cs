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
            const string testv2Password = "pasSword!3";
            const string loginSource = "tester";

            // attempt signin
            var loginModel = new LoginModel
            {
                UserName = testUsername,
                Password = testPassword,
                Source = loginSource
            };

            var loginResult = await client.PostAsJsonAsync($"login", loginModel);

            Assert.Equal(400, (int)loginResult.StatusCode);

            // singing up
            var signupModel = new SignUpModel
            {
                Email = testEmail,
                UserName = testUsername,
                Password = testPassword,
                ConfirmPassword = testPassword
            };

            var signinRes = await client.PostAsJsonAsync($"signup", signupModel);
            Assert.True(signinRes.IsSuccessStatusCode,
                                $"Sign up had error code {signinRes.StatusCode} saying '{await signinRes.Content.ReadAsStringAsync()}'");

            loginResult = await client.PostAsJsonAsync($"login", loginModel);

            Assert.True(loginResult.IsSuccessStatusCode,
                               $"Sign in had error code {loginResult.StatusCode} saying '{await loginResult.Content.ReadAsStringAsync()}'");


            // test with failed password
            var failedLoginResult = await client.PostAsJsonAsync($"login", new LoginModel
            {
                UserName = loginModel.UserName,
                Password = "INVALID",
                Source = loginSource
            });

            Assert.Equal(System.Net.HttpStatusCode.BadRequest, failedLoginResult.StatusCode);

            TokenResponseModel tokenResponse = await loginResult.Content.ReadFromJsonAsync<TokenResponseModel>();
            Assert.NotNull(tokenResponse.AccessToken);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", tokenResponse.AccessToken);

            // reset password
            var resetPasswordResult = await client.PutAsJsonAsync($"profile/resetPassword", new PasswordResetModel
            {
                CurrentPassword = testPassword,
                NewPassword = testv2Password
            });
            Assert.Equal(System.Net.HttpStatusCode.OK, resetPasswordResult.StatusCode);
            var newLoginResult = await client.PostAsJsonAsync($"login", new LoginModel
            {
                UserName = loginModel.UserName,
                Password = testv2Password,
                Source = loginSource
            });
            Assert.Equal(System.Net.HttpStatusCode.OK, newLoginResult.StatusCode);


            // profile setup
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

            var newTokenRequest = await client.PostAsJsonAsync("renewToken", new TokenRenewRequestModel
            {
                AccessToken = tokenResponse.AccessToken,
                RefreshToken = tokenResponse.RefreshToken,
            });

            Assert.Equal(200, (int)newTokenRequest.StatusCode);

            var newTokenReponse = await newTokenRequest.Content.ReadFromJsonAsync<TokenResponseModel>();

            Assert.NotNull(newTokenReponse.AccessToken);
            Assert.NotEqual(tokenResponse.AccessToken, newTokenReponse.AccessToken);

            // verify if refresh token is no longer valid
            var newTokenRequest2 = await client.PostAsJsonAsync("renewToken", new TokenRenewRequestModel
            {
                AccessToken = tokenResponse.AccessToken,
                RefreshToken = tokenResponse.RefreshToken,
            });
            Assert.Equal(400, (int)newTokenRequest2.StatusCode);

        }



    }

}
