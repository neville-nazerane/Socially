using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Socially.Core.Exceptions;
using Socially.Core.Models;
using Socially.Server.Services;
using Socially.WebAPI.EndpointUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Socially.WebAPI.Endpoints
{
    public class AccountEndpoints : EndpointsBase
    {
        public override MultiEndpointConventionBuilder Setup(IEndpointRouteBuilder endpoints)
        {

            return new MultiEndpointConventionBuilder {

                endpoints.MapGet("account/verifyEmail/{email}", VerifyEmailAsync),

                endpoints.MapGet("account/verifyUsername/{userName}", VerifyUsernameAsync),

                endpoints.MapPost($"account/signup", SignupAsync),

                endpoints.MapPost($"account/login", LoginAsync)

            };
        }


        static Task<bool> VerifyEmailAsync(string email,
                                     IUserService service,
                                     CancellationToken cancellationToken = default)
            => service.VerifyEmailAsync(email, cancellationToken);

        static Task<bool> VerifyUsernameAsync(string userName, 
                                        IUserService service, 
                                        CancellationToken cancellationToken = default)
            => service.VerifyUsernameAsync(userName, cancellationToken);

        static Task SignupAsync(SignUpModel model,
                                 IUserService userService,
                                 CancellationToken cancellationToken = default)
            => userService.SignUpAsync(model, cancellationToken);

        static async Task<string> LoginAsync(LoginModel model,
                                             IUserService service)
        {
            var res = await service.LoginAsync(model);
            if (res == null)
                throw new BadRequestException(string.Empty);

            return res;
        }

    }
}
