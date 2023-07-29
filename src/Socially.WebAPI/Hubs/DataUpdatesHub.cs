using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.SignalR;
using Socially.Models;
using Socially.Server.DataAccess;
using Socially.Server.Managers;
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
            await using var provider = CreateScopeProvider();
            await provider.RealTimeManager.SubscribeForPostsAsync(Context.ConnectionId, postIds);
        }

        public async Task AddComment(Guid requestId, AddCommentModel comment)
        {
            await using var provider = CreateScopeProvider();
            try
            {
                var createdComment = await provider.PostManager.AddCommentAsync(comment);
                var connectionIds = provider.RealTimeManager.GetPostConnectionIdsAsync(comment.PostId);
                await SendToAllAsync(connectionIds, "CommentAdded", createdComment);
            }
            catch (Exception ex)
            {
                await SendErrorAsync(requestId, ex.Message);
            }
        }

        private Task SendErrorAsync(Guid requestId, string errorMessage)
            => Clients.Client(Context.ConnectionId)
                      .SendAsync("ErrorOccurred", requestId, errorMessage);

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await using var provider = CreateScopeProvider();
            await provider.RealTimeManager.UnsubscribeForConnectionAsync(Context.ConnectionId);
        }

        private async Task SendToAllAsync(IAsyncEnumerable<string> connectionIds, string methodName, object data)
        {
            var processingIds = new List<string>();
            await foreach (var id in connectionIds)
            {
                processingIds.Add(id);
                if (processingIds.Count > 50)
                {
                    await Clients.Clients(processingIds).SendAsync(methodName, data);
                    processingIds.Clear();
                }
            }
            if (processingIds.Any())
                await Clients.Clients(processingIds).SendAsync(methodName, data);
        }

        ManagersProvider CreateScopeProvider() => new(_serviceProvider);

        class ManagersProvider : ScopableServiceProvider
        {

            public IPostManager PostManager => GetService<IPostManager>();

            public IRealTimeManager RealTimeManager => GetService<IRealTimeManager>();

            public ManagersProvider(IServiceProvider serviceProvider) : base(serviceProvider)
            {
            }

        }


    }
}
