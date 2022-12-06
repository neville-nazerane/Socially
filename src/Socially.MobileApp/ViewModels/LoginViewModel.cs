using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Socially.Apps.Consumer.Services;
using Socially.MobileApp.Models;
using Socially.MobileApp.Models.Mappings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.MobileApp.ViewModels
{

    public partial class LoginViewModel : ViewModelBase
    {
        private readonly IApiConsumer _apiConsumer;
        private readonly IAuthAccess _authAccess;

        [ObservableProperty]
        LoginModel loginModel;

        public LoginViewModel(IApiConsumer apiConsumer, IAuthAccess authAccess)
        {
            _apiConsumer = apiConsumer;
            _authAccess = authAccess;
        }

        [RelayCommand]
        async Task AttemptLogin()
        {
            var res = await _apiConsumer.LoginAsync(loginModel.ToModel());
            await _authAccess.SetStoredTokenAsync(res);

            
        }

    }
}
