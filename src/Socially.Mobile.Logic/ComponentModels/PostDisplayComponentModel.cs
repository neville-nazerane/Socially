using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Socially.Apps.Consumer.Services;
using Socially.Mobile.Logic.Models;
using Socially.Mobile.Logic.Services;
using Socially.Mobile.Logic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Socially.Mobile.Logic.ComponentModels
{
    public partial class PostDisplayComponentModel : ViewModelBase
    {
        private readonly ICachedContext _cachedContext;
        private readonly IApiConsumer _apiConsumer;
        
        [ObservableProperty]
        PostDisplayModel post;

        [ObservableProperty]
        UserSummaryModel user;

        public PostDisplayComponentModel(ICachedContext cachedContext,
                                         IApiConsumer apiConsumer)
        {
            _cachedContext = cachedContext;
            _apiConsumer = apiConsumer;
        } 

        partial void OnPostChanging(PostDisplayModel value)
        {
            User = _cachedContext.GetUser(value.CreatorId);
        }

        [RelayCommand]
        public async Task AddCommentAsync(string text, CancellationToken cancellationToken = default)
        {
            await _apiConsumer.AddCommentAsync(new()
            {
                Text = text,
                ParentCommentId = null,
                PostId = Post.Id
            }, cancellationToken);
        }

    }
}
