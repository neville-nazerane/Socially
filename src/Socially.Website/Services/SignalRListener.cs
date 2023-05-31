using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Socially.Apps.Consumer.Services;
using Socially.Models;
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
            _dataUpdateConn.Reconnected += Reconnected;
        }

        private Task Reconnected(string arg)
        {
            return Task.CompletedTask;
        }

        private async Task<string> GetToken()
        {
            var info = await _authAccess.GetStoredTokenAsync();
            return info.AccessToken;
        }

        public Task InitAsync()
        {
            SetupListeners();
            return _dataUpdateConn.StartAsync();
        }

        void SetupListeners()
        {
            _dataUpdateConn.On<PostDisplayModel>("PostUpdated", _cacheUpdater.UpdatePostAsync);
        }
    }
}
