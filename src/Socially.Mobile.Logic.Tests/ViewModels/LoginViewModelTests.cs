using Socially.Apps.Consumer.Services;
using Socially.Mobile.Logic.Tests.Utils;
using Socially.Models;

namespace Socially.Mobile.Logic.ViewModels.Tests
{
    public class LoginViewModelTests
    {
        private Mock<IApiConsumer> mockedApiConsumer;
        private Mock<IAuthAccess> mockedAuthAccess;
        private LoginViewModel viewModel;

        private void Init()
        {
            mockedApiConsumer = new Mock<IApiConsumer>();
            mockedAuthAccess = new Mock<IAuthAccess>();
            viewModel = new LoginViewModel(mockedApiConsumer.Object, mockedAuthAccess.Object);
        }

        [Fact]
        public async Task AttemptLogin_NoUsernameOrPassword_ShowsValidationExceptions()
        {
            // ARRANGE
            Init();

            // ACT
            await viewModel.AttemptLoginAsync();

            // ASSERT

            // ensure no server call is made
            mockedApiConsumer.Verify(c => c.LoginAsync(It.IsAny<LoginModel>(),
                                                       It.IsAny<CancellationToken>()),
                                     Times.Never);

            mockedAuthAccess.Verify(a => a.SetStoredTokenAsync(It.IsAny<TokenResponseModel>()),
                                    Times.Never);

            bool isValidUsername = viewModel.LoginValidation.IsValidProperty(nameof(LoginModel.UserName));
            bool isValidPassword = viewModel.LoginValidation.IsValidProperty(nameof(LoginModel.Password));

            Assert.False(isValidUsername);
            Assert.False(isValidPassword);
        }


        [Fact]
        public async Task AttemptLogin_NoPassword_ShowsValidationExceptions()
        {
            // ARRANGE
            Init();
            viewModel.LoginModel.UserName = "neville";

            // ACT
            await viewModel.AttemptLoginAsync();

            // ASSERT

            // ensure no server call is made
            mockedApiConsumer.Verify(c => c.LoginAsync(It.IsAny<LoginModel>(),
                                                       It.IsAny<CancellationToken>()),
                                     Times.Never);

            mockedAuthAccess.Verify(a => a.SetStoredTokenAsync(It.IsAny<TokenResponseModel>()),
                                    Times.Never);

            bool isValidUsername = viewModel.LoginValidation.IsValidProperty(nameof(LoginModel.UserName));
            bool isValidPassword = viewModel.LoginValidation.IsValidProperty(nameof(LoginModel.Password));

            Assert.True(isValidUsername);
            Assert.False(isValidPassword);
        }

        [Fact]
        public async Task AttemptLogin_NoUsername_ShowsValidationExceptions()
        {
            // ARRANGE
            Init();
            viewModel.LoginModel.Password = "verysecret";

            // ACT
            await viewModel.AttemptLoginAsync();

            // ASSERT

            // ensure no server call is made
            mockedApiConsumer.Verify(c => c.LoginAsync(It.IsAny<LoginModel>(),
                                                       It.IsAny<CancellationToken>()),
                                     Times.Never);

            mockedAuthAccess.Verify(a => a.SetStoredTokenAsync(It.IsAny<TokenResponseModel>()),
                                    Times.Never);

            bool isValidUsername = viewModel.LoginValidation.IsValidProperty(nameof(LoginModel.UserName));
            bool isValidPassword = viewModel.LoginValidation.IsValidProperty(nameof(LoginModel.Password));

            Assert.False(isValidUsername);
            Assert.True(isValidPassword);
        }

    }
}
