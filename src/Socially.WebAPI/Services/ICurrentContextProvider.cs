using Microsoft.AspNetCore.Http;
using Socially.Server.Managers.Utils;

namespace Socially.WebAPI.Services
{
    public interface ICurrentContextProvider
    {
        void SetupCurrentContext(HttpContext httpContext, CurrentContext context);
    }
}