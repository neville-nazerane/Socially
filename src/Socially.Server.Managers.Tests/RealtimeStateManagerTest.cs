using Azure.Data.Tables;
using Moq;
using Socially.Server.DataAccess;
using Socially.Server.Managers.Tests.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Socially.Server.Managers.Tests
{
    public class RealtimeStateManagerTests : TestBase
    {
        private readonly Mock<ITableAccess> mockedTableAccess;
        private readonly RealtimeStateManager manager;

        public RealtimeStateManagerTests()
        {
            mockedTableAccess = new Mock<ITableAccess>();
            manager = new RealtimeStateManager(mockedTableAccess.Object);
        }

        [Fact]
        public async Task Register_ThreeItems_AddsThreeToBoth()
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
        public async Task Unregister_ThreeTags_RemovesThreeFromBoth()
        {
            // ARRANGE
            var connId = "connection id";
            var toDelete = Enumerable.Range(0, 3)
                                     .Select(d => new TableEntity(connId, Guid.NewGuid().ToString()))
                                     .ToAsyncEnumerable();

            mockedTableAccess.Setup(t => t.ListByPartitionAsync(It.IsAny<string>(),
                                                                It.IsAny<string>(),
                                                                It.IsAny<int>()))
                             .Returns(toDelete);

            // ACT
            await manager.UnregisterAsync(connId);

            // ASSERT

            mockedTableAccess.Verify(t => t.DeleteAsync(It.IsAny<string>(),
                                                        connId,
                                                        It.IsAny<string>(),
                                                        It.IsAny<CancellationToken>()),
                                    Times.Exactly(3));

            mockedTableAccess.Verify(t => t.DeleteAllWithPartitionAsync(It.IsAny<string>(),
                                                                        connId),
                                    Times.Once);

        }

        [Fact]
        public void GetConnectionIds_Contains4_GetsAll4()
        {
            // ARRANGE
            var tag = "my tag";
            var elements = Enumerable.Range(0, 3)
                                     .Select(d => new TableEntity(tag, Guid.NewGuid().ToString()))
                                     .ToAsyncEnumerable();
            mockedTableAccess.Setup(t => t.ListByPartitionAsync(It.IsAny<string>(),
                                                                It.IsAny<string>(),
                                                                It.IsAny<int>()))
                             .Returns(elements);


            // ACT
            var connIds = manager.GetConnectionIdsAsync(tag, 10);

            // ASSERT
            var ids = connIds.ToBlockingEnumerable();
            
            Assert.Equal(3, ids.Count());
            Assert.NotEqual("my tag", ids.First());

        }

    }
}
