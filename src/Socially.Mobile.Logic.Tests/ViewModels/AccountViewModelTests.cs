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
    public class AccountViewModelTests
    {
        private Mock<IApiConsumer> mockedApiConsumed;
        private Mock<ISocialLogger> mockedLogger;
        private AccountViewModel viewModel;

        void Init()
        {
            mockedApiConsumed = new Mock<IApiConsumer>();
            mockedLogger = new Mock<ISocialLogger>();

            viewModel = new(mockedApiConsumed.Object,
                            mockedLogger.Object);
        }

        [Fact]
        public async Task Submit_InvalidInput_ShowsErrors()
        {
            // ARRANGE
            Init();
            viewModel.Model.FirstName = new string(Enumerable.Repeat('a', 200).ToArray());

            // ACT
            await viewModel.SubmitAsync();

            // ASSERT
            Assert.Null(viewModel.ErrorMessage);
            Assert.NotEmpty(viewModel.Validation);
        }

        [Fact]
        public async Task Submit_ValidInput_Proceeds()
        {
            // ARRANGE
            Init();

            // ACT
            await viewModel.SubmitAsync();

            // ASSERT
            Assert.Null(viewModel.ErrorMessage);
            Assert.Empty(viewModel.Validation);

        }

        [Fact]
        public async Task Submit_BadRrequest_ShowsError()
        {
            // ARRANGE
            Init();

            var exception = new ErrorForClientException(new[]
            {
                new ErrorModel(nameof(ProfileUpdateModel.LastName), "Not a nice last name")
            });
            mockedApiConsumed.Setup(c => c.UpdateProfileAsync(It.IsAny<ProfileUpdateModel>(),
                                                              It.IsAny<CancellationToken>()))
                              .Throws(exception);

            // ACT
            await viewModel.SubmitAsync();

            // ASSERT
            Assert.Null(viewModel.ErrorMessage);
            Assert.NotEmpty(viewModel.Validation);
        }

        [Fact]
        public async Task Submit_ApiThrows_ShowsError()
        {
            // ARRANGE
            Init();
            var exception = new Exception("Don't show this");
            mockedApiConsumed.Setup(c => c.UpdateProfileAsync(It.IsAny<ProfileUpdateModel>(),
                                                  It.IsAny<CancellationToken>()))
                  .Throws(exception);

            // ACT
            await viewModel.SubmitAsync();

            // ASSERT
            Assert.NotNull(viewModel.ErrorMessage);
            Assert.Empty(viewModel.Validation);

        }

        [Fact]
        public async Task Get_NoErrors_ShowsNoErrors()
        {
            // ARRANGE
            Init();
            var result = new ProfileUpdateModel
            {
                FirstName = "Somename"
            };
            mockedApiConsumed.Setup(c => c.GetUpdateProfileAsync(It.IsAny<CancellationToken>()))
                             .ReturnsAsync(result);

            // ACT
            await viewModel.GetAsync();

            // ASSERT
            Assert.Null(viewModel.ErrorMessage);
            Assert.Empty(viewModel.Validation);

            Assert.Equal("Somename", viewModel.Model.FirstName); 
        }

        [Fact]
        public async Task Get_ApiThrowsException_ShowsError()
        {
            // ARRANGE
            Init();
            
            mockedApiConsumed.Setup(c => c.GetUpdateProfileAsync(It.IsAny<CancellationToken>()))
                             .Throws(new Exception());

            // ACT
            await viewModel.GetAsync();

            // ASSERT
            Assert.NotNull(viewModel.ErrorMessage);
            Assert.Empty(viewModel.Validation);
        }

    }
}
