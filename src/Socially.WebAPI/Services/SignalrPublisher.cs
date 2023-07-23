using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Socially.Models;
using Socially.Server.Entities;
using Socially.Server.Managers;
using Socially.WebAPI.Hubs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Socially.WebAPI.Services
{
    public class SignalRPublisher : ISignalRPublisher
    {
        private readonly ISignalRStateManager _stateManager;
        private readonly ILogger<SignalRPublisher> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IHubContext<DataUpdatesHub> _hubContext;

        private readonly ConcurrentQueue<QueueContext> _queue;

        public SignalRPublisher(ISignalRStateManager stateManager,
                                ILogger<SignalRPublisher> logger,
                                IServiceProvider serviceProvider,
                                IHubContext<DataUpdatesHub> hubContext)
        {
            _queue = new();
            _stateManager = stateManager;
            _logger = logger;
            _serviceProvider = serviceProvider;
            _hubContext = hubContext;
        }

        public void EnqueuePost(int id) => _queue.Enqueue(new(id, QueuedItemType.Post));

        public async Task KeepRunningAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await KeepDequeueingAsync(cancellationToken);
            }
            catch (Exception ex) when (ex is not TaskCanceledException)
            {
                _logger.LogError(ex, "Failed pub sub runner");
                await KeepDequeueingAsync(cancellationToken);
            }
        }

        async Task KeepDequeueingAsync(CancellationToken cancellationToken = default)
        {
            int deQueueLimit = 10;
            while (!cancellationToken.IsCancellationRequested)
            {
                for (int i = 0; i < deQueueLimit; i++)
                {
                    if (!_queue.TryDequeue(out QueueContext context))
                        continue;

                    switch (context.Type)
                    {
                        case QueuedItemType.Post:
                            await PublishPostAsync(context.Id, cancellationToken);
                            break;
                    }

                }
                await Task.Delay(1000, cancellationToken);
            }
        }

        private async Task PublishPostAsync(int id, CancellationToken cancellationToken)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var postManager = scope.ServiceProvider.GetService<IPostManager>();
            var post = await postManager.GetByIdAsync(id, cancellationToken);
            await PublishForTagAsync($"post_{post.Id}", 20, "PostUpdated", post);
        }

        async Task PublishForTagAsync(string tag,
                                      int pageLimit,
                                      string methodName,
                                      object data)
        {

            var connectionIds = _stateManager.GetConnectionIdsAsync(tag, pageLimit);
            var ids = new List<string>();

            await foreach (var connectionId in connectionIds)
            {
                ids.Add(connectionId);
                if (ids.Count % pageLimit == 0)
                {
                    await _hubContext.Clients.Clients(ids).SendAsync(methodName, data, CancellationToken.None);
                    ids.Clear();
                }
            }
            if (ids.Any())
                await _hubContext.Clients.Clients(ids).SendAsync(methodName, data, CancellationToken.None);
        }

        record QueueContext(int Id, QueuedItemType Type);

        enum QueuedItemType
        {
            Post, User
        }

    }
}
