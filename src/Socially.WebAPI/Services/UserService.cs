using Microsoft.AspNetCore.Identity;
using NetCore.Jwt;
using Socially.Core.Entities;
using Socially.Core.Exceptions;
using Socially.Core.Models;
using Socially.Server.Managers;
using Socially.Server.Services.Models;
using Socially.WebAPI.Models;
using Socially.WebAPI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Socially.WebAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserProfileManager _userProfileManager;
        private readonly CurrentContext _currentContext;
        private readonly TokenInfo _tokenInfo;
        private readonly UserManager<User> _userManager;

        public UserService(UserManager<User> userManager,
                           IUserProfileManager userProfileManager,
                           CurrentContext currentContext,
                           TokenInfo tokenInfo)
        {
            _userManager = userManager;
            _userProfileManager = userProfileManager;
            _currentContext = currentContext;
            _tokenInfo = tokenInfo;
        }

        public async Task<string> LoginAsync(LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user is null) return null;
            bool valid = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!valid) return null;
            Claim[] claims = new Claim[] {
                new Claim(ClaimTypes.Name, model.UserName),
                new Claim(ClaimTypes.GivenName, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };
            return _tokenInfo.GenerateToken(new TokenRequest
            {
                Claims = claims,
                ExpireIn = TimeSpan.FromHours(2),
                Audience = model.Source
            });
            //return _bearerManager.Generate(claims);
        }

        public async Task SignUpAsync(SignUpModel model, CancellationToken cancellationToken = default)
        {

            var emailExists = await _userProfileManager.EmailExistsAsync(model.Email, cancellationToken);

            if (emailExists)
            {
                throw new BadRequestException(
                                new ErrorModel(nameof(model.Email),
                                               "Email already exists"));
            }

            var usernameExists = await _userProfileManager.UserNameExistsAsync(model.UserName, cancellationToken);

            if (usernameExists)
            {
                throw new BadRequestException(
                                new ErrorModel(nameof(model.UserName),
                                               "Username already exists"));
            }


            var result = await _userManager.CreateAsync(new User
            {
                Email = model.Email,
                UserName = model.UserName,
                CreatedOn = DateTime.UtcNow
            }, model.Password);

            if (!result.Succeeded)
                throw new BadRequestException(GetErrorModel(result.Errors));

        }

        public Task UpdateProfileAsync(ProfileUpdateModel model, CancellationToken cancellation = default)
            => _userProfileManager.UpdateAsync(_currentContext.UserId, model, cancellation);

        public Task<ProfileUpdateModel> GetUpdatableProfileAsync(CancellationToken cancellationToken = default)
            => _userProfileManager.GetUpdatableProfileAsync(_currentContext.UserId, cancellationToken);

        private static IEnumerable<ErrorModel> GetErrorModel(IEnumerable<IdentityError> errors)
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
                        Field = fieldName,
                        Errors = new List<string>()
                    };
                    result.Add(err);
                }
                err.Errors.Add(errorModel.Description);
            }

            return result;
        }

        public Task<bool> VerifyEmailAsync(string email, CancellationToken cancellationToken = default)
            => _userProfileManager.EmailExistsAsync(email, cancellationToken);

        public Task<bool> VerifyUsernameAsync(string userName, CancellationToken cancellationToken = default)
            => _userProfileManager.UserNameExistsAsync(userName, cancellationToken);

    }
}
