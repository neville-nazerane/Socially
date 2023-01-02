using Socially.Apps.Consumer.Services;
using Socially.Mobile.Logic.Services;
using Socially.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Mobile.Logic.ViewModels.Tests
{
    public class ImagePickerViewModelTests
    {
        Mock<IApiConsumer> mockedApiConsumer;
        Mock<ISocialLogger> mockedLogger;
        Mock<IMessaging> mockedMessaging;
        Mock<INavigationControl> mockedNavigation;
        ImagePickerViewModel viewModel;

        void Init()
        {
            mockedApiConsumer = new();
            mockedLogger = new();
            mockedMessaging = new();
            mockedNavigation = new();

            viewModel = new(mockedApiConsumer.Object,
                            mockedLogger.Object,
                            mockedNavigation.Object,
                            mockedMessaging.Object);
        }

        [Fact]
        public async Task Get_ApiThrowsException_ShowsError()
        {
            // ARRANGE
            Init();

            var exception = new Exception();
            mockedApiConsumer.Setup(c => c.GetAllImagesOfUserAsync(It.IsAny<CancellationToken>()))
                             .Throws(exception);

            // ACT
            await viewModel.GetAsync();

            // ASSERT
            VerifyExceptionThrown(exception);
        }



        [Fact]
        public async Task Get_ApiReturns_StoresData()
        {
            // ARRANGE
            Init();

            var res = new ObservableCollection<string>();
            mockedApiConsumer.Setup(c => c.GetAllImagesOfUserAsync(It.IsAny<CancellationToken>()))
                             .ReturnsAsync(res);

            // ACT
            await viewModel.GetAsync();

            // ASSERT
            mockedLogger.Verify(l => l.LogException(It.IsAny<Exception>(), It.IsAny<string>()),
                                Times.Never);

            Assert.Equal(res, viewModel.Model);
        }

        [Fact]
        public async Task AddImage_ApiThrowsException_DisplaysErrors()
        {
            // ARRANGE
            Init();
            var ex = new Exception();
            mockedApiConsumer.Setup(c => c.UploadAsync(It.IsAny<ImageUploadModel>(),
                                                       It.IsAny<CancellationToken>()))
                            .Throws(ex);

            // ACT
            await viewModel.AddImageAsync(null);

            // ASSERT
            VerifyExceptionThrown(ex);
            mockedApiConsumer.Verify(c => c.GetAllImagesOfUserAsync(It.IsAny<CancellationToken>()),
                                    Times.Never);

        }

        [Fact]
        public async Task AddImage_ApiSucceeds_UpdatesFromServer()
        {
            // ARRANGE
            Init();

            // ACT
            await viewModel.AddImageAsync(null);

            // ASSERT
            mockedApiConsumer.Verify(c => c.GetAllImagesOfUserAsync(It.IsAny<CancellationToken>()),
                                     Times.Once);

        }


        private void VerifyExceptionThrown(Exception exception)
        {
            mockedLogger.Verify(l => l.LogException(exception, It.IsAny<string>()), Times.Once);
            mockedMessaging.Verify(m => m.DisplayAsync("Failed!",
                                                       It.IsAny<string>(),
                                                       It.IsAny<string>()),
                                    Times.Once);
        }

    }
}
