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
    public class ImagesEndpoints : EndpointsBase
    {

        public override IEndpointConventionBuilder Aggregate(RouteHandlerBuilder builder) => builder.WithTags("images");


        public override IEnumerable<RouteHandlerBuilder> Setup(IEndpointRouteBuilder endpoints)
        {
            return new RouteHandlerBuilder[]
            {
                endpoints.MapPost("image", UploadAsync)
            };
        }

        Task UploadAsync(ImagesService service,
                         ImageUploadModel model,
                         CancellationToken cancellationToken = default)
            => service.UploadAsync(model.Image, cancellationToken);

    }
}
