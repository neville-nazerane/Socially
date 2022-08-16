﻿using Microsoft.AspNetCore.Identity;
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
using System.IdentityModel.Tokens.Jwt;
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

        public Task<TokenResponseModel> LoginAsync(LoginModel model, CancellationToken cancellationToken = default)
            => GetTokenIfLoginValidAsync(model, TimeSpan.FromHours(1), cancellationToken);
        
        public Task<TokenResponseModel> RenewTokenAsync(TokenRenewRequestModel model, CancellationToken cancellationToken = default)
            => RenewTokenAsync(model, TimeSpan.FromHours(1), cancellationToken);

        private async Task<TokenResponseModel> GetTokenIfLoginValidAsync(LoginModel model,
                                                                         TimeSpan expireIn,
                                                                         CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user is null) return null;
            bool valid = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!valid) return null;
            var token = _tokenInfo.GenerateToken(new TokenRequest
            {
                Claims = GetClaimsForUser(user),
                ExpireIn = expireIn,
                Audience = model.Source
            });

            var refreshToken = await _userProfileManager.CreateRefreshTokenAsync(user.Id, TimeSpan.FromDays(90), cancellationToken);

            return new TokenResponseModel
            {
                AccessToken = token,
                ExpiresIn = expireIn.TotalSeconds,
                RefreshToken = refreshToken,
                TokenType = "Bearer",
            };
        }

        private static Claim[] GetClaimsForUser(User user)
        {
            return new Claim[] {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.GivenName, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };
        }

        private async Task<TokenResponseModel> RenewTokenAsync(TokenRenewRequestModel model, 
                                                               TimeSpan expireIn,
                                                               CancellationToken cancellationToken = default)
        {
            var principle = _tokenInfo.GetPrinciple(model.AccessToken);
            var readToken = new JwtSecurityTokenHandler().ReadJwtToken(model.AccessToken);

            int userId = int.Parse(principle.FindFirstValue(ClaimTypes.NameIdentifier));
            if (await _userProfileManager.VerifyRefreshToken(userId, model.RefreshToken, cancellationToken))
            {
                var user = await _userProfileManager.GetUserByIdAsync(userId, cancellationToken);
                var token = _tokenInfo.GenerateToken(new TokenRequest
                {
                    Claims = GetClaimsForUser(user),
                    ExpireIn = expireIn,
                    Audience = readToken.Audiences.FirstOrDefault()
                });
                var refreshToken = await _userProfileManager.CreateRefreshTokenAsync(userId, TimeSpan.FromDays(90), cancellationToken);
                var result = new TokenResponseModel
                {
                    AccessToken = token,
                    ExpiresIn = expireIn.TotalSeconds,
                    RefreshToken = refreshToken,
                    TokenType = "Bearer"
                };
                await _userProfileManager.DisableRefreshTokenAsync(userId, model.RefreshToken, cancellationToken);
                return result;
            }
            else throw new BadRequestException("Invalid token");
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
