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
        private readonly INavigation _navigation;
        private readonly ISocialLogger _socialLogger;

        //[ObservableProperty]
        //LoginModel loginModel;

        public LoginViewModel(IApiConsumer apiConsumer, 
                              IAuthAccess authAccess,
                              INavigation navigation,
                              ISocialLogger socialLogger)
        {
            //loginModel = new();
            _apiConsumer = apiConsumer;
            _authAccess = authAccess;
            _navigation = navigation;
            _socialLogger = socialLogger;
        }

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

    }
}
