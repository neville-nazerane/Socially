using Microsoft.AspNetCore.Components.Authorization;
using Socially.Apps.Consumer.Services;
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
        private readonly IAuthAccess _authAccess;

        public WebHttpHandler(IAuthAccess authAccess)
        {
            _authAccess = authAccess;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return await ValidateAuthTokenAsync(request, cancellationToken);
        }

        private async Task<HttpResponseMessage> ValidateAuthTokenAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            await RenewTokenIfExpired(request, cancellationToken);
            var res = await base.SendAsync(request, cancellationToken);
            if (res.StatusCode == System.Net.HttpStatusCode.Unauthorized || res.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                await ForceRenewTokenAsync($"{request.RequestUri.Scheme}://{request.RequestUri.Authority}", cancellationToken);
                await UseTokenAsync(request);
                res = await base.SendAsync(request, cancellationToken);
            }
            return res;
        }

        private async ValueTask RenewTokenIfExpired(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var storedToken = await _authAccess.GetStoredTokenAsync();
            var expiary = storedToken?.Expiary;
            if (expiary.HasValue && DateTime.UtcNow > expiary)
            {
                await Console.Out.WriteLineAsync($"Detected expired token {expiary.Value.ToLocalTime()}");
                await ForceRenewTokenAsync($"{request.RequestUri.Scheme}://{request.RequestUri.Authority}", cancellationToken);
            }

            await UseTokenAsync(request);
        }

        private async ValueTask UseTokenAsync(HttpRequestMessage request)
        {
            var token = await _authAccess.GetStoredTokenAsync();
            if (token is not null)
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", token.AccessToken);
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
                    var info = await _authAccess.GetStoredTokenAsync();

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
                    await _authAccess.SetStoredTokenAsync(tokenResponse);

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
