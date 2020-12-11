using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Socially.WebAPI.EndpointUtils
{
    public static class HttpContextQueryExtensions
    {
        public static string GetQueryString(this HttpContext context, string key) => context.Request.Query[key];

        public static int GetQueryInt(this HttpContext context, string key)
            => int.Parse(context.GetQueryString(key));

        public static bool GetQueryBool(this HttpContext context, string key)
            => bool.Parse(context.GetQueryString(key));

        public static double GetQueryDouble(this HttpContext context, string key)
            => double.Parse(context.GetQueryString(key));

        public static long GetQueryLong(this HttpContext context, string key)
            => long.Parse(context.GetQueryString(key));

    }
}
