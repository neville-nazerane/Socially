using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Socially.WebAPI.EndpointUtils
{
    public static class HttpContextServiceExtensions
    {

        public static TService Service<TService>(this HttpContext context)
            => context.RequestServices.GetService<TService>();

    }
}
