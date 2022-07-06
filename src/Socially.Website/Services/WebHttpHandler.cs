using Socially.Apps.Consumer.Utils;

namespace Socially.Website.Services
{
    public class WebHttpHandler : ApiHttpHandler
    {
        private readonly AuthService authService;

        public WebHttpHandler(AuthService authService)
        {
            this.authService = authService;
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
