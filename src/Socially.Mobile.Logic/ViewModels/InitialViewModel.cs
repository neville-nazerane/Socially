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
        private readonly ICachedContext _cachedContext;
        private readonly IAuthAccess _authAccess;

        public InitialViewModel(INavigationControl navigationControl,
                                ICachedContext cachedContext,
                                IAuthAccess authAccess)
        {
            _navigationControl = navigationControl;
            _cachedContext = cachedContext;
            _authAccess = authAccess;
        }

        public override async Task OnNavigatedAsync()
        {
            if ((await _authAccess.GetStoredTokenAsync()) is null)
                await _navigationControl.GoToLoginAsync();
            else
            {
                await _cachedContext.GetCurrentUserAsync();
                await _navigationControl.GoToHomeAsync();
            }
        }

    }
}
