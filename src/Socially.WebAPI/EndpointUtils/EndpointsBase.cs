using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Socially.WebAPI.EndpointUtils
{
    public abstract class EndpointsBase
    {

        protected string Path { get; private set; }

        public abstract EndpointMultiConvention Setup(IEndpointRouteBuilder endpoints);

        public EndpointMultiConvention Setup(IEndpointRouteBuilder endpoints, string path)
        {
            Path = path;
            return Setup(endpoints);
        }

    }
}
