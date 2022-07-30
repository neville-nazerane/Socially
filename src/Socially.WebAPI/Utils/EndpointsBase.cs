using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Socially.WebAPI.Utils
{
    public abstract class EndpointsBase
    {

        public virtual IEndpointConventionBuilder Aggregate(RouteHandlerBuilder builder) => builder;

        public abstract IEnumerable<RouteHandlerBuilder> Setup(IEndpointRouteBuilder endpoints);

    }
}
