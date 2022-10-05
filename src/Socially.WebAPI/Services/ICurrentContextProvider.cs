using Microsoft.AspNetCore.Http;
using Socially.Server.Services.Models;

namespace Socially.WebAPI.Services
{
    public interface ICurrentContextProvider
    {
        void SetupCurrentContext(HttpContext httpContext, CurrentContext context);
    }
}