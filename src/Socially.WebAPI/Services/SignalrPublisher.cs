using Microsoft.AspNetCore.SignalR;
using Socially.Models;
using Socially.Server.Managers;
using Socially.WebAPI.Hubs;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Socially.WebAPI.Services
{
    public class SignalrPublisher
    {
        private readonly ISignalRStateManager _stateManager;
        private readonly IHubContext<DataUpdatesHub> _hubContext;

        public SignalrPublisher(ISignalRStateManager stateManager, IHubContext<DataUpdatesHub> hubContext)
        {
            _stateManager = stateManager;
            _hubContext = hubContext;
        }

        public async Task PublishPostAsync(PostDisplayModel post, CancellationToken cancellationToken = default)
        {
            string tag = $"post_{post.Id}";
            int limit = 20;

            var connectionIds = _stateManager.GetConnectionIdsAsync(tag, limit);
            var ids = new List<string>();

            await foreach (var connectionId in connectionIds)
            {
                ids.Add(connectionId);
                if (ids.Count % limit == 0)
                {
                    await _hubContext.Clients.Clients(ids).SendAsync("PostUpdated", post, CancellationToken.None);
                    ids.Clear();
                }
            }
            if (ids.Any())
                await _hubContext.Clients.Clients(ids).SendAsync("PostUpdated", post, CancellationToken.None);

        }

    }
}
