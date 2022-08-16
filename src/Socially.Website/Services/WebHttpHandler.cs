using Microsoft.AspNetCore.Components.Authorization;
using Socially.Apps.Consumer.Utils;
using Socially.Core.Models;
using System.Net.Http.Json;

namespace Socially.Website.Services
{
    public class WebHttpHandler : ApiHttpHandler
    {
        private readonly AuthProvider _authService;

        public WebHttpHandler(AuthenticationStateProvider authService)
        {
            _authService = (AuthProvider) authService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            await UseTokenAsync(request, cancellationToken);
            var res = await base.SendAsync(request, cancellationToken);
            if (res.StatusCode == System.Net.HttpStatusCode.Unauthorized || res.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                await RenewTokenAsync($"{request.RequestUri.Scheme}://{request.RequestUri.Authority}", cancellationToken);
                await UseTokenAsync(request, cancellationToken);
                res = await base.SendAsync(request, cancellationToken);
            }
            return res;
        }

        private async ValueTask UseTokenAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var expiary = await _authService.GetExiparyAsync(cancellationToken);
            if (expiary.HasValue && DateTime.UtcNow > expiary)
                await RenewTokenAsync($"{request.RequestUri.Scheme}://{request.RequestUri.Authority}",  cancellationToken);

            var token = await _authService.GetTokenAsync(cancellationToken);
            if (token is not null)
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", token);
        }

        private async Task RenewTokenAsync(string baseUrl, CancellationToken cancellationToken = default)
        {
            var renewRequest = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}/renewToken");
            var info = await _authService.GetTokenInfoAsync(cancellationToken);
            if (info is null) return;
            renewRequest.Content = JsonContent.Create(new TokenRenewRequestModel
            {
                AccessToken = info.AccessToken,
                RefreshToken = info.RefreshToken,
            });
            var res = await base.SendAsync(renewRequest, cancellationToken);
            res.EnsureSuccessStatusCode();
            var tokenResponse = await res.Content.ReadFromJsonAsync<TokenResponseModel>(cancellationToken: cancellationToken);
            await _authService.SetAsync(tokenResponse, cancellationToken);
        }

    }
}
