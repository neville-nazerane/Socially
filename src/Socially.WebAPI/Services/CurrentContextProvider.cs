using Microsoft.AspNetCore.Http;
using Socially.Server.Managers.Utils;
using System.Security.Claims;

namespace Socially.WebAPI.Services
{
    public class CurrentContextProvider : ICurrentContextProvider
    {

        public void SetupCurrentContext(HttpContext httpContext, CurrentContext context)
        {
            if (httpContext.User.Identity.IsAuthenticated)
            {
                context.UserId = int.Parse(
                                        httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            }
        }

    }
}
