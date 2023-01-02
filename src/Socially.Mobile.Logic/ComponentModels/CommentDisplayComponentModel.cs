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
    public partial class CommentDisplayComponentModel : ViewModelBase
    {

        private readonly ICachedContext _cachedContext;

        [ObservableProperty]
        DisplayCommentModel comment;

        [ObservableProperty]
        UserSummaryModel user;

        public CommentDisplayComponentModel(ICachedContext cachedContext)
        {
            _cachedContext = cachedContext;
        }

        partial void OnCommentChanging(DisplayCommentModel value)
        {
            User = _cachedContext.GetUser(value.CreatorId);
        }

    }
}
