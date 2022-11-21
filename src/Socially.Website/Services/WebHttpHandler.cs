using Microsoft.AspNetCore.Components.Authorization;
using Socially.Apps.Consumer.Utils;
using Socially.Models;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Socially.Website.Services
{
    public class WebHttpHandler : ApiHttpHandler
    {
        private static TaskCompletionSource renewalLock;

        private readonly AuthProvider _authService;

        public WebHttpHandler(AuthProvider authService)
        {
            _authService = authService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            await RenewTokenIfExpired(request, cancellationToken);
            var res = await base.SendAsync(request, cancellationToken);
            if (res.StatusCode == System.Net.HttpStatusCode.Unauthorized || res.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                await ForceRenewTokenAsync($"{request.RequestUri.Scheme}://{request.RequestUri.Authority}", cancellationToken);
                await UseTokenAsync(request, cancellationToken);
                res = await base.SendAsync(request, cancellationToken);
            }
            return res;
        }

        private async ValueTask RenewTokenIfExpired(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var expiary = await _authService.GetExiparyAsync(cancellationToken);
            if (expiary.HasValue && DateTime.UtcNow > expiary)
            {
                await Console.Out.WriteLineAsync($"Detected expired token {expiary.Value.ToLocalTime()}");
                await ForceRenewTokenAsync($"{request.RequestUri.Scheme}://{request.RequestUri.Authority}", cancellationToken);
            }

            await UseTokenAsync(request, cancellationToken);
        }

        private async ValueTask UseTokenAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _authService.GetTokenAsync(cancellationToken);
            if (token is not null)
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", token);
        }

        private async Task ForceRenewTokenAsync(string baseUrl, CancellationToken cancellationToken = default)
        {
            if (renewalLock is not null)
            {
                await renewalLock.Task;
            }
            else
            {
                var lockInstance = new TaskCompletionSource();
                renewalLock = lockInstance;
                try
                {

                    await Console.Out.WriteLineAsync("Refresing token");
                    var renewRequest = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}/renewToken");
                    var info = await _authService.GetTokenInfoAsync(cancellationToken);

                    // if nothing stored in ram or local storage
                    if (info is null) return;
                    renewRequest.Content = JsonContent.Create(new TokenRenewRequestModel
                    {
                        AccessToken = info.AccessToken,
                        RefreshToken = info.RefreshToken,
                    });
                    await Console.Out.WriteLineAsync("Requesting refresh token");
                    var res = await base.SendAsync(renewRequest, cancellationToken);
                    await Console.Out.WriteLineAsync("Done refreshing token");
                    res.EnsureSuccessStatusCode();
                    var tokenResponse = await res.Content.ReadFromJsonAsync<TokenResponseModel>(cancellationToken: cancellationToken);
                    await _authService.SetAsync(tokenResponse, cancellationToken);

                    renewalLock = null;
                    lockInstance.TrySetResult();
                }
                catch (Exception ex)
                {
                    renewalLock = null;
                    lockInstance.TrySetException(ex);
                }
            }

        }

    }
}
