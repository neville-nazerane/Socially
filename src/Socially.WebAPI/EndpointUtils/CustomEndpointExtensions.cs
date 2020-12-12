using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Socially.WebAPI.EndpointUtils
{
    public static class CustomEndpointExtensions
    {

        public static IEndpointConventionBuilder MapCustom<TEndpoint>(
                                                this IEndpointRouteBuilder endpoints)
            where TEndpoint : EndpointsBase, new()
            => new TEndpoint().Setup(endpoints);

        public static IEndpointConventionBuilder MapCustom<TEndpoint>(
                                        this IEndpointRouteBuilder endpoints, string path)
            where TEndpoint : EndpointsBase, new()
            => new TEndpoint().Setup(endpoints, path);

    }
}
