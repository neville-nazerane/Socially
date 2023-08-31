using Microsoft.EntityFrameworkCore;
using Socially.Server.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Socially.Server.Managers.Tests
{
    public class RealTimeManagerTests : TestBase
    {

        RealTimeManager manager;

        public RealTimeManagerTests()
        {
            manager = new(RealTimeDbContext);
        }


        [Fact]
        public async Task SubscribeForPosts_AddThree_ThreeAdded()
        {
            // ARRANGE
            var posts = new[] { 32, 12, 55 };
            string connId = Guid.NewGuid().ToString("N");

            // ACT
            await manager.SubscribeForPostsAsync(connId, posts);

            // ASSERT
            var postCount = await RealTimeDbContext.PostConnections.CountAsync();
            Assert.Equal(3, postCount);

        }

        [Fact]
        public async Task GetPostConnectionIds_ThreeConnectionsForPostId_ReturnsThree()
        {
            // ARRANGE
            int targetPostId = 101; // Replace with an actual PostId from your sample data
            var postConnections = new PostConnection[]
            {
                new() { PostId = targetPostId, ConnectionId = "conn1" },
                new() { PostId = targetPostId, ConnectionId = "conn2" },
                new() { PostId = targetPostId, ConnectionId = "conn3" },
                new() { PostId = 102, ConnectionId = "conn4" } // Different PostId
            };

            await RealTimeDbContext.PostConnections.AddRangeAsync(postConnections);
            await RealTimeDbContext.SaveChangesAsync();

            // ACT
            var resultAsync = manager.GetPostConnectionIdsAsync(targetPostId);
            var count = resultAsync.ToBlockingEnumerable().Count(); // Count the number of items

            // ASSERT
            Assert.Equal(3, count);
        }

        [Fact]
        public async Task SubscribeForUsers_AddFour_FourAdded()
        {
            // ARRANGE
            var userIds = new[] { 7, 42, 69, 100 };
            string connId = Guid.NewGuid().ToString("N");

            // ACT
            await manager.SubscribeForUsersAsync(connId, userIds);

            // ASSERT
            var userCount = await RealTimeDbContext.UserConnections.CountAsync();
            Assert.Equal(4, userCount);
        }

        [Fact]
        public async Task GetUserConnectionIds_TwoConnectionsForUserId_ReturnsTwo()
        {
            // ARRANGE
            int targetUserId = 101; // Replace with an actual UserId from your sample data
            var userConnections = new UserConnection[]
            {
                new() { UserId = targetUserId, ConnectionId = "conn1" },
                new() { UserId = targetUserId, ConnectionId = "conn2" },
                new() { UserId = 102, ConnectionId = "conn3" } // Different UserId
            };

            await RealTimeDbContext.UserConnections.AddRangeAsync(userConnections);
            await RealTimeDbContext.SaveChangesAsync();

            // ACT
            var resultAsync = manager.GetUserConnectionIdsAsync(targetUserId);
            var count = resultAsync.ToBlockingEnumerable().Count(); // Count the number of items

            // ASSERT
            Assert.Equal(2, count);
        }


    }
}
