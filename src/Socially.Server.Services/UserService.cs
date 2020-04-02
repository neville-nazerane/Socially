using Microsoft.AspNetCore.Identity;
using Socially.Core.Entities;
using Socially.Server.Managers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Socially.Server.Services
{
    public class UserService
    {
        private readonly IUserVerificationManager _userVerificationManager;

        public UserService(SignInManager<User> signInManager, IUserVerificationManager userVerificationManager)
        {
            _userVerificationManager = userVerificationManager;
        }



    }
}
