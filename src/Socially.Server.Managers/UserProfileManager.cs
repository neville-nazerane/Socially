using Microsoft.EntityFrameworkCore;
using Socially.Core.Models;
using Socially.Server.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task UpdateAsync(int userId, ProfileUpdateModel model, CancellationToken cancellationToken = default)
        {
            if (model is null) throw new ArgumentNullException(nameof(model));
            var profile = await _dbContext.Users.FindAsync(new object[]{ userId }, cancellationToken: cancellationToken);
            
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

    }
}
