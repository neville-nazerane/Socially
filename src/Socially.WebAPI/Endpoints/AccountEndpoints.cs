using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Socially.Core.Exceptions;
using Socially.Core.Models;
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

                endpoints.MapPut("profile", UpdateProfileAsync).RequireAuthorization(),
                endpoints.MapGet("profile", GetUpdatableProfileAsync).RequireAuthorization()

            };
        }



        static Task<ProfileUpdateModel> GetUpdatableProfileAsync(IUserService service,
                                                                 CancellationToken cancellationToken = default)
            => service.GetUpdatableProfileAsync(cancellationToken);

        static Task UpdateProfileAsync(IUserService service,
                                       ProfileUpdateModel model,
                                       CancellationToken cancellation = default)
            => service.UpdateProfileAsync(model, cancellation);

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
