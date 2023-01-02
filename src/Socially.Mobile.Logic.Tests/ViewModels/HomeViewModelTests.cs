using Socially.Apps.Consumer.Services;
using Socially.Mobile.Logic.Services;
using Socially.Mobile.Logic.ViewModels;
using Socially.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Mobile.Logic.ViewModels.Tests
{
    public class HomeViewModelTests
    {
        Mock<ISocialLogger> mockedLogger;
        Mock<IApiConsumer> mockedApiConsumer;
        Mock<ICachedContext> mockedCacheContext;
        HomeViewModel viewModel;

        void Init()
        {
            mockedLogger = new();
            mockedApiConsumer = new();
            mockedCacheContext = new();

            viewModel = new(mockedLogger.Object,
                            mockedApiConsumer.Object,
                            mockedCacheContext.Object);
        }

        [Fact]
        public async Task Get_ApiThrowsException_ShowsError()
        {
            // ARRANGE
            Init();
            mockedApiConsumer.Setup(c => c.GetHomePostsAsync(It.IsAny<int>(),
                                                              It.IsAny<DateTime?>(),
                                                              It.IsAny<CancellationToken>()))
                             .Throws(new Exception());

            // ACT
            await viewModel.GetAsync();

            // ASSERT
            Assert.NotNull(viewModel.ErrorMessage);
        }

        [Fact]
        public async Task Get_ApiResponds_Populates()
        {
            // ARRANGE
            Init();
            var res = new[]
            {
                new PostDisplayModel
                {
                    Id = 90
                }
            };
            mockedApiConsumer.Setup(c => c.GetHomePostsAsync(It.IsAny<int>(),
                                                             It.IsAny<DateTime?>(),
                                                             It.IsAny<CancellationToken>()))
                            .ReturnsAsync(res);

            // ACT
            await viewModel.GetAsync();

            // ASSERT
            Assert.Null(viewModel.ErrorMessage);
            Assert.Single(viewModel.Model);
            Assert.Equal(90, viewModel.Model.First().Id);
        }
    }
}
