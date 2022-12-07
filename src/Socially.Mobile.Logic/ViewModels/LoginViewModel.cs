using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Socially.Apps.Consumer.Services;
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

        [ObservableProperty]
        LoginModel loginModel;

        [ObservableProperty]
        ObservableCollection<ValidationResult> loginValidation;

        public LoginViewModel(IApiConsumer apiConsumer, IAuthAccess authAccess)
        {
            loginModel = new();
            loginValidation = new();
            _apiConsumer = apiConsumer;
            _authAccess = authAccess;
        }

        [RelayCommand]
        public async Task AttemptLoginAsync()
        {
            if (loginModel.Validate(loginValidation))
            {
                var res = await _apiConsumer.LoginAsync(loginModel.ToModel());
                await _authAccess.SetStoredTokenAsync(res);
            }
        }

    }
}
