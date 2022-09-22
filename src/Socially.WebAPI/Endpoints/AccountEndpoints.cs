using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Socially.Models;
using Socially.Models.Exceptions;
using Socially.WebAPI.Models;
using Socially.WebAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Socially.WebAPI.Endpoints
{

    public class AccountEndpoints : EndpointsBase
    {

        public override IEndpointConventionBuilder Aggregate(RouteHandlerBuilder builder)
        {
            return builder.WithTags("Account stuff");
        }

        public override IEnumerable<RouteHandlerBuilder> Setup(IEndpointRouteBuilder endpoints)
        {
            return new RouteHandlerBuilder[]
            {

                endpoints.MapPost("signup", SignupAsync),
                endpoints.MapPost("login", LoginAsync),
                endpoints.MapPost("renewToken", RenewTokenAsync),

                endpoints.MapPost("forgotPassword/{email}", ForgotPasswordAsync),
                endpoints.MapPut("resetForgottenPassword", ResetForgottenPasswordAsync)
            };
        }

        static Task ForgotPasswordAsync(string email, 
                                       IUserService service,
                                       CancellationToken cancellationToken = default)
            => service.ForgotPasswordAsync(email, cancellationToken);

        static Task ResetForgottenPasswordAsync(ForgotPasswordModel model,
                                                      IUserService service,
                                                      CancellationToken cancellationToken = default)
            => service.ResetForgottenPasswordAsync(model, cancellationToken);


        static Task<TokenResponseModel> RenewTokenAsync(TokenRenewRequestModel mode,
                                                        IUserService service,
                                                        CancellationToken cancellationToken = default)
            => service.RenewTokenAsync(mode, cancellationToken);


        static Task SignupAsync(SignUpModel model,
                                IUserService userService,
                                CancellationToken cancellationToken = default)
            => userService.SignUpAsync(model, cancellationToken);

        static async Task<TokenResponseModel> LoginAsync(LoginModel model,
                                             IUserService service)
        {
            var res = await service.LoginAsync(model);
            if (res == null)
                throw new BadRequestException(string.Empty);

            return res;
        }



    }
}
