using Microsoft.AspNetCore.Http;
using Socially.Server.Managers.Utils;
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

        public async Task InvokeAsync(HttpContext httpContext, CurrentContext context)
        {
            if (httpContext.User.Identity.IsAuthenticated)
            {
                context.UserId = int.Parse(
                                        httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            }
            await _next(httpContext);
        }

    }
}
