using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Socially.Apps.Consumer.Services;
using Socially.Mobile.Logic.Models;
using Socially.Mobile.Logic.Models.Mappings;
using Socially.Mobile.Logic.Services;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Mobile.Logic.ViewModels
{
    public partial class HomeViewModel : ViewModelBase<ObservableCollection<PostDisplayModel>>
    {

        private readonly ISocialLogger _logger;
        private readonly IApiConsumer _apiConsumer;
        private readonly ICachedContext _cachedContext;

        [ObservableProperty]
        bool isSelected;

        [ObservableProperty]
        string newPostText;

        [RelayCommand]
        void Swap() => IsSelected = !IsSelected;

        public HomeViewModel(ISocialLogger logger, IApiConsumer apiConsumer, ICachedContext cachedContext)
        {
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
            var userids = Model.SelectMany(c => c.Comments.Select(c => c.CreatorId))
                               .Union(Model.Select(p => p.CreatorId))
                               .ToImmutableArray();
            await _cachedContext.UpdateUserProfilesIfNotExistAsync(userids);
        }

        [RelayCommand]
        async Task AddPostAsync()
        {
            if (!string.IsNullOrEmpty(NewPostText))
            {
                await _apiConsumer.AddPostAsync(new Socially.Models.AddPostModel
                {
                    Text = NewPostText
                });
                await RefreshAsync();
                NewPostText = null;
            }
        }

        public override async Task<ObservableCollection<PostDisplayModel>> GetFromServerAsync(CancellationToken cancellationToken = default)
            => new((await _apiConsumer.GetHomePostsAsync(10, null, cancellationToken).ToMobileModel())
                    .Reverse());

    }
}
