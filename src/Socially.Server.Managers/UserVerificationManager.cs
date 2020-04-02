using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Socially.Server.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Socially.Server.Managers
{
    public class UserVerificationManager : IUserVerificationManager
    {
        private readonly ApplicationDbContext _dbContext;

        public UserVerificationManager(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
        {
            return _dbContext.Users.AnyAsync(u => u.Email == email, cancellationToken);
        }

        public Task<bool> UserNameExistsAsync(string userName, CancellationToken cancellationToken = default)
        {
            return _dbContext.Users.AnyAsync(u => u.UserName == userName, cancellationToken);
        }

    }
}
