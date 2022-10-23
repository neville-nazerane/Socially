using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Socially.Models;
using Socially.WebAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Socially.Server.Managers;
using Socially.Server.Managers.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Socially.WebAPI.Endpoints
{
    public class UserEndpoints : EndpointsBase
    {

        public override IEndpointConventionBuilder Aggregate(RouteHandlerBuilder builder)
        {
            return builder.RequireAuthorization()
                          .WithTags("users"); 
        }

        public override IEnumerable<RouteHandlerBuilder> Setup(IEndpointRouteBuilder endpoints)
        {
            return new RouteHandlerBuilder[]
            {
                endpoints.MapGet("users", SearchAsync),
                endpoints.MapPost("users/getById", GetUsersByIdsAsync),
            };
        }

        Task<IEnumerable<SearchedUserModel>> SearchAsync(IUserProfileManager manager,
                                                         CurrentContext currentContext,
                                                         string q,
                                                         CancellationToken cancellationToken = default)
            => manager.SearchAsync(currentContext.UserId, q, cancellationToken);

        Task<IEnumerable<UserSummaryModel>> GetUsersByIdsAsync(IUserProfileManager manager,
                                                               [FromBody]IEnumerable<int> userIds,
                                                               CancellationToken cancellationToken = default)
            => manager.GetUsersByIdsAsync(userIds, cancellationToken);

    }
}
