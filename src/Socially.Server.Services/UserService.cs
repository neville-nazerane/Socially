using Microsoft.AspNetCore.Identity;
using Socially.Core.Entities;
using Socially.Core.Models;
using Socially.Server.Managers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Socially.Server.Services
{
    public class UserService : IUserService
    {
        private readonly IUserVerificationManager _userVerificationManager;
        private readonly UserManager<User> _userManager;

        public UserService(UserManager<User> userManager, IUserVerificationManager userVerificationManager)
        {
            _userManager = userManager;
            _userVerificationManager = userVerificationManager;
        }

        public async Task<bool> LoginAsync(LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user is null) return false;
            return await _userManager.CheckPasswordAsync(user, model.Password);
        }

        public async Task<IdentityResult> SignUpAsync(SignUpModel model)
            => await _userManager.CreateAsync(new User
            {
                Email = model.Email,
                UserName = model.UserName
            }, model.Password);

        public Task<bool> VerifyEmailAsync(string email, CancellationToken cancellationToken = default)
            => _userVerificationManager.EmailExistsAsync(email, cancellationToken);

        public Task<bool> VerifyUsernameAsync(string userName, CancellationToken cancellationToken = default)
            => _userVerificationManager.UserNameExistsAsync(userName, cancellationToken);

    }
}
