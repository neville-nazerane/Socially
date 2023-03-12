using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Socially.Apps.Consumer.Services;
using Socially.Mobile.Logic.Models;
using Socially.Mobile.Logic.Models.PubMessages;
using Socially.Mobile.Logic.Services;
using Socially.Mobile.Logic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Mobile.Logic.ComponentModels
{
    public partial class CommentDisplayComponentModel : ViewModelBase
    {

        private readonly ICachedContext _cachedContext;
        private readonly IApiConsumer _apiConsumer;
        private readonly IPubSubService _pubSubService;
        [ObservableProperty]
        DisplayCommentModel comment;

        [ObservableProperty]
        UserSummaryModel user;

        public int PostId { get; set; }

        public CommentDisplayComponentModel(ICachedContext cachedContext, 
                                            IApiConsumer apiConsumer,
                                            IPubSubService pubSubService)
        {
            _cachedContext = cachedContext;
            _apiConsumer = apiConsumer;
            _pubSubService = pubSubService;
        }

        partial void OnCommentChanging(DisplayCommentModel value)
        {
            User = _cachedContext.GetUser(value.CreatorId);
        }


        [RelayCommand]
        public async Task AddCommentAsync(string text, CancellationToken cancellationToken = default)
        {
            await _apiConsumer.AddCommentAsync(new()
                                            {
                                                Text = text,
                                                ParentCommentId = Comment.Id,
                                                PostId = PostId
                                            }, cancellationToken);
            _pubSubService.Publish(new RefreshPostMessage
            {
                PostId = PostId
            });
        }

    }
}
