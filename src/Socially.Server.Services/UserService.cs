using Microsoft.AspNetCore.Identity;
using Socially.Core.Entities;
using Socially.Core.Exceptions;
using Socially.Core.Models;
using Socially.Server.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task SignUpAsync(SignUpModel model, CancellationToken cancellationToken = default)
        {

            var emailExists = await _userVerificationManager.EmailExistsAsync(model.Email, cancellationToken);

            if (emailExists)
            {
                throw new BadRequestException(
                                new ErrorModel(nameof(model.Email), 
                                               "Email already exists"));
            }

            var usernameExists = await _userVerificationManager.UserNameExistsAsync(model.Email, cancellationToken);

            if (usernameExists)
            {
                throw new BadRequestException(
                                new ErrorModel(nameof(model.Email),
                                               "Email already exists"));
            }


            var result = await _userManager.CreateAsync(new User
            {
                Email = model.Email,
                UserName = model.UserName
            }, model.Password);

            if (!result.Succeeded)
                throw new BadRequestException(GetErrorModel(result.Errors));

        }

        private IEnumerable<ErrorModel> GetErrorModel(IEnumerable<IdentityError> errors)
        {
            var result = new List<ErrorModel>();
            string[] names = new string[]
            { 
                nameof(SignUpModel.UserName),  
                nameof(SignUpModel.Password),  
                nameof(SignUpModel.Email) 
            };

            foreach (var errorModel in errors)
            {
                string fieldName = "";
                foreach (var name in names)
                    if (errorModel.Description.Contains(name))
                        fieldName = name;
                
                var err = result.SingleOrDefault(e => e.Field == fieldName);
                if (err is null)
                {
                    err = new ErrorModel
                    {
                        Field= fieldName,
                        Errors = new List<string>()
                    };
                    result.Add(err);
                }
                err.Errors.Add(errorModel.Description);
            }

            return result;
        }

        public Task<bool> VerifyEmailAsync(string email, CancellationToken cancellationToken = default)
            => _userVerificationManager.EmailExistsAsync(email, cancellationToken);

        public Task<bool> VerifyUsernameAsync(string userName, CancellationToken cancellationToken = default)
            => _userVerificationManager.UserNameExistsAsync(userName, cancellationToken);

    }
}
