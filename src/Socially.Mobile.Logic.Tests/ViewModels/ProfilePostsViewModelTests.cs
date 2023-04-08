using Socially.Apps.Consumer.Services;
using Socially.Mobile.Logic.Services;
using Socially.Mobile.Logic.ViewModels;
using Socially.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Mobile.Logic.Tests.ViewModels
{
    public class ProfilePostsViewModelTests
    {

        ProfilePostsViewModel viewModel;
        Mock<ISocialLogger> mockedLogger;
        Mock<IApiConsumer> mockedApiConsumer;
        Mock<ICachedContext> mockedCachedContext;
        Mock<IPubSubService> mockedPubSubService;

        void Init()
        {
            mockedLogger = new();
            mockedApiConsumer = new();
            mockedCachedContext = new();
            mockedPubSubService = new();
            viewModel = new(mockedLogger.Object,
                            mockedApiConsumer.Object,
                            mockedPubSubService.Object,
                            mockedCachedContext.Object);
        }

        [Fact]
        public async Task Get_ApiThrewException_LogsError()
        {
            // ARRANGE
            Init();
            var ex = new Exception();
            mockedApiConsumer.Setup(c => c.GetCurrentUserPostsAsync(It.IsAny<int>(),
                                                It.IsAny<DateTime?>(),
                                                It.IsAny<CancellationToken>()))
                             .Throws(ex);

            // ACT
            await viewModel.GetAsync();

            // ASSERT
            mockedLogger.Verify(l => l.LogException(ex, It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task Get_ApiResponds_Populates()
        {
            // ARRANGE
            Init();
            var result = new[]
            {
                new PostDisplayModel
                {
                    Id = 20,
                }
            };
            mockedApiConsumer.Setup(c => c.GetCurrentUserPostsAsync(It.IsAny<int>(),
                                                It.IsAny<DateTime?>(),
                                                It.IsAny<CancellationToken>()))
                             .ReturnsAsync(result);

            // ACT
            await viewModel.GetAsync();

            // ASSERT
            mockedLogger.Verify(l => l.LogException(It.IsAny<Exception>(), It.IsAny<string>()), Times.Never);
            Assert.Single(viewModel.Model);
            Assert.Equal(20, viewModel.Model.First().Id);

        }

        [Fact]
        public async Task AddPost_ApiResponds_Refreshes()
        {
            // ARRANGE
            Init();

            // ACT
            await viewModel.AddPostAsync();

            // ASSERT
            mockedApiConsumer.Verify(a => a.GetCurrentUserPostsAsync(It.IsAny<int>(),
                                                                     It.IsAny<DateTime?>(),
                                                                     It.IsAny<CancellationToken>()),
                                     Times.Once);

        }

    }
}
