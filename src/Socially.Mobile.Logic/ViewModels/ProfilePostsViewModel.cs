using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Socially.Apps.Consumer.Services;
using Socially.Mobile.Logic.Models;
using Socially.Mobile.Logic.Models.Mappings;
using Socially.Mobile.Logic.Services;
using Socially.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Mobile.Logic.ViewModels
{
    public partial class ProfilePostsViewModel : ViewModelBase<ObservableCollection<PostDisplayModel>>
    {
        private readonly ISocialLogger _logger;
        private readonly IApiConsumer _apiConsumer;
        private readonly ICachedContext _cachedContext;

        [ObservableProperty]
        AddPostModel addPostModel;

        public ProfilePostsViewModel(ISocialLogger logger,
                                     IApiConsumer apiConsumer,
                                     ICachedContext cachedContext)
        {
            AddPostModel = new();
            _logger = logger;
            _apiConsumer = apiConsumer;
            _cachedContext = cachedContext;
        }

        public override void OnException(Exception ex) => _logger.LogException(ex);

        public override async Task OnNavigatedAsync()
        {
            await RefreshAsync();
        }

        [RelayCommand]
        private async Task RefreshAsync()
        {
            await GetAsync();
            var userIds = Model.SelectMany(p => p.Comments.Select(c => c.CreatorId))
                               .Union(Model.Select(p => p.CreatorId))
                               .ToImmutableArray();
            await _cachedContext.UpdateUserProfilesIfNotExistAsync(userIds);
        }
        
        public override async Task<ObservableCollection<PostDisplayModel>> GetFromServerAsync(CancellationToken cancellationToken = default)
        {
            var res = new ObservableCollection<PostDisplayModel>(
                                (await _apiConsumer.GetCurrentUserPostsAsync(20, null, cancellationToken).ToMobileModel())
                                .Reverse());
            
            foreach (var post in res)
            {
                post.Comments = ReverseComments(post.Comments);
            }

            return res;
        }

        ICollection<DisplayCommentModel> ReverseComments(IEnumerable<DisplayCommentModel> commentModels)
        {
            if (commentModels is null)
                return null;

            var result = commentModels.Reverse().ToList();

            foreach (var comment in commentModels)
                comment.Comments = ReverseComments(comment.Comments);

            return result;
        }

        [RelayCommand]
        public async Task AddPostAsync()
        {
            if (AddPostModel.Validate(Validation))
            {
                await ExecuteAndValidate(() => _apiConsumer.AddPostAsync(AddPostModel.ToModel()));
                await RefreshAsync();
                AddPostModel = new();
            }
        }

    }
}
