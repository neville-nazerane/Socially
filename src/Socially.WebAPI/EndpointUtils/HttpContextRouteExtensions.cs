using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Socially.WebAPI.EndpointUtils
{
    public static class HttpContextRouteExtensions
    {

        public static string GetRouteString(this HttpContext context, string key)
            => context.Request.RouteValues[key].ToString();

        public static int GetRouteInt(this HttpContext context, string key)
            => int.Parse(context.GetRouteString(key));

        public static bool GetRouteBool(this HttpContext context, string key)
            => bool.Parse(context.GetRouteString(key));

        public static double GetRouteDouble(this HttpContext context, string key)
            => double.Parse(context.GetRouteString(key));

        public static long GetRouteLong(this HttpContext context, string key)
            => long.Parse(context.GetRouteString(key));

    }
}
