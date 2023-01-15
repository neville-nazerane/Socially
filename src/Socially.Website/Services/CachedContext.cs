using Microsoft.AspNetCore.Components.Authorization;
using Socially.Apps.Consumer.Services;
using Socially.ClientUtils;
using Socially.Models;
using Socially.Website.Models;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Socially.Website.Services
{
    public class CachedContext
    {
        private readonly IApiConsumer _consumer;
        private readonly ICachedStorage<int, UserSummaryModel> _userStorage;
        private readonly AuthenticationStateProvider _authProvider;
        UserSummaryModel _currentProfileInfo;

        readonly AsyncLocker _currentProfileLock;
        readonly AsyncLocker _updatesLock;


        public CachedContext(IApiConsumer consumer,
                             ICachedStorage<int, UserSummaryModel> userStorage,
                             AuthenticationStateProvider authProvider)
        {
            _updatesLock = new();
            _currentProfileLock = new();
            _authProvider = authProvider;
            _authProvider.AuthenticationStateChanged += AuthProvider_AuthenticationStateChanged;
            _consumer = consumer;
            _userStorage = userStorage;
        }

        private void AuthProvider_AuthenticationStateChanged(Task<AuthenticationState> task)
        {
            _currentProfileInfo = null;
        }

        public async ValueTask<UserSummaryModel> GetCurrentProfileInfoAsync()
        {
            var state = await _authProvider.GetAuthenticationStateAsync();
            if (state?.User?.Claims?.Count() < 1) return null;
            using var _ = await _currentProfileLock.WaitAndBeginLockAsync();
            if (_currentProfileInfo is null)
            {
                _currentProfileInfo = await _consumer.GetCurrentUserSummary();
                await _userStorage.UpdateAsync(_currentProfileInfo.ToSingleItemArray());
            }
            return _currentProfileInfo;
        }

        public async Task UpdateUserProfilesIfNotExistAsync(IEnumerable<int> ids)
        {
            using var _ = await _updatesLock.WaitAndBeginLockAsync();

            var missingIds = ids.Where(id => !_userStorage.IsInitialized(id));
            if (missingIds.Any())
            {
                await ForceUpdateUserProfilesAsync(missingIds);
            }
        }

        public UserSummaryModel GetUser(int id) => _userStorage.Get(id);

        public async Task ForceUpdateUserProfilesAsync(IEnumerable<int> ids)
        {
            var users = await _consumer.GetUsersByIdsAsync(ids);
            await _userStorage.UpdateAsync(users);
        }

    }
}
