using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Socially.Apps.Consumer.Exceptions;
using Socially.Apps.Consumer.Services;
using Socially.Mobile.Logic.Services;
using Socially.Mobile.Logic.Utils;
using Socially.MobileApp.Logic.Models;
using Socially.MobileApp.Logic.Models.Mappings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Mobile.Logic.ViewModels
{
    public partial class SignupViewModel : ViewModelBase
    {
        private readonly INavigation _navigation;
        private readonly IMessaging _messaging;
        private readonly IApiConsumer _apiConsumer;
        private readonly ISocialLogger _logger;

        [ObservableProperty]
        private SignUpModel model;

        public SignupViewModel(INavigation navigation,
                               IMessaging messaging,
                               IApiConsumer apiConsumer,
                               ISocialLogger logger)
        {
            model = new();
            
            _navigation = navigation;
            _messaging = messaging;
            _apiConsumer = apiConsumer;
            _logger = logger;
        }

        [RelayCommand]
        public async Task SubmitAsync()
        {
            if (model.Validate(Validation))
            {
                try
                {
                    await _apiConsumer.SignupAsync(model.ToModel());
                    await _messaging.DisplayAsync("Done!", "Your Account has been created", "Go to Login");
                    await _navigation.GoToLoginPageAsync();
                }
                catch (ErrorForClientException clientException)
                {
                    Validation = clientException.ToObservableCollection();
                }
                catch (Exception ex) 
                {
                    _logger.LogException(ex);
                    ErrorMessage = "Failed to signup. Please try again";
                }
            }
        }

    }
}
