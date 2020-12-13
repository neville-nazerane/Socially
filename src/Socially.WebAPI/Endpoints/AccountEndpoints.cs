using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Socially.Core.Models;
using Socially.Server.Services;
using Socially.WebAPI.EndpointUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Socially.WebAPI.Endpoints
{
    public class AccountEndpoints : EndpointsBase
    {
        public override EndpointMultiConvention Setup(IEndpointRouteBuilder endpoints)
        {
            return new EndpointMultiConvention {

                endpoints.MapGet($"{Path}/verifyEmail/{{email}}", context
                    => context.Service<IUserService>()
                               .VerifyEmailAsync(context.GetRouteString("email"),
                                                 context.RequestAborted)),

                endpoints.MapGet($"{Path}/verifyUsername/{{userName}}", context
                     => context.Service<IUserService>()
                               .VerifyUsernameAsync(context.GetRouteString("userName"),
                                                    context.RequestAborted)),

                //endpoints.MapPost($"{Path}/signup", async context 
                //    => await context.TryValidateModelAsync()),

            };
        }
    }
}
