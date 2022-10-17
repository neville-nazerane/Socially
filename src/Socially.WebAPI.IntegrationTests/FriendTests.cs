using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Socially.Apps.Consumer.Services;
using Socially.Server.DataAccess;
using Socially.Server.Entities;
using Socially.WebAPI.IntegrationTests.Utils;
using Socially.WebAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Socially.WebAPI.IntegrationTests
{
    public class FriendTests : IClassFixture<CustomWebApplicationFactory>
    {

        private readonly CustomWebApplicationFactory _factory;

        public FriendTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Request()
        {

            // ARRANGE
            var consumer = new ApiConsumer(_factory.CreateClient());


            await using var scope = _factory.Services.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();

            await consumer.TestSignupAndLoginAsync();
            var user = await dbContext.GetTestUserAsync();

            // ACT
            var res = await consumer.RequestFriendAsync(20);

            // ASSERT

            Assert.Equal(200, (int)res.StatusCode);
            
            var requests = await dbContext.FriendRequests.ToListAsync();
            Assert.Single(requests);
            
            var request = requests.First();
            Assert.NotNull(request);
            Assert.Equal(20, request.ForId);
            Assert.Equal(user.Id, request.RequesterId);

        }

        [Fact]
        public async Task Responds_AcceptRequest_Friended()
        {

            // ARRANGE
            var consumer = new ApiConsumer(_factory.CreateClient());

            await using var scope = _factory.Services.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();

            await consumer.TestSignupAndLoginAsync();
            var user = await dbContext.GetTestUserAsync();

            await dbContext.FriendRequests.AddRangeAsync(new Server.Entities.FriendRequest
            {
                ForId = user.Id,
                RequesterId = 12
            });
            await dbContext.SaveChangesAsync();


            // ACT
            await consumer.RespondAsync(12, true);

            // ASSERT

            var requests = await dbContext.FriendRequests.ToListAsync();
            Assert.Single(requests);

            var request = requests.First();
            Assert.Equal(user.Id, request.ForId);
            Assert.Equal(12, request.RequesterId);
            
            var friends = await dbContext.Friends.ToListAsync();
            Assert.Equal(2, friends.Count());
            var currentUserFriend = friends.SingleOrDefault(f => f.OwnerUserId == user.Id);
            Assert.NotNull(currentUserFriend);
            Assert.Equal(12, currentUserFriend.FriendUserId);

            var otherUserFriend = friends.SingleOrDefault(f => f.OwnerUserId == 12);
            Assert.NotNull(otherUserFriend);
            Assert.Equal(user.Id, otherUserFriend.FriendUserId);
        }

        [Fact]
        public async Task Responds_RejectRequest_NoFriendCreated()
        {

            // ARRANGE
            var consumer = new ApiConsumer(_factory.CreateClient());

            await using var scope = _factory.Services.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();

            await consumer.TestSignupAndLoginAsync();
            var user = await dbContext.GetTestUserAsync();

            await dbContext.FriendRequests.AddRangeAsync(new Server.Entities.FriendRequest
            {
                ForId = user.Id,
                RequesterId = 12
            });
            await dbContext.SaveChangesAsync();


            // ACT
            await consumer.RespondAsync(12, false);

            // ASSERT

            var requests = await dbContext.FriendRequests.ToListAsync();
            Assert.Single(requests);

            var friends = await dbContext.Friends.ToListAsync();
            Assert.Empty(friends);
        }

        [Fact]
        public async Task GetRequests()
        {
            // ARRANGE
            var consumer = new ApiConsumer(_factory.CreateClient());

            await using var scope = _factory.Services.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();

            await consumer.TestSignupAndLoginAsync();
            var user = await dbContext.GetTestUserAsync();

            await dbContext.FriendRequests.AddRangeAsync(new FriendRequest[]
            {
                new FriendRequest
                {
                    RequesterId = user.Id,
                    ForId = 20
                },
                new FriendRequest
                {
                    ForId = user.Id,
                    Requester = new User
                    {
                        Id = 21
                    }
                },
                new FriendRequest
                {
                    ForId = user.Id,
                    Requester = new User
                    {
                        Id = 22
                    }
                },
                new FriendRequest
                {
                    ForId = 30,
                    Requester = new User
                    {
                        Id = 31
                    }
                }
            });
            await dbContext.SaveChangesAsync();

            // ACT
            var requests = await consumer.GetFriendRequestsAsync();

            Assert.NotNull(requests);
            Assert.NotEmpty(requests);
            Assert.Equal(2, requests.Count());

        }

        [Fact]
        public async Task GetFriends()
        {
            // ARRANGE
            var consumer = new ApiConsumer(_factory.CreateClient());

            await using var scope = _factory.Services.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();

            await consumer.TestSignupAndLoginAsync();
            var user = await dbContext.GetTestUserAsync();
            await dbContext.Friends.AddRangeAsync(new Friend[]
            {
                new Friend
                {
                    FriendUser = new User
                    {
                        FirstName = "name",
                        LastName = "other name"
                    },
                    OwnerUserId = user.Id
                },
                new Friend
                {
                    FriendUser = new User
                    {
                        FirstName = "name",
                        LastName = "other name"
                    },
                    OwnerUserId = user.Id
                },
                new Friend
                {
                    FriendUser = new User
                    {
                        FirstName = "name",
                        LastName = "other name"
                    },
                    OwnerUserId = user.Id + 2
                }
            });;
            await dbContext.SaveChangesAsync();

            // ACT
            var friends = await consumer.GetFriendsAsync();

            // ASSERT
            Assert.NotNull(friends);
            Assert.NotEmpty(friends);
            Assert.Equal(2, friends.Count());

        }

    }
}
