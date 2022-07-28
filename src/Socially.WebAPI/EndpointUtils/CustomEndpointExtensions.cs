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
        {
            var ends = new TEndpoint();
            var result = new MultiEndpointConventionBuilder();
            foreach (var res in ends.Setup(endpoints))
                result.Add(ends.Aggregate(res));
            return result;
        }

        class MultiEndpointConventionBuilder : List<IEndpointConventionBuilder>, IEndpointConventionBuilder
        {

            public void Add(Action<EndpointBuilder> convention)
            {
                foreach (var conv in this) conv.Add(convention);
            }
        }

    }
}
