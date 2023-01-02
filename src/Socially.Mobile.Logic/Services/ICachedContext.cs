﻿using Socially.Mobile.Logic.Models;

namespace Socially.Mobile.Logic.Services
{
    public interface ICachedContext
    {
        Task ClearDbAsync();
        Task ClearRAMAsync();
        Task ForceUpdateUserProfilesAsync(IEnumerable<int> ids);
        Task<UserSummaryModel> GetCurrentUserAsync();
        UserSummaryModel GetUser(int id);
        Task UpdateUserProfilesIfNotExistAsync(IEnumerable<int> ids);
    }
}