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

        public Task ListenForPostsAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default)
        {
            var tags = ids.Select(id => $"post_{id}").ToList();
            return _stateManager.RegisterAsync(tags, Context.ConnectionId, cancellationToken);
        }

        public override Task OnDisconnectedAsync(Exception exception) => _stateManager.UnregisterAsync(Context.ConnectionId);

    }
}
