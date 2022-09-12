using Microsoft.EntityFrameworkCore;
using Socially.Apps.Consumer.Services;
using Socially.Core.Entities;
using Socially.Server.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Socially.WebAPI.IntegrationTests.Utils
{
    public static class ConsumerExtensions
    {

        const string testEmail = "ya@goo.com";
        const string testUsername = "username";
        const string testPassword = "pasSword!1";

        public static async Task TestSignupAndLoginAsync(this IApiConsumer consumer)
        {
            await consumer.SignupAsync(new Core.Models.SignUpModel
            {
                Password = testPassword,
                UserName = testUsername,
                Email = testEmail,
                ConfirmPassword = testPassword
            });

            var token = await consumer.LoginAsync(new Core.Models.LoginModel
            {
                UserName = testUsername,
                Password = testPassword,
                Source = "tester"
            });

            consumer.SetJwt(token.AccessToken);

        }

        public static Task<User> GetTestUserAsync(this ApplicationDbContext dbContext) 
            => dbContext.Users.SingleOrDefaultAsync(u => u.UserName == testUsername);

    }
}
