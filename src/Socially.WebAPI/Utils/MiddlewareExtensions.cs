using Microsoft.AspNetCore.Builder;
using Socially.WebAPI.Middlewares;

namespace Socially.WebAPI.Utils
{
    public static class MiddlewareExtensions
    {

        public static WebApplication UseCurrentSetup(this WebApplication app)
        {
            app.UseMiddleware<CurrentSetupMiddleware>();
            return app;
        }

    }
}
