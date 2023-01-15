﻿using CommunityToolkit.Mvvm.Input;
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
    public partial class ProfilePostsViewModel : ViewModelBase<ObservableCollection<PostDisplayModel>>
    {
        private readonly ISocialLogger _logger;
        private readonly IApiConsumer _apiConsumer;
        private readonly ICachedContext _cachedContext;

        public ProfilePostsViewModel(ISocialLogger logger,
                                     IApiConsumer apiConsumer,
                                     ICachedContext cachedContext)
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

            return res;
        }



    }
}
