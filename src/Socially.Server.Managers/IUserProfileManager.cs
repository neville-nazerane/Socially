﻿using Socially.Core.Models;
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
        Task DisableRefreshToken(int userId, string refreshToken, CancellationToken cancellationToken = default);
        Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);
        Task<ProfileUpdateModel> GetUpdatableProfileAsync(int userId, CancellationToken cancellationToken = default);
        Task UpdateAsync(int userId, ProfileUpdateModel model, CancellationToken cancellationToken = default);
        Task<bool> UserNameExistsAsync(string userName, CancellationToken cancellationToken = default);
        Task<bool> VerifyRefreshToken(int userId, string refreshToken, CancellationToken cancellationToken = default);
    }
}
