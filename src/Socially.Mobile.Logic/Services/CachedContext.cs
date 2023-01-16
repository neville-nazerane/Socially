using Socially.Apps.Consumer.Services;
using Socially.Mobile.Logic.Models;
using Socially.Mobile.Logic.Models.Mappings;
using Socially.Website.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Mobile.Logic.Services
{
    public class CachedContext : ICachedContext
    {
        private readonly IApiConsumer _apiConsumer;
        private readonly ICachedNoSqlStorage<int, UserSummaryModel> _userStorage;

        private int currentUserId;

        public CachedContext(IApiConsumer apiConsumer,
                             ICachedNoSqlStorage<int, UserSummaryModel> userStorage)
        {
            _apiConsumer = apiConsumer;
            _userStorage = userStorage;
        }

        public async Task<UserSummaryModel> GetCurrentUserAsync()
        {
            if (currentUserId == 0)
            {
                var user = await _apiConsumer.GetCurrentUserSummary().ToMobileModel();
                await _userStorage.UpdateAsync(user);
                currentUserId = user.Id;
            }
            
            return GetUser(currentUserId);
        }

        public UserSummaryModel GetCurrentUser() => GetUser(currentUserId);

        public async Task UpdateUserProfilesIfNotExistAsync(IEnumerable<int> ids)
        {
            await _userStorage.AwaitLockAsync();
            var missingIds = ids.Where(id => !_userStorage.IsInitialized(id));
            if (missingIds.Any())
                await ForceUpdateUserProfilesAsync(missingIds);
        }

        public UserSummaryModel GetUser(int id) => _userStorage.Get(id);

        public async Task ForceUpdateUserProfilesAsync(IEnumerable<int> ids)
        {
            var users = await _apiConsumer.GetUsersByIdsAsync(ids);
            await _userStorage.UpdateAsync(users.ToMobileModel().ToArray());
        }

        public Task ClearRAMAsync() => _userStorage.ClearAllInRamAsync();

        public Task ClearDbAsync() => _userStorage.ClearDbAsync();

    }
}
