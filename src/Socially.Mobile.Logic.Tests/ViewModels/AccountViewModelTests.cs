using Socially.Apps.Consumer.Services;
using Socially.Mobile.Logic.Services;
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

    }
}
