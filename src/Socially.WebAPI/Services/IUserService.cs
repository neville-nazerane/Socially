﻿using Microsoft.AspNetCore.Identity;
using Socially.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Socially.WebAPI.Services
{
    public interface IUserService
    {
        Task ForgotPasswordAsync(string email, CancellationToken cancellationToken = default);
        Task<ProfileUpdateModel> GetUpdatableProfileAsync(CancellationToken cancellationToken = default);
        Task<TokenResponseModel> LoginAsync(LoginModel model, CancellationToken cancellationToken = default);
        Task<TokenResponseModel> RenewTokenAsync(TokenRenewRequestModel model, CancellationToken cancellationToken = default);
        Task ResetForgottenPasswordAsync(ForgotPasswordModel model, CancellationToken cancellationToken = default);
        Task ResetPasswordAsync(PasswordResetModel model, CancellationToken cancellationToken = default);
        Task SignUpAsync(SignUpModel model, CancellationToken cancellationToken = default);
        Task UpdateProfileAsync(ProfileUpdateModel model, CancellationToken cancellation = default);
        Task<bool> VerifyEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<bool> VerifyUsernameAsync(string userName, CancellationToken cancellationToken = default);
    }
}