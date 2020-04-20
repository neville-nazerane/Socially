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
        private readonly SignInManager<User> _signInManager;
        private readonly IUserVerificationManager _userVerificationManager;

        public UserService(SignInManager<User> signInManager, IUserVerificationManager userVerificationManager)
        {
            _signInManager = signInManager;
            _userVerificationManager = userVerificationManager;
        }

        public async Task<SignInResult> LoginAsync(LoginModel model)
            => await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);

        public async Task<IdentityResult> SignUpAsync(SignUpModel model)
            => await _signInManager.UserManager.CreateAsync(new User
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
