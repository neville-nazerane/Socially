﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Socially.Apps.Consumer.Services;
using Socially.Mobile.Logic.Models;
using Socially.Mobile.Logic.Models.Mappings;
using Socially.Mobile.Logic.Models.PubMessages;
using Socially.Mobile.Logic.Services;
using Socially.Mobile.Logic.Utils;
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
        private readonly IPubSubService _pubSubService;
        private readonly ICachedContext _cachedContext;

        [ObservableProperty]
        bool isSelected;

        [ObservableProperty]
        AddPostModel addPostModel;

        [RelayCommand]
        void Swap() => IsSelected = !IsSelected;

        public HomeViewModel(ISocialLogger logger,
                             IApiConsumer apiConsumer,
                             IPubSubService pubSubService,
                             ICachedContext cachedContext)
        {
            _logger = logger;
            _apiConsumer = apiConsumer;
            _pubSubService = pubSubService;
            _cachedContext = cachedContext;

            AddPostModel = new();
        }

        public override void OnException(Exception ex) => _logger.LogException(ex);

        public override async Task OnNavigatedAsync()
        {
            await RefreshAsync();
            _pubSubService.Subscribe<RefreshPostMessage>(_id, m => Task.Run(RefreshAsync));
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
            if (AddPostModel.Validate(Validation))
            {
                await ExecuteAndValidate(() => _apiConsumer.AddPostAsync(AddPostModel.ToModel()));
                await RefreshAsync();
                AddPostModel = new();
            }
        }

        public override async Task<ObservableCollection<PostDisplayModel>> GetFromServerAsync(CancellationToken cancellationToken = default)
            => new((await _apiConsumer.GetHomePostsAsync(10, null, cancellationToken).ToMobileModel())
                                      .ReverseRecursive());

    }
}
