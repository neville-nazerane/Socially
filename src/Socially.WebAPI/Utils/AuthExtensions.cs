using Microsoft.AspNetCore.Http;
using Socially.Server.Managers.Utils;
using System.Security.Claims;

namespace Socially.WebAPI.Utils
{
    public static class AuthExtensions
    {

        public static bool IsAuthenticated(this ClaimsPrincipal user)
            => user.Identity.IsAuthenticated;

        public static int GetUserId(this ClaimsPrincipal user)
            => int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier));

        public static void Populate(this ClaimsPrincipal user, CurrentContext context)
        {
            if (user.IsAuthenticated())
            {
                context.UserId = GetUserId(user);
            }
        }

    }
}
