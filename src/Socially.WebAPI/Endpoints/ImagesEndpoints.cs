using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Socially.Core.Models;
using Socially.WebAPI.Models;
using Socially.WebAPI.Services;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Socially.WebAPI.Endpoints
{
    public class ImagesEndpoints : EndpointsBase
    {

        public override IEndpointConventionBuilder Aggregate(RouteHandlerBuilder builder) => builder.WithTags("images");


        public override IEnumerable<RouteHandlerBuilder> Setup(IEndpointRouteBuilder endpoints)
        {
            return new RouteHandlerBuilder[]
            {
                endpoints.MapGet("images", GetAllForUserAsync),
                endpoints.MapPost("image", UploadAsync)
            };
        }

        Task UploadAsync(ImagesService service,
                        [FromForm]ImageUploadModel model,
                         CancellationToken cancellationToken = default)
            => service.UploadAsync(model.Image, cancellationToken);

        Task<IEnumerable<string>> GetAllForUserAsync(ImagesService service, CancellationToken cancellationToken = default)
            => service.GetAllForUserAsync(cancellationToken);

    }
}
