using Microsoft.AspNetCore.Components.Authorization;
using Socially.Apps.Consumer.Utils;

namespace Socially.Website.Services
{
    public class WebHttpHandler : ApiHttpHandler
    {
        private readonly AuthProvider authService;

        public WebHttpHandler(AuthenticationStateProvider authService)
        {
            this.authService = (AuthProvider) authService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await authService.GetTokenAsync(cancellationToken);
            if (token is not null)
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", token);
            return await base.SendAsync(request, cancellationToken);
        }

    }
}
