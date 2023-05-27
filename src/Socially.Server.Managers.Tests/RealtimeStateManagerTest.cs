using Azure.Data.Tables;
using Moq;
using Socially.Server.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Socially.Server.Managers.Tests
{
    public class RealtimeStateManagerTest : TestBase
    {
        private readonly Mock<ITableAccess> mockedTableAccess;
        private readonly RealtimeStateManager manager;

        public RealtimeStateManagerTest()
        {
            mockedTableAccess = new Mock<ITableAccess>();
            manager = new RealtimeStateManager(mockedTableAccess.Object);
        }

        [Fact]
        public async void Register_ThreeItems_AddsThreeToBoth()
        {
            // ARRANGE
            var tags = Enumerable.Repeat("My tag", 3);
            var connId = "connection id";

            // ACT
            await manager.RegisterAsync(tags, connId);

            // ASSERT
            mockedTableAccess.Verify(m => m.AddRangeAsync(It.IsAny<string>(), 
                                                          It.Is<IEnumerable<ITableEntity>>(e => e.Count() == 3),
                                                          It.IsAny<CancellationToken>()),
                                     Times.Once);
            mockedTableAccess.Verify(m => m.AddAsync(It.IsAny<string>(),
                                                     It.Is<ITableEntity>(e => e.PartitionKey == "connection id"),
                                                     It.IsAny<CancellationToken>()),
                                     Times.Exactly(3));

        }

        [Fact]
        public async void Unregister_ThreeTags_RemovesThreeFromBoth()
        {

        }

    }
}
