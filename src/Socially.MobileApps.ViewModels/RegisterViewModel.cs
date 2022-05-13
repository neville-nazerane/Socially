using Socially.MobileApps.Contracts;
using Socially.MobileApps.Models;
using Socially.MobileApps.Services.Exceptions;
using Socially.MobileApps.Services.HttpServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Socially.MobileApps.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        private readonly IApiConsumer _apiConsumer;

        public SignUpModel Model { get; set; }

        public ICommand SubmitCmd { get; set; }

        public RegisterViewModel(IThemeControl themeControl, IApiConsumer apiConsumer) : base(themeControl)
        {
            Model = new SignUpModel();
            _apiConsumer = apiConsumer;

            SubmitCmd = BuildCommand(SubmitAsync);
        }

        public async Task SubmitAsync()
        {
            try
            {
                var res = await _apiConsumer.SignUpAsync(Model);
            }
            catch (ErrorForClientException ex)
            {
                throw;
            }
        }



    }
}
