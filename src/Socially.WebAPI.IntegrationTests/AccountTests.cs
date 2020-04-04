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

            var signupModel = new SignUpModel
            {
                Email = "ya@goo.com",
                UserName = "username",
                Password = "pasSword!2",
                ConfirmPassword = "pasSword!2"
            };

            var signinRes = await client.PostAsJsonAsync("signup", signupModel);
            Assert.True(signinRes.IsSuccessStatusCode,
                                $"Sign in had error code {signinRes.StatusCode} saying '{await signinRes.Content.ReadAsStringAsync()}'");


        }

    }

}
