using Castle.Core.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Socially.Models;
using Socially.Server.Managers.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Socially.Server.Managers.Tests
{
    public class FriendManagerTests : TestBase
    {
        private Mock<ILogger<FriendManager>> mockedLogger;
        private FriendManager manager;

        private ValueTask SetupManagerAsync()
        {
            mockedLogger = new Mock<ILogger<FriendManager>>();
            manager = new FriendManager(DbContext, mockedLogger.Object);
            return ValueTask.CompletedTask;
        }

        [Fact, Trait("action", "add"), Trait("happyPath", "fail")]
        public async Task Request_AlreadyRequestedByCurrentUser_Throws()
        {
            // ARRANGE
            int currentUserId = 10;
            await SetupManagerAsync();
            DateTime createdTime = DateTime.UtcNow;
            await DbContext.FriendRequests.AddAsync(new Entities.FriendRequest
            {
                ForId = 8,
                RequesterId = currentUserId,
                RequestedOn = createdTime
            });
            await DbContext.SaveChangesAsync();

            // ACT & ASSERT
            var exception = await Assert.ThrowsAsync<FriendRequestExistsException>(() => manager.RequestAsync(currentUserId, 8));
            Assert.Equal(currentUserId, exception.Model.RequesterId);
            Assert.Equal(createdTime, exception.Model.RequestedOn);

        }

        [Fact, Trait("action", "add"), Trait("happyPath", "fail")]
        public async Task Request_AlreadyRequestedByOtherUser_Throws()
        {
            // ARRANGE
            int currentUserId = 10;
            await SetupManagerAsync();
            DateTime createdTime = DateTime.UtcNow;
            await DbContext.FriendRequests.AddAsync(new Entities.FriendRequest
            {
                ForId = currentUserId,
                RequesterId = 8,
                RequestedOn = createdTime
            });
            await DbContext.SaveChangesAsync();

            // ACT & ASSERT
            var exception = await Assert.ThrowsAsync<FriendRequestExistsException>(() => manager.RequestAsync(currentUserId, 8));
            Assert.Equal(8, exception.Model.RequesterId);
            Assert.Equal(createdTime, exception.Model.RequestedOn);
        }

        [Fact, Trait("action", "add"), Trait("happyPath", "success")]
        public async Task Request_RequestDoesntExist_Requests()
        {
            // ARRANGE
            await SetupManagerAsync();
            int currentUserId = 10;

            // ACT
            await manager.RequestAsync(currentUserId, 11);

            // ASSERT
            var requests = await DbContext.FriendRequests.ToListAsync();
            Assert.NotEmpty(requests);
            Assert.Single(requests);
            
            var request = requests.First();
            Assert.Equal(currentUserId, request.RequesterId);
            Assert.Equal(11, request.ForId);

        }

        [Fact, Trait("action", "add"), Trait("happyPath", "success")]
        public async Task Request_CurrentUserRequestedOthers_Requests()
        {
            // ARRANGE
            await SetupManagerAsync();
            int currentUserId = 10;
            await DbContext.FriendRequests.AddAsync(new Entities.FriendRequest
            {
                ForId = 8,
                RequesterId = currentUserId,
                RequestedOn = DateTime.UtcNow
            });
            await DbContext.SaveChangesAsync();

            // ACT
            await manager.RequestAsync(currentUserId, 11);

            // ASSERT
            var requests = await DbContext.FriendRequests.ToListAsync();
            Assert.NotEmpty(requests);
            Assert.Equal(2, requests.Count);

        }

        [Fact, Trait("action", "add"), Trait("happyPath", "success")]
        public async Task Request_CurrentUserHasOtherRequests_Requests()
        {
            // ARRANGE
            await SetupManagerAsync();
            int currentUserId = 10;
            await DbContext.FriendRequests.AddAsync(new Entities.FriendRequest
            {
                ForId = currentUserId,
                RequesterId = 8,
                RequestedOn = DateTime.UtcNow
            });
            await DbContext.SaveChangesAsync();

            // ACT
            await manager.RequestAsync(currentUserId, 11);

            // ASSERT
            var requests = await DbContext.FriendRequests.ToListAsync();
            Assert.NotEmpty(requests);
            Assert.Equal(2, requests.Count);

        }


        [Fact]
        public async Task Respond_RequestDoesntExist_Warns()
        {
            // ARRANGE
            await SetupManagerAsync();

            // ACT
            bool success = await manager.RespondAsync(10, 20, true);

            // ASSERT
            Assert.False(success);
            mockedLogger.VerifyLog(l => l.LogWarning(It.IsAny<string>(), It.IsIn(10), It.IsIn(20)));
        }

        [Fact]
        public async Task Respond_RequestedByCurrent_Warns()
        {
            // ARRANGE
            await SetupManagerAsync();
            int currentUserId = 10;
            await DbContext.FriendRequests.AddAsync(new Entities.FriendRequest
            {
                RequestedOn = DateTime.UtcNow,
                RequesterId = currentUserId,
                ForId = 11
            });
            await DbContext.SaveChangesAsync();

            // ACT
            bool success = await manager.RespondAsync(11, currentUserId, true);

            // ASSERT
            Assert.False(success);
            mockedLogger.VerifyLog(l => l.LogWarning(It.IsAny<string>(), It.IsIn(11), It.IsIn(currentUserId)));

        }

        [Fact]
        public async Task Respond_RequestedByDifferentUser_Warns()
        {
            // ARRANGE
            await SetupManagerAsync();
            int currentUserId = 10;
            await DbContext.FriendRequests.AddAsync(new Entities.FriendRequest
            {
                RequestedOn = DateTime.UtcNow,
                RequesterId = 31,
                ForId = currentUserId
            });
            await DbContext.SaveChangesAsync();

            // ACT
            bool success = await manager.RespondAsync(11, currentUserId, true);

            // ASSERT
            Assert.False(success);
            mockedLogger.VerifyLog(l => l.LogWarning(It.IsAny<string>(), It.IsIn(11), It.IsIn(currentUserId)));

        }

        [Fact]
        public async Task Respond_RejectRequestByTargetUser_MarksAsRejected()
        {
            // ARRANGE
            await SetupManagerAsync();
            int currentUserId = 10;
            await DbContext.FriendRequests.AddAsync(new Entities.FriendRequest
            {
                RequestedOn = DateTime.UtcNow,
                RequesterId = 11,
                ForId = currentUserId
            });
            await DbContext.SaveChangesAsync();

            // ACT
            bool success = await manager.RespondAsync(11, currentUserId, false);

            // ASSERT
            Assert.True(success);

            var requests = await DbContext.FriendRequests.SingleAsync();
            Assert.False(requests.IsAccepted);

            var friends = await DbContext.Friends.ToListAsync();
            Assert.Empty(friends);
        }

        [Fact]
        public async Task Respond_ApproveRequestByTargetUser_CreatesFriend()
        {
            // ARRANGE
            await SetupManagerAsync();
            int currentUserId = 10;
            await DbContext.FriendRequests.AddAsync(new Entities.FriendRequest
            {
                RequestedOn = DateTime.UtcNow,
                RequesterId = 11,
                ForId = currentUserId
            });
            await DbContext.SaveChangesAsync();

            // ACT
            bool success = await manager.RespondAsync(11, currentUserId, true);

            // ASSERT
            Assert.True(success);

            var requests = await DbContext.FriendRequests.SingleAsync();
            Assert.True(requests.IsAccepted);

            var friends = await DbContext.Friends.ToListAsync();
            Assert.NotEmpty(friends);
            Assert.Equal(2, friends.Count);

            var friend1 = friends.SingleOrDefault(f => f.OwnerUserId == currentUserId);
            Assert.NotNull(friend1);
            Assert.Equal(11, friend1.FriendUserId);

            var friend2 = friends.SingleOrDefault(f => f.OwnerUserId == 11);
            Assert.NotNull(friend2);
            Assert.Equal(currentUserId, friend2.FriendUserId);

        }

    }
}
