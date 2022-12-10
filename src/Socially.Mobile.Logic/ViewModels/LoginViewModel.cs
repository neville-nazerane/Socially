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

    public partial class LoginViewModel : ViewModelBase
    {
        private readonly IApiConsumer _apiConsumer;
        private readonly IAuthAccess _authAccess;
        private readonly INavigation _navigation;
        private readonly ISocialLogger _socialLogger;
        [ObservableProperty]
        string errorMessage;

        [ObservableProperty]
        LoginModel loginModel;

        [ObservableProperty]
        ObservableCollection<ValidationResult> loginValidation;

        public LoginViewModel(IApiConsumer apiConsumer, 
                              IAuthAccess authAccess,
                              INavigation navigation,
                              ISocialLogger socialLogger)
        {
            loginModel = new();
            loginValidation = new();
            _apiConsumer = apiConsumer;
            _authAccess = authAccess;
            _navigation = navigation;
            _socialLogger = socialLogger;
        }

        [RelayCommand]
        public async Task AttemptLoginAsync()
        {
            if (loginModel.Validate(loginValidation))
            {
                try
                {
                    var res = await _apiConsumer.LoginAsync(loginModel.ToModel());
                    try
                    {
                        await _authAccess.SetStoredTokenAsync(res);
                        await _navigation.GoToHomeAsync();
                    }
                    catch (Exception ex)
                    {
                        _socialLogger.LogException(ex);
                        ErrorMessage = "Failed to store login information";
                    }
                }
                catch (ErrorForClientException clientException)
                {
                    LoginValidation = clientException.ToObservableCollection();
                }
                catch (Exception ex)
                {
                    _socialLogger.LogException(ex);
                    ErrorMessage = "Failed to login";
                }

            }
        }

    }
}
