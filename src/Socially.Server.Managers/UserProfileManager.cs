using Microsoft.EntityFrameworkCore;
using Socially.Core.Entities;
using Socially.Core.Models;
using Socially.Server.DataAccess;
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
                         })
                         .SingleOrDefaultAsync(cancellationToken);

        public Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default) 
            => _dbContext.Users.AnyAsync(u => u.Email == email, cancellationToken);

        public Task<bool> UserNameExistsAsync(string userName, CancellationToken cancellationToken = default) 
            => _dbContext.Users.AnyAsync(u => u.UserName == userName, cancellationToken);

        public async Task<string> CreateRefreshTokenAsync(int userId, 
                                                    TimeSpan expireOn,
                                                    CancellationToken cancellationToken = default)
        {
            Core.Entities.UserRefreshToken entity = new()
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

        public Task<bool> VerifyRefreshToken(int userId,
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
