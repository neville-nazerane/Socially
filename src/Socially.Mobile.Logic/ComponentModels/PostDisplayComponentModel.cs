using CommunityToolkit.Mvvm.ComponentModel;
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
    public partial class PostDisplayComponentModel : ViewModelBase
    {
        private readonly ICachedContext _cachedContext;
        [ObservableProperty]
        PostDisplayModel post;

        [ObservableProperty]
        UserSummaryModel user;

        public PostDisplayComponentModel(ICachedContext cachedContext)
        {
            _cachedContext = cachedContext;
        }

        partial void OnPostChanging(PostDisplayModel value)
        {
            User = _cachedContext.GetUser(value.CreatorId);
        }

    }
}
