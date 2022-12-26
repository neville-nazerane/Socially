using Socially.Apps.Consumer.Exceptions;
using Socially.Apps.Consumer.Services;
using Socially.Mobile.Logic.Services;
using Socially.Mobile.Logic.Tests.Utils;
using Socially.Models;
using System;

namespace Socially.Mobile.Logic.ViewModels.Tests
{
    public class LoginViewModelTests
    {
        private Mock<IApiConsumer> mockedApiConsumer;
        private Mock<IAuthAccess> mockedAuthAccess;
        private Mock<ISocialLogger> mockedLogger;
        private Mock<INavigationControl> mockedNavigation;
        private Mock<IMessaging> mockedMessaging;
        private LoginViewModel viewModel;

        private void Init()
        {
            mockedApiConsumer = new Mock<IApiConsumer>();
            mockedAuthAccess = new Mock<IAuthAccess>();
            mockedLogger = new Mock<ISocialLogger>();
            mockedNavigation = new Mock<INavigationControl>();
            mockedMessaging = new();
            viewModel = new LoginViewModel(mockedApiConsumer.Object,
                                           mockedAuthAccess.Object,
                                           mockedNavigation.Object,
                                           mockedMessaging.Object,
                                           mockedLogger.Object);
        }

        [Fact]
        public async Task AttemptLogin_NoUsernameOrPassword_ShowsValidationExceptions()
        {
            // ARRANGE
            Init();

            // ACT
            await viewModel.SubmitAsync();

            // ASSERT

            // ensure no server call is made
            mockedApiConsumer.Verify(c => c.LoginAsync(It.IsAny<LoginModel>(),
                                                       It.IsAny<CancellationToken>()),
                                     Times.Never);

            mockedAuthAccess.Verify(a => a.SetStoredTokenAsync(It.IsAny<TokenResponseModel>()),
                                    Times.Never);

            mockedNavigation.Verify(a => a.GoToHomeAsync(), Times.Never);

            bool isValidUsername = viewModel.Validation.IsValidProperty(nameof(LoginModel.UserName));
            bool isValidPassword = viewModel.Validation.IsValidProperty(nameof(LoginModel.Password));

            Assert.False(isValidUsername);
            Assert.False(isValidPassword);
        }

        [Fact]
        public async Task AttemptLogin_ValidInput_NoErrors()
        {
            // ARRANGE
            Init();
            viewModel.Model.UserName = "neville";
            viewModel.Model.Password = "neville";

            // ACT
            await viewModel.SubmitAsync();

            // ASSERT

            mockedApiConsumer.Verify(c => c.LoginAsync(It.IsAny<LoginModel>(),
                                           It.IsAny<CancellationToken>()),
                         Times.Once);

            mockedAuthAccess.Verify(a => a.SetStoredTokenAsync(It.IsAny<TokenResponseModel>()),
                                    Times.Once);

            mockedNavigation.Verify(a => a.GoToHomeAsync(), Times.Once);


            Assert.Null(viewModel.ErrorMessage);

            bool isValidUsername = viewModel.Validation.IsValidProperty(nameof(LoginModel.UserName));
            bool isValidPassword = viewModel.Validation.IsValidProperty(nameof(LoginModel.Password));

            Assert.True(isValidUsername);
            Assert.True(isValidPassword);

        }

        [Fact]
        public async Task AttemptLogin_NoPassword_ShowsValidationExceptions()
        {
            // ARRANGE
            Init();
            viewModel.Model.UserName = "neville";

            // ACT
            await viewModel.SubmitAsync();

            // ASSERT

            // ensure no server call is made
            mockedApiConsumer.Verify(c => c.LoginAsync(It.IsAny<LoginModel>(),
                                                       It.IsAny<CancellationToken>()),
                                     Times.Never);

            mockedAuthAccess.Verify(a => a.SetStoredTokenAsync(It.IsAny<TokenResponseModel>()),
                                    Times.Never);

            mockedNavigation.Verify(a => a.GoToHomeAsync(), Times.Never);


            bool isValidUsername = viewModel.Validation.IsValidProperty(nameof(LoginModel.UserName));
            bool isValidPassword = viewModel.Validation.IsValidProperty(nameof(LoginModel.Password));

            Assert.True(isValidUsername);
            Assert.False(isValidPassword);
        }

