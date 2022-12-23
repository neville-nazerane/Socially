using Socially.Apps.Consumer.Services;
using Socially.Mobile.Logic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Mobile.Logic.ViewModels
{
    public class InitialViewModel : ViewModelBase
    {
        private readonly INavigationControl _navigationControl;
        private readonly IAuthAccess _authAccess;

        public InitialViewModel(INavigationControl navigationControl,
                                IAuthAccess authAccess)
        {
            _navigationControl = navigationControl;
            _authAccess = authAccess;
        }

        public override async Task OnNavigatedAsync()
        {
            if ((await _authAccess.GetStoredTokenAsync()) is null)
                await _navigationControl.GoToLoginPageAsync();
            else await _navigationControl.GoToHomeAsync();
        }

    }
}
