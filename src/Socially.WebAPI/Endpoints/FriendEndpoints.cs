using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Socially.WebAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Socially.WebAPI.Services;
using Socially.Models;
using Socially.Server.Managers;

namespace Socially.WebAPI.Endpoints
{
    public class FriendEndpoints : EndpointsBase
    {

        public override IEndpointConventionBuilder Aggregate(RouteHandlerBuilder builder)
            => builder.WithTags("Friends")
                      .RequireAuthorization();

        public override IEnumerable<RouteHandlerBuilder> Setup(IEndpointRouteBuilder endpoints)
        {
            return new RouteHandlerBuilder[]
            {

                endpoints.MapPost("/friend/request/{forId}", RequestAsync),
                endpoints.MapPut("/friend/respond/{requesterId}/{isAccepted}", RespondAsync),
                endpoints.MapGet("/friend/requests", GetRequestsAsync)

            };
        }

        Task RequestAsync(IFriendManager manager,
                          int forId,
                          CancellationToken cancellationToken = default)
            => manager.RequestAsync(forId, cancellationToken);

        Task<bool> RespondAsync(IFriendManager manager,
                                int requesterId,
                                bool isAccepted,
                                CancellationToken cancellationToken = default)
            => manager.RespondAsync(requesterId, isAccepted, cancellationToken);

        Task<IEnumerable<UserSummaryModel>> GetRequestsAsync(IFriendManager manager, CancellationToken cancellationToken = default)
            => manager.GetRequestsAsync(cancellationToken);

    }
}
