using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.SignalR;
using Socially.Server.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Socially.WebAPI.Hubs
{

    public class DataUpdatesHub : Hub
    {
        private readonly ISignalRStateManager _stateManager;

        public DataUpdatesHub(ISignalRStateManager stateManager)
        {
            _stateManager = stateManager;
        }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public async Task ListenForPosts2(IEnumerable<int> ids)
        {
            var tags = ids.Select(id => $"post_{id}").ToList();
            await _stateManager.RegisterAsync(tags, Context.ConnectionId);
        }

        public override Task OnDisconnectedAsync(Exception exception) => _stateManager.UnregisterAsync(Context.ConnectionId);

    }
}
