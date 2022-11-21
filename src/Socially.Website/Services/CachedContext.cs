using Socially.Apps.Consumer.Services;
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
        private readonly AuthProvider _authProvider;
        UserSummaryModel _currentProfileInfo;
        TaskCompletionSource _currentProfileLock;

        private CachedUserMappings _userSummaries;
        private TaskCompletionSource _userSummariesLock;

        public CachedContext(IApiConsumer consumer, AuthProvider authProvider)
        {
            _userSummaries = new();
            _authProvider = authProvider;
            
            _authProvider.AuthenticationStateChanged += AuthProvider_AuthenticationStateChanged;
            _consumer = consumer;
        }

        private void AuthProvider_AuthenticationStateChanged(Task<Microsoft.AspNetCore.Components.Authorization.AuthenticationState> task)
        {
            _currentProfileInfo = null;
        }

        public async ValueTask<UserSummaryModel> GetCurrentProfileInfoAsync()
        {
            if (_currentProfileInfo is null)
            {
                if (_currentProfileLock is not null)
                    await _currentProfileLock.Task;
                if (_currentProfileInfo is null)
                {
                    _currentProfileLock = new TaskCompletionSource();
                    try
                    {
                        _currentProfileInfo = await _consumer.GetCurrentUserSummary();
                        _userSummaries.Update(_currentProfileInfo.ToSingleItemArray());
                        _currentProfileLock.TrySetResult();
                    }
                    catch (Exception ex)
                    {
                        _currentProfileLock?.TrySetException(ex);
                    }
                    finally
                    {
                        _currentProfileLock = null;
                    }
                }
            }
            return _currentProfileInfo;
        }

        public async ValueTask UpdateUserProfilesIfNotExistAsync(IEnumerable<int> ids)
        {
            var missingIds = ids.Where(id => !_userSummaries.IsInitialized(id));
            if (missingIds.Any())
                await ForceUpdateUserProfilesAsync(missingIds);
        }

        public UserSummaryModel GetUser(int id) => _userSummaries.Get(id);

        public async Task ForceUpdateUserProfilesAsync(IEnumerable<int> missingIds)
        {
            if (_userSummariesLock is not null)
                await _userSummariesLock.Task;
            _userSummariesLock = new();
            try
            {
                var users = await _consumer.GetUsersByIdsAsync(missingIds);
                _userSummaries.Update(users);
            }
            finally
            {
                _userSummariesLock?.SetResult();
                _userSummariesLock = null;
            }
        }
    

    }
}
