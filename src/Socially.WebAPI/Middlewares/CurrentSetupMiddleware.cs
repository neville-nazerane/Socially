using Microsoft.AspNetCore.Http;
using Socially.Server.Managers.Utils;
using Socially.WebAPI.Utils;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Socially.WebAPI.Middlewares
{
    public class CurrentSetupMiddleware
    {
        private readonly RequestDelegate _next;

        public CurrentSetupMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task InvokeAsync(HttpContext httpContext, CurrentContext context)
        {
            httpContext.User.Populate(context);
            return _next(httpContext);
        }

    }
}
