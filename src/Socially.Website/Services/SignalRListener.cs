using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Socially.Apps.Consumer.Services;
using Socially.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Socially.Website.Services
{
    public class SignalRListener
    {
        private readonly IAuthAccess _authAccess;
        private readonly ICacheUpdater _cacheUpdater;
        private readonly HubConnection _dataUpdateConn;

        public SignalRListener(IAuthAccess authAccess,
                               ICacheUpdater cacheUpdater,
                               IConfiguration configuration)
        {
            _authAccess = authAccess;
            _cacheUpdater = cacheUpdater;
            _dataUpdateConn = new HubConnectionBuilder().WithUrl($"{configuration["baseURL"]}/hubs/dataUpdates", o => o.AccessTokenProvider = GetToken)
                                                        .WithAutomaticReconnect()
                                                        .Build();
            SetupListeners();

            _dataUpdateConn.Reconnected += Reconnected;
        }

        public void StartListeners()
        {
            // TODO event needs to be abstracted to IAuthAccess when service is used with MAUI
            ((AuthProvider)_authAccess).AuthenticationStateChanged += SignalRListener_AuthenticationStateChanged;
        }

        private async void SignalRListener_AuthenticationStateChanged(Task<AuthenticationState> task)
        {
            try
            {
                var state = await task;
                if (state.User is null)
                    await _dataUpdateConn.StopAsync();
                else
                    await _dataUpdateConn.StartAsync();
            }
            catch
            {
                System.Console.WriteLine("Failed to listen");
            }
        }

        private Task Reconnected(string arg)
        {
            return Task.CompletedTask;
        }

        private async Task<string> GetToken()
        {
            try
            {
                var info = await _authAccess.GetStoredTokenAsync();
                return info.AccessToken;
            }
            catch
            {
                return null;
            }
        }

        public async Task InitAsync()
        {
            try
            {
                await _dataUpdateConn.StartAsync();
            }
            catch
            {
                System.Console.WriteLine("FAILED signalr");
            }
        }

        void SetupListeners()
        {
            _dataUpdateConn.On<PostDisplayModel>("PostUpdated", _cacheUpdater.UpdatePostAsync);
        }

        public Task ListenForPostsAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default) 
            => _dataUpdateConn.InvokeAsync("ListenForPostsAsync", string.Join(",", ids), cancellationToken);

    }
}
