using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Socially.WebAPI.EndpointUtils
{
    public class MultiEndpointConventionBuilder : List<IEndpointConventionBuilder>, IEndpointConventionBuilder
    {

        public void Add(Action<EndpointBuilder> convention)
        {
            foreach (var conv in this) conv.Add(convention);
        }
    }
}
