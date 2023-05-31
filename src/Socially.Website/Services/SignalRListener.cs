﻿using Microsoft.AspNetCore.Components.Authorization;
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
            var state = await task;
            if (state.User is null)
                await _dataUpdateConn.StopAsync();
            else
                await _dataUpdateConn.StartAsync();
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

        //public Task InitAsync()
        //{
        //    return _dataUpdateConn.StartAsync();
        //}

        void SetupListeners()
        {
            _dataUpdateConn.On<PostDisplayModel>("PostUpdated", _cacheUpdater.UpdatePostAsync);
        }
    }
}
