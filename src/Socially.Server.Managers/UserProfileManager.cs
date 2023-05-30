using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Socially.Models;
using Socially.Models.Enums;
using Socially.Server.DataAccess;
using Socially.Server.Entities;
using Socially.Server.ModelMappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Socially.Server.Managers
{
    public class UserProfileManager : IUserProfileManager
    {
        private readonly ApplicationDbContext _dbContext;

        public UserProfileManager(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ValueTask<User> GetUserByIdAsync(int userId, CancellationToken cancellationToken = default)
            => _dbContext.Users.FindAsync(new object[] { userId }, cancellationToken);

        public Task<User> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
            => _dbContext.Users
                         .Where(u => u.Email == email)
                         .SingleOrDefaultAsync(cancellationToken);

        public Task<User> GetUserByUsernameAsync(string username, CancellationToken cancellationToken = default)
            => _dbContext.Users
                          .Where(u => u.UserName == username)
                          .SingleOrDefaultAsync(cancellationToken);

        public Task<ProfileSummary> GetSummaryAsync(int userId, CancellationToken cancellationToken = default)
            => _dbContext.Users
                         .Where(u => u.Id == userId)
                         .Select(u => new ProfileSummary
                         {
                             Id = u.Id,
                             UserName = u.UserName,
                             FullName = $"{u.FirstName} {u.LastName}"
                         })
                         .SingleOrDefaultAsync(cancellationToken);

        public async Task UpdateAsync(int userId, ProfileUpdateModel model, CancellationToken cancellationToken = default)
        {
            if (model is null) throw new ArgumentNullException(nameof(model));
            var profile = await _dbContext.Users.FindAsync(new object[]{ userId }, cancellationToken: cancellationToken);
            if (profile is null) throw new NullReferenceException(nameof(profile));
            profile.FirstName = model.FirstName;
            profile.LastName = model.LastName;
            profile.DateOfBirth = model.DateOfBirth;

            int? picId = await _dbContext.ProfileImages
                                                .Where(p => p.FileName == model.ProfilePictureFileName && p.UserId == userId)
                                                .Select(p => (int?) p.Id)
                                                .SingleOrDefaultAsync(cancellationToken);
            profile.ProfilePictureId = picId;

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public Task<ProfileUpdateModel> GetUpdatableProfileAsync(int userId, CancellationToken cancellationToken = default)
            => _dbContext.Users
                            .Where(u => u.Id == userId)
                            .Select(u => new ProfileUpdateModel
                            {
                                FirstName = u.FirstName,
                                LastName = u.LastName,
                                DateOfBirth = u.DateOfBirth,
                                ProfilePictureFileName = u.ProfilePicture == null ? null : u.ProfilePicture.FileName
                            })
                            .SingleOrDefaultAsync(cancellationToken);

        public async Task<IEnumerable<UserSummaryModel>> GetUsersByIdsAsync(IEnumerable<int> userIds, CancellationToken cancellationToken = default)
            => await _dbContext.Users
                               .Where(u => userIds.Contains(u.Id))
                               .SelectAsSummaryModel()
                               .ToListAsync(cancellationToken);

        public Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default) 
            => _dbContext.Users.AnyAsync(u => u.Email == email, cancellationToken);

        public Task<bool> UserNameExistsAsync(string userName, CancellationToken cancellationToken = default) 
            => _dbContext.Users.AnyAsync(u => u.UserName == userName, cancellationToken);

        public async Task<string> CreateRefreshTokenAsync(int userId, 
                                                    TimeSpan expireOn,
                                                    CancellationToken cancellationToken = default)
        {
            UserRefreshToken entity = new()
            {
                ExpiresOn = DateTime.UtcNow.Add(expireOn),
                UserId = userId,
                RefreshToken = GenerateRefreshToken(),
                IsEnabled = true
            };
            await _dbContext.UserRefreshTokens.AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return entity.RefreshToken;
        }

        public Task<bool> VerifyRefreshTokenAsync(int userId,
                                             string refreshToken,
                                             CancellationToken cancellationToken = default)
            => _dbContext.UserRefreshTokens.AnyAsync(u => u.UserId == userId
                                                          && u.IsEnabled == true
                                                          && u.ExpiresOn > DateTime.UtcNow                
                                                          && u.RefreshToken == refreshToken,
                                                     cancellationToken);

        public async Task DisableRefreshTokenAsync(int userId,
                                              string refreshToken,
                                              CancellationToken cancellationToken = default)
        {
            var entity = await _dbContext.UserRefreshTokens
                                          .FirstOrDefaultAsync(u => u.UserId == userId && u.RefreshToken == refreshToken,
                                                               cancellationToken);
            entity.IsEnabled = false;
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<SearchedUserModel>> SearchAsync(int userId,
                                                  string q,
                                                  CancellationToken cancellationToken = default)
        {

            var res = await _dbContext.Users.Where(u => u.Id != userId &&
                                                                (u.UserName.Equals(q) || EF.Functions.Like(u.FirstName , $"%{q}%") || EF.Functions.Like(u.LastName, $"%{q}%") ))
                                       .Select(u => new
                                       {
                                           u.Id,
                                           u.FirstName,
                                           u.LastName,
                                           ProfilePicUrl = u.ProfilePicture == null ? null : u.ProfilePicture.FileName,
                                           RecievedRequest = u.RecievedFriendRequests.Where(r => r.RequesterId == userId && r.IsAccepted != false)
                                                                                               .Select(r => new { r.IsAccepted })
                                                                                               .SingleOrDefault(),
                                           SentRequest = u.SentFriendRequests.Where(r => r.ForId == userId && r.IsAccepted != false)
                                                                                        .Select(r => new { r.IsAccepted })
                                                                                        .SingleOrDefault()
                                       })
                                       .ToListAsync(cancellationToken);

            return res.Select(u => new SearchedUserModel
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                ProfilePicUrl = u.ProfilePicUrl,
                FriendState = u.RecievedRequest != null ?
                                     //(GetStateByRecievedRequest(u.RecievedRequest.IsAccepted) ?? (u.SentRequest != null ? (GetStateBySentRequest(u.SentRequest.IsAccepted) ?? UserFriendState.None) : UserFriendState.None)) : UserFriendState.None
                                     (GetStateByRecievedRequest(u.RecievedRequest.IsAccepted) ??
                                        (u.SentRequest != null ? (GetStateBySentRequest(u.SentRequest.IsAccepted) ?? UserFriendState.None) : UserFriendState.None))
                                     : (u.SentRequest != null ? (GetStateBySentRequest(u.SentRequest.IsAccepted) ?? UserFriendState.None) : UserFriendState.None)

            });
        }

        static UserFriendState? GetStateByRecievedRequest(bool? isAccepted)
        {
            if (isAccepted == null)
                return UserFriendState.SentRequest;
            else if (isAccepted == true)
                return UserFriendState.Friend;
            else return null;
        }

        static UserFriendState? GetStateBySentRequest(bool? isAccepted)
        {
            if (isAccepted == null)
                return UserFriendState.RecievedRequest;
            else if (isAccepted == true)
                return UserFriendState.Friend;
            else return null;
        }

        // code from: https://code-maze.com/using-refresh-tokens-in-asp-net-core-authentication
        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

    }
}
