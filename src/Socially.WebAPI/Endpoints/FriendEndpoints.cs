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
            return new[]
            {
                endpoints.MapPost("/friend/request/{forId}", RequestAsync),
                endpoints.MapPut("/friend/respond/{requesterId}/{isAccepted}", RespondAsync),
                endpoints.MapGet("/friend/requests", GetRequestsAsync),
                endpoints.MapGet("/friends", GetFriendsAsync),
                endpoints.MapDelete("/friend/{friendId}", RemoveFriendAsync)
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

        Task<IEnumerable<UserSummaryModel>> GetFriendsAsync(IFriendManager manager, CancellationToken cancellationToken = default)
            => manager.GetFriendsAsync(cancellationToken);

        Task<int> RemoveFriendAsync(IFriendManager friendManager,
                                     int friendId,        
                                     CancellationToken cancellationToken = default)
            => friendManager.RemoveFriendAsync(friendId, cancellationToken);

    }
}
