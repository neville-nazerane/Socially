using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Socially.Models;
using Socially.Server.DataAccess;
using Socially.Server.Managers;
using Socially.Server.Managers.Utils;
using Socially.WebAPI.Services;
using Socially.WebAPI.Utils;
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

        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DataUpdatesHub> _logger;

        public DataUpdatesHub(IServiceProvider serviceProvider, ILogger<DataUpdatesHub> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task ListenToPosts(IEnumerable<int> postIds)
        {
            await using var provider = CreateHubScope(null);
            await provider.RealTimeManager.SubscribeForPostsAsync(Context.ConnectionId, postIds);
        }

        public async Task AddComment(Guid requestId, AddCommentModel comment)
        {
            await using var scope = CreateHubScope(requestId);
            try
            {
                var createdComment = await scope.PostManager.AddCommentAsync(comment);
                var connectionIds = scope.RealTimeManager.GetPostConnectionIdsAsync(comment.PostId);
                await SendToAllAsync(connectionIds, c => c.SendAsync("CommentAdded", comment.PostId, comment.ParentCommentId, createdComment));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add comment");
                await scope.SendErrorAsync("Failed to add comment");
            }
        }

        public async Task DeleteComment(Guid requestId, int commentId)
        {
            await using var scope = CreateHubScope(requestId);
            try
            {
                var comment = await scope.PostManager.DeleteCommentAsync(commentId);
                var connectionIds = scope.RealTimeManager.GetPostConnectionIdsAsync(comment.PostId.Value);
                await SendToAllAsync(connectionIds, c => c.SendAsync("CommentDeleted", commentId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete comment");
                await scope.SendErrorAsync("Failed to delete comment");
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await using var provider = CreateHubScope(null);
            await provider.RealTimeManager.UnsubscribeForConnectionAsync(Context.ConnectionId);
        }

        private async Task SendToAllAsync(IAsyncEnumerable<string> connectionIds, Func<IClientProxy, Task> sendFunc)
        {
            // send to current first
            var currentId = Context.ConnectionId;
            await sendFunc(Clients.Client(currentId));
            var processingIds = new List<string>();
            await foreach (var id in connectionIds)
            {
                if (id != currentId) processingIds.Add(id);
                if (processingIds.Count > 50)
                {
                    await sendFunc(Clients.Clients(processingIds));
                    processingIds.Clear();
                }
            }
            if (processingIds.Any())
                await sendFunc(Clients.Clients(processingIds));
        }

        HubScope CreateHubScope(Guid? requestId) => new(_serviceProvider, this, requestId);




    }
}
