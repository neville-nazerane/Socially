using Socially.Apps.Consumer.Exceptions;
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
    public class ForgotPasswordViewModelTests
    {
        private Mock<IMessaging> mockedMessaging;
        private Mock<IApiConsumer> mockedApiConsumer;
        private Mock<ISocialLogger> mockedLogger;
        private ForgotPasswordViewModel viewModel;

        private void Init()
        {
            mockedMessaging = new Mock<IMessaging>();
            mockedApiConsumer = new Mock<IApiConsumer>();
            mockedLogger = new Mock<ISocialLogger>();
            viewModel = new(mockedMessaging.Object,
                            mockedApiConsumer.Object,
                            mockedLogger.Object);
        }

        [Fact]
        public async Task Submit_NoEmail_ShowsError()
        {
            // ARRANGE
            Init();

            // ACT
            await viewModel.SubmitAsync();

            // ASSERT
            Assert.NotNull(viewModel.ErrorMessage);
            Assert.Equal("Enter an email", viewModel.ErrorMessage);
            VerifyDoneMessage(Times.Never);

        }

        [Fact]
        public async Task Submit_ValidSubmit_Proceeds()
        {
            // ARRANGE
            Init();
            viewModel.Email = "hello@yahoo.com";

            // ACT
            await viewModel.SubmitAsync();

            // ASSERT
            Assert.Null(viewModel.ErrorMessage);

            mockedApiConsumer.Verify(c => c.ForgotPasswordAsync("hello@yahoo.com", It.IsAny<CancellationToken>()),
                                     Times.Once);

            VerifyDoneMessage(Times.Once);

        }

        [Fact]
        public async Task Submit_BadRequest_ShowsError()
        {

            // ARRANGE
            Init();
            viewModel.Email = "hello@yahoo.com";

            var ex = new ErrorForClientException(new[] {
                new ErrorModel(nameof(viewModel.Email), "Failed to send")
            });

            mockedApiConsumer.Setup(c => c.ForgotPasswordAsync(It.IsAny<string>(),
                                                               It.IsAny<CancellationToken>()))
                             .Throws(ex);

            // ACT 
            await viewModel.SubmitAsync();

            // ASSERT
            Assert.NotNull(viewModel.ErrorMessage);
            Assert.Equal("Failed to send", viewModel.ErrorMessage);

            VerifyDoneMessage(Times.Never);

        }

        [Fact]
        public async Task Submit_Exception_ShowsError()
        {
            // ARRANGE
            Init();
            viewModel.Email = "hello@yahoo.com";

            var ex = new Exception("Don't show this");
            mockedApiConsumer.Setup(c => c.ForgotPasswordAsync(It.IsAny<string>(),
                                                               It.IsAny<CancellationToken>()))
                            .Throws(ex);

            // ACT
            await viewModel.SubmitAsync();

            // ASSERT
            Assert.Equal("Failed. Try again", viewModel.ErrorMessage);
            VerifyDoneMessage(Times.Never);

        }

        private void VerifyDoneMessage(Func<Times> times)
        {
            mockedMessaging.Verify(c => c.DisplayAsync("Done!",
                                                       It.IsAny<string>(),
                                                       It.IsAny<string>()),
                                   times);
        }
    }
}
