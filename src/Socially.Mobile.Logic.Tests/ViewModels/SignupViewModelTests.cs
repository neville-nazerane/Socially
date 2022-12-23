using Socially.Apps.Consumer.Exceptions;
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
    public class SignupViewModelTests
    {
        private Mock<INavigationControl> mockedNavigation;
        private Mock<IMessaging> mockedMessaging;
        private Mock<IApiConsumer> mockedApiConsumer;
        private Mock<ISocialLogger> mockedLogger;
        private SignupViewModel viewModel;

        void Init()
        {
            mockedNavigation = new Mock<INavigationControl>();
            mockedMessaging = new Mock<IMessaging>();
            mockedApiConsumer = new Mock<IApiConsumer>();
            mockedLogger = new Mock<ISocialLogger>();
            viewModel = new SignupViewModel(mockedNavigation.Object,
                                            mockedMessaging.Object,
                                            mockedApiConsumer.Object,
                                            mockedLogger.Object);
        }

        [Fact]
        public async Task Submit_MissingInput_ShowsErrors()
        {
            // ARRANGE
            Init();

            // ACT
            await viewModel.SubmitAsync();

            // ASSERT
            Assert.NotEmpty(viewModel.Validation);

            mockedApiConsumer.Verify(c => c.SignupAsync(It.IsAny<SignUpModel>(),
                                                        It.IsAny<CancellationToken>()),
                                     Times.Never);

            mockedMessaging.Verify(m => m.DisplayAsync(It.IsAny<string>(),
                                                       It.IsAny<string>(),
                                                       It.IsAny<string>()),
                                   Times.Never);

            mockedNavigation.Verify(n => n.GoToLoginPageAsync(), Times.Never);

        }

        [Fact]
        public async Task Submit_ValidInput_Proceeds()
        {
            // ARRANGE
            Init();
            FillupModel();

            // ACT
            await viewModel.SubmitAsync();

            // ASSERT
            Assert.Empty(viewModel.Validation);

            mockedApiConsumer.Verify(c => c.SignupAsync(It.IsAny<SignUpModel>(),
                                            It.IsAny<CancellationToken>()),
                         Times.Once);

            mockedMessaging.Verify(m => m.DisplayAsync("Done!",
                                                       It.IsAny<string>(),
                                                       It.IsAny<string>()),
                                   Times.Once);

            mockedNavigation.Verify(n => n.GoToLoginPageAsync(), Times.Once);

        }

        [Fact]
        public async Task Submit_BadResponse_ShowsErrors()
        {
            // ARRANGE
            Init();
            FillupModel();

            var exception = new ErrorForClientException(new[]
            {
                new ErrorModel(nameof(SignUpModel.UserName), "Sample error")
            });
            mockedApiConsumer.Setup(c => c.SignupAsync(It.IsAny<SignUpModel>(), 
                                                       It.IsAny<CancellationToken>()))
                             .Throws(exception);

            // ACT
            await viewModel.SubmitAsync();

            // ARRANGE

            Assert.NotEmpty(viewModel.Validation);

            mockedMessaging.Verify(m => m.DisplayAsync(It.IsAny<string>(),
                                           It.IsAny<string>(),
                                           It.IsAny<string>()),
                       Times.Never);

            mockedNavigation.Verify(n => n.GoToLoginPageAsync(), Times.Never);
        }

        [Fact]
        public async Task Submit_ThrownException_ShowsError()
        {
            // ARRANGE
            Init();
            FillupModel();

            var exception = new Exception();
            mockedApiConsumer.Setup(c => c.SignupAsync(It.IsAny<SignUpModel>(),
                                                       It.IsAny<CancellationToken>()))
                             .Throws(exception);

            // ACT
            await viewModel.SubmitAsync();

            // ARRANGE

            Assert.Empty(viewModel.Validation);
            Assert.NotNull(viewModel.ErrorMessage);
            Assert.Equal("Failed to signup. Please try again", viewModel.ErrorMessage);

            mockedMessaging.Verify(m => m.DisplayAsync("Done!",
                                                       It.IsAny<string>(),
                                                       It.IsAny<string>()),
                                   Times.Never);

            mockedNavigation.Verify(n => n.GoToLoginPageAsync(), Times.Never);

            mockedLogger.Verify(l => l.LogException(exception, It.IsAny<string>()),
                                Times.Once);

        }

        private void FillupModel()
        {
            var model = viewModel.Model;
            model.UserName = "Test";
            model.Password = "password";
            model.ConfirmPassword = "password";
            model.Email = "boom";
        }
    }
}
