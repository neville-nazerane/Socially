using Socially.Models;
using Socially.Server.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Socially.Server.Managers
{
    public interface IUserProfileManager
    {
        Task<string> CreateRefreshTokenAsync(int userId, TimeSpan expireIn, CancellationToken cancellationToken = default);
        Task DisableRefreshTokenAsync(int userId, string refreshToken, CancellationToken cancellationToken = default);
        Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);
        Task<ProfileSummary> GetSummaryAsync(int userId, CancellationToken cancellationToken = default);
        Task<ProfileUpdateModel> GetUpdatableProfileAsync(int userId, CancellationToken cancellationToken = default);
        ValueTask<User> GetUserByIdAsync(int userId, CancellationToken cancellationToken = default);
        Task<User> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task UpdateAsync(int userId, ProfileUpdateModel model, CancellationToken cancellationToken = default);
        Task<bool> UserNameExistsAsync(string userName, CancellationToken cancellationToken = default);
        Task<bool> VerifyRefreshToken(int userId, string refreshToken, CancellationToken cancellationToken = default);
        Task<User> GetUserByUsernameAsync(string username, CancellationToken cancellationToken = default);
        Task<IEnumerable<SearchedUserModel>> SearchAsync(int userId, string q, CancellationToken cancellationToken = default);
        Task<IEnumerable<UserSummaryModel>> GetUsersByIdsAsync(IEnumerable<int> userIds, CancellationToken cancellationToken = default);
    }
}
