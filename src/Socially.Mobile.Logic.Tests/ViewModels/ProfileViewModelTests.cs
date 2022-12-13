using Socially.Apps.Consumer.Services;
using Socially.Mobile.Logic.Services;
using Socially.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Mobile.Logic.ViewModels.Tests
{
    public class ProfileViewModelTests
    {

        Mock<ISocialLogger> mockedLogger;
        Mock<IApiConsumer> mockedApiConsumer;
        ProfileViewModel viewModel;

        void Init()
        {
            mockedLogger = new();
            mockedApiConsumer = new();
            viewModel = new(mockedLogger.Object, mockedApiConsumer.Object);
        }

        [Fact]
        public async Task Get_ApiThrows_ShowsError()
        {
            // ARRANGE
            Init();
            var ex = new Exception();
            mockedApiConsumer.Setup(c => c.GetCurrentUserSummary(It.IsAny<CancellationToken>()))
                             .Throws(ex);

            // ACT
            await viewModel.GetAsync();

            // ASSERT
            Assert.NotNull(viewModel.ErrorMessage);
            mockedLogger.Verify(l => l.LogException(ex, It.IsAny<string>()), Times.Once);

        }

        [Fact]
        public async Task Get_ApiResponds_Populates()
        {
            // ARRANGE
            Init();
            var model = new UserSummaryModel
            {
                Id = 11
            };
            mockedApiConsumer.Setup(c => c.GetCurrentUserSummary(It.IsAny<CancellationToken>()))
                             .ReturnsAsync(model);

            // ACT
            await viewModel.GetAsync();

            // ASSERT
            Assert.Null(viewModel.ErrorMessage);
            Assert.NotNull(viewModel.Model);
            Assert.Equal(11, viewModel.Model.Id);

        }

    }
}
