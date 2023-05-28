using Microsoft.AspNetCore.SignalR;
using Socially.Server.Managers;

namespace Socially.WebAPI.Hubs
{
    public class DataUpdatesHub : Hub
    {
        private readonly ISignalRStateManager _stateManager;

        public DataUpdatesHub(ISignalRStateManager stateManager)
        {
            _stateManager = stateManager;
        }

    }
}
