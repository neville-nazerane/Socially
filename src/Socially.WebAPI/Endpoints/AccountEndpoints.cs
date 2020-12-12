using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Socially.Core.Models;
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

                endpoints.MapPost("/batman", async context =>
                    await context.Response.WriteAsJsonAsync(
                                await context.TryValidateModelAsync<SignUpModel>(null)))

            };
        }
    }
}
