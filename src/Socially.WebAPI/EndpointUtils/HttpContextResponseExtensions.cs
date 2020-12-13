using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Socially.WebAPI.EndpointUtils
{
    public static class HttpContextResponseExtensions
    {

        public static Task WriteAsync(this HttpContext context,
                                      string text,
                                      CancellationToken token = default)
            => context.Response.WriteAsync(text, token);

        public static Task WriteAsync(this HttpContext context,
                                      bool res,
                                      CancellationToken token = default)
            => context.WriteAsync(res.ToString(), token);

        public static Task WriteAsync(this HttpContext context,
                                      int res,
                                      CancellationToken token = default)
            => context.WriteAsync(res.ToString(), token);


        public static Task WriteAsync(this HttpContext context,
                                      long res,
                                      CancellationToken token = default)
            => context.WriteAsync(res.ToString(), token);


        public static Task WriteAsync(this HttpContext context,
                                      double res,
                                      CancellationToken token = default)
            => context.WriteAsync(res.ToString(), token);

    }
}
