using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Socially.Core.Models;
using Socially.WebAPI.Models;
using Socially.WebAPI.Services;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Socially.WebAPI.Endpoints
{
    public class ProfileEndpoints : EndpointsBase
    {
        public override IEndpointConventionBuilder Aggregate(RouteHandlerBuilder builder)
        {
            return builder.WithTags("Profile")
                          .RequireAuthorization();
        }

        public override IEnumerable<RouteHandlerBuilder> Setup(IEndpointRouteBuilder endpoints)
        {

            return new RouteHandlerBuilder[]
            {

                endpoints.MapPut("profile", UpdateProfileAsync),
                endpoints.MapGet("profile", GetUpdatableProfileAsync)

            };

        }

        static Task<ProfileUpdateModel> GetUpdatableProfileAsync(IUserService service,
                                                         CancellationToken cancellationToken = default)
            => service.GetUpdatableProfileAsync(cancellationToken);

        static Task UpdateProfileAsync(IUserService service,
                                       ProfileUpdateModel model,
                                       CancellationToken cancellation = default)
            => service.UpdateProfileAsync(model, cancellation);

    }
}
