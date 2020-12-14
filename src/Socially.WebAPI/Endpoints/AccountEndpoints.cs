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
        public override MultiEndpointConventionBuilder Setup(IEndpointRouteBuilder endpoints)
        {

            return new MultiEndpointConventionBuilder {

                endpoints.MapGet($"{Path}/verifyEmail/{{email}}", async context
                    => await context.WriteAsync( 
                                await context.Service<IUserService>()
                                             .VerifyEmailAsync(context.GetRouteString("email"),
                                                               context.RequestAborted), 
                                context.RequestAborted)),

                endpoints.MapGet($"{Path}/verifyUsername/{{userName}}", async context
                     => await context.WriteAsync(
                                await context.Service<IUserService>()
                                             .VerifyUsernameAsync(context.GetRouteString("userName"),
                                                                  context.RequestAborted),
                                context.RequestAborted)),

                endpoints.MapPost($"{Path}/signup", async context
                    => await context.TryValidateModelAsync<SignUpModel>(
                                m => context.Service<IUserService>()
                                            .SignUpAsync(m),
                                context.RequestAborted)),

                endpoints.MapPost($"{Path}/login", async context
                    => await context.TryValidateModelAsync<LoginModel>(async m 
                                => context.Response.StatusCode =
                                        (await context.Service<IUserService>()
                                                      .LoginAsync(m)) 
                                                      ? StatusCodes.Status200OK 
                                                      : StatusCodes.Status400BadRequest,
                                context.RequestAborted))

            };
        }
    }
}
