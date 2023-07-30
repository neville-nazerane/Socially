using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.SignalR;
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

        public DataUpdatesHub(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
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
                await SendToAllAsync(connectionIds, "CommentAdded", createdComment);
            }
            catch (Exception ex)
            {
                await scope.SendErrorAsync("Failed to add comment");
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await using var provider = CreateHubScope(null);
            await provider.RealTimeManager.UnsubscribeForConnectionAsync(Context.ConnectionId);
        }

        private async Task SendToAllAsync(IAsyncEnumerable<string> connectionIds, string methodName, object data)
        {
            // send to current first
            var currentId = Context.ConnectionId;
            await Clients.Client(currentId).SendAsync(methodName, data);
            var processingIds = new List<string>();
            await foreach (var id in connectionIds)
            {
                if (id != currentId) processingIds.Add(id);
                if (processingIds.Count > 50)
                {
                    await Clients.Clients(processingIds).SendAsync(methodName, data);
                    processingIds.Clear();
                }
            }
            if (processingIds.Any())
                await Clients.Clients(processingIds).SendAsync(methodName, data);
        }

        HubScope CreateHubScope(Guid? requestId) => new(_serviceProvider, this, requestId);




    }
}