        [Fact]
        public async Task AttemptLogin_NoUsername_ShowsValidationExceptions()
        {
            // ARRANGE
            Init();
            viewModel.Model.Password = "verysecret";

            // ACT
            await viewModel.SubmitAsync();

            // ASSERT

            // ensure no server call is made
            mockedApiConsumer.Verify(c => c.LoginAsync(It.IsAny<LoginModel>(),
                                                       It.IsAny<CancellationToken>()),
                                     Times.Never);

            mockedAuthAccess.Verify(a => a.SetStoredTokenAsync(It.IsAny<TokenResponseModel>()),
                                    Times.Never);

            mockedNavigation.Verify(a => a.GoToHomeAsync(), Times.Never);


            bool isValidUsername = viewModel.Validation.IsValidProperty(nameof(LoginModel.UserName));
            bool isValidPassword = viewModel.Validation.IsValidProperty(nameof(LoginModel.Password));

            Assert.False(isValidUsername);
            Assert.True(isValidPassword);
        }

        [Fact]
        public async Task AttemptLogin_SetTokenThrowsException_DisplaysError()
        {
            // ARRANGE
            Init();
            viewModel.Model.UserName = "neville";
            viewModel.Model.Password = "neville";

            var ex = new Exception();
            mockedAuthAccess.Setup(a => a.SetStoredTokenAsync(It.IsAny<TokenResponseModel>()))
                            .Throws(ex);

            // ACT
            await viewModel.SubmitAsync();

            // ASSERT
            mockedApiConsumer.Verify(c => c.LoginAsync(It.IsAny<LoginModel>(),
                               It.IsAny<CancellationToken>()),
             Times.Once);

            mockedNavigation.Verify(a => a.GoToHomeAsync(), Times.Never);


            Assert.Equal("Failed to store login information", viewModel.ErrorMessage);
            mockedLogger.Verify(l => l.LogException(ex, null));
        }

        [Fact]
        public async Task AttemptLogin_BadRequest_PopulatesValidation()
        {
            // ARRANGE
            Init();
            viewModel.Model.UserName = "neville";
            viewModel.Model.Password = "neville";

            var exception = new ErrorForClientException
            (
                new[] {
                    new ErrorModel
                    {
                        Field = nameof(LoginModel.UserName),
                        Errors = new[] { "I don't like that username" }
                    }
                }
            );
            mockedApiConsumer.Setup(c => c.LoginAsync(It.IsAny<LoginModel>(), It.IsAny<CancellationToken>()))
                             .Throws(exception);

            // ACT
            await viewModel.SubmitAsync();

            // ASSERT
            mockedAuthAccess.Verify(a => a.SetStoredTokenAsync(It.IsAny<TokenResponseModel>()),
                                    Times.Never);

            mockedNavigation.Verify(a => a.GoToHomeAsync(), Times.Never);


            Assert.Null(viewModel.ErrorMessage);

            bool isValidUsername = viewModel.Validation.IsValidProperty(nameof(LoginModel.UserName));
            bool isValidPassword = viewModel.Validation.IsValidProperty(nameof(LoginModel.Password));

            Assert.True(isValidPassword);
            Assert.False(isValidUsername);

            var usernameErrors = viewModel.Validation.GetErrorsForProperty(nameof(LoginModel.UserName));
            Assert.Single(usernameErrors);
            Assert.Equal("I don't like that username", usernameErrors.First());
        }

        [Fact]
        public async Task AttemptLogin_ApiCallThrows_ShowsError()
        {
            // ARRANGE
            Init();
            viewModel.Model.UserName = "neville";
            viewModel.Model.Password = "neville";

            var ex = new Exception();
            mockedApiConsumer.Setup(c => c.LoginAsync(It.IsAny<LoginModel>(), It.IsAny<CancellationToken>()))
                             .Throws(ex);

            // ACT
            await viewModel.SubmitAsync();

            // ASSERT
            mockedAuthAccess.Verify(a => a.SetStoredTokenAsync(It.IsAny<TokenResponseModel>()),
                                    Times.Never);

            mockedNavigation.Verify(a => a.GoToHomeAsync(), Times.Never);


            bool isValidUsername = viewModel.Validation.IsValidProperty(nameof(LoginModel.UserName));
            bool isValidPassword = viewModel.Validation.IsValidProperty(nameof(LoginModel.Password));

            Assert.True(isValidPassword);
            Assert.True(isValidUsername);

            Assert.NotNull(viewModel.ErrorMessage);
            Assert.Equal("Failed to login", viewModel.ErrorMessage);

            mockedLogger.Verify(l => l.LogException(ex, null), Times.Once);
        }

    }
}
