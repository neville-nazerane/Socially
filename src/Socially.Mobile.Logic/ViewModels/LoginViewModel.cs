using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Socially.Apps.Consumer.Exceptions;
using Socially.Apps.Consumer.Services;
using Socially.Mobile.Logic.Models;
using Socially.Mobile.Logic.Models.Mappings;
using Socially.Mobile.Logic.Services;
using Socially.Mobile.Logic.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Mobile.Logic.ViewModels
{

    public partial class LoginViewModel : ViewModelBase<LoginModel>
    {
        private readonly IApiConsumer _apiConsumer;
        private readonly IAuthAccess _authAccess;
        private readonly INavigationControl _navigation;
        private readonly IMessaging _messaging;
        private readonly ISocialLogger _socialLogger;

        public LoginViewModel(IApiConsumer apiConsumer, 
                              IAuthAccess authAccess,
                              INavigationControl navigation,
                              IMessaging messaging,
                              ISocialLogger socialLogger)
        {
            _apiConsumer = apiConsumer;
            _authAccess = authAccess;
            _navigation = navigation;
            _messaging = messaging;
            _socialLogger = socialLogger;
            Model.Source = "mobile";
        }

        [RelayCommand]
        Task GoToSignupAsync() => _navigation.GoToSignupAsync();

        [RelayCommand]
        Task GoToForgotPasswordAsync() => _navigation.GoToForgotPasswordAsync();

        public override string ErrorOnException => "Failed to login";

        public override string ErrorWhenBadRequestEmpty => "Failed to login";

        public override void OnException(Exception ex) => _socialLogger.LogException(ex);

        public override async Task SubmitToServerAsync(LoginModel model, CancellationToken cancellationToken = default)
        {
            // exceptions for the below should be caught by base submit async function
            var res = await _apiConsumer.LoginAsync(Model.ToModel(), cancellationToken);
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

        public override Task OnValidationChangedAsync()
        {
            var error = Validation?.FirstOrDefault(v => !string.IsNullOrEmpty(v.ErrorMessage));
            if (error is not null)
            {
                string field = error.MemberNames.FirstOrDefault();
                return _messaging.DisplayAsync(field, error.ErrorMessage, "OK");
            }
            return Task.CompletedTask;
        }

    }
}
