using CommunityToolkit.Mvvm.ComponentModel;
using Socially.Apps.Consumer.Services;
using Socially.Mobile.Logic.Models;
using Socially.Mobile.Logic.Services;
using Socially.Mobile.Logic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Mobile.Logic.ComponentModels
{
    public partial class ProfileHeaderComponentModel : ViewModelBase
    {
        private readonly ICachedContext _cachedContext;

        [ObservableProperty]
        private UserSummaryModel user;

        public ProfileHeaderComponentModel(ICachedContext cachedContext)
        {
            _cachedContext = cachedContext;
        }

        public async Task RefreshUserAsync()
        {
            User = await _cachedContext.GetCurrentUserAsync();
        }

    }
}
