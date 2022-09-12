using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using Socially.Core.Models;
using Socially.WebAPI.Models;
using Socially.WebAPI.Services;
using Socially.WebAPI.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Socially.WebAPI.Endpoints
{
    public class ImagesEndpoints : EndpointsBase
    {

        public override IEndpointConventionBuilder Aggregate(RouteHandlerBuilder builder) 
            => builder.WithTags("images")
                      .RequireAuthorization();


        public override IEnumerable<RouteHandlerBuilder> Setup(IEndpointRouteBuilder endpoints)
        {
            return new RouteHandlerBuilder[]
            {
                endpoints.MapGet("images", GetAllForUserAsync),
                endpoints.MapPost("image", UploadAsync),
                endpoints.MapDelete("image/{fileName}", DeleteAsync)
            };
        }

        Task<string> UploadAsync(FormBinder<ImageUploadModel> model,
                         IImagesService service,
                         CancellationToken cancellationToken = default)
            => service.UploadAsync(model.Model.Image, cancellationToken);

        Task<IEnumerable<string>> GetAllForUserAsync(IImagesService service, CancellationToken cancellationToken = default)
            => service.GetAllForUserAsync(cancellationToken);

        Task DeleteAsync(IImagesService service, string fileName, CancellationToken cancellationToken = default)
            => service.DeleteAsync(fileName, cancellationToken);

    }
}
