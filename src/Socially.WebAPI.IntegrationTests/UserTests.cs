using Microsoft.Extensions.DependencyInjection;
using Socially.Apps.Consumer.Services;
using Socially.Server.DataAccess;
using Socially.Server.Entities;
using Socially.WebAPI.IntegrationTests.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Socially.WebAPI.IntegrationTests
{
    public class UserTests : TestsBase
    {

        private readonly CustomWebApplicationFactory _factory;

        public UserTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetByIds_OneValidIds_GetsOne()
        {


            // ARRANGE
            var consumer = new ApiConsumer(_factory.CreateClient());


            await using var scope = _factory.Services.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();

            await consumer.TestSignupAndLoginAsync();
            var user = await dbContext.GetTestUserAsync();

            await dbContext.Users.AddRangeAsync(new User[]
            {
                new User
                {
                    Id = user.Id + 1,
                    FirstName = "Test First",
                    LastName = "Test Last"
                },
                new User
                {
                    Id = user.Id + 2,
                    FirstName = "Test2 First",
                    LastName = "Test2 Last"
                },
                new User
                {
                    Id = user.Id + 3,
                    FirstName = "Test3 First",
                    LastName = "Test3 Last"
                },
                new User
                {
                    Id = user.Id + 4,
                    FirstName = "Test4 First",
                    LastName = "Test4 Last"
                }
            });
            await dbContext.SaveChangesAsync();

            // ACT
            var res = await consumer.GetUsersByIdsAsync(new[] { user.Id + 3, user.Id + 13, user.Id + 10 });

            // ASSERT
            Assert.NotNull(res);
            Assert.NotEmpty(res);
            Assert.Single(res);

        }

        [Fact]
        public async Task Search_ValidUser_GetsOne()
        {


            // ARRANGE
            var consumer = new ApiConsumer(_factory.CreateClient());


            await using var scope = _factory.Services.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();

            await consumer.TestSignupAndLoginAsync();
            var user = await dbContext.GetTestUserAsync();

            await dbContext.Users.AddRangeAsync(new User[]
            {
                new User
                {
                    Id = user.Id + 1,
                    FirstName = "Test First",
                    LastName = "Test Last"
                },
                new User
                {
                    Id = user.Id + 2,
                    FirstName = "Test2 First",
                    LastName = "Test2 Last"
                },
                new User
                {
                    Id = user.Id + 3,
                    FirstName = "Test3 First",
                    LastName = "Test3 Last"
                },
                new User
                {
                    Id = user.Id + 4,
                    FirstName = "Test4 First",
                    LastName = "Test4 Last"
                }
            });
            await dbContext.SaveChangesAsync();

            // ACT
            var res = await consumer.SearchUserAsync("4");

            // ASSERT
            Assert.NotNull(res);
            Assert.NotEmpty(res);
            Assert.Single(res);

        }

    }
}
