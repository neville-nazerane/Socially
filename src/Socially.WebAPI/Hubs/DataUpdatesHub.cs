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

        public async Task AddComment(AddCommentModel comment)
        {
            await using var provider = CreateScopeProvider();
            var commentId = await provider.PostManager.AddCommentAsync(comment);
            var connectionIds = provider.RealTimeManager.GetPostConnectionIdsAsync(comment.PostId);
            var processingIds = new List<string>();
            await foreach (var id in connectionIds)
            {
                processingIds.Add(id);
                if (processingIds.Count > 30)
                {
                    await Clients.Clients(processingIds).SendAsync("CommentAdded");
                    processingIds.Clear();
                }
            }
            if (processingIds.Any())
                await Clients.Clients(processingIds).SendAsync("CommentAdded");
        }

        Task RunOnPostManagerAsync(Func<IPostManager, IRealTimeManager, Task> func)
            => _serviceProvider.RunScopedLogicAsync(func);

        ManagersProvider CreateScopeProvider() => new(_serviceProvider);

        class ManagersProvider : ScopableServiceProvider
        {

            public IPostManager PostManager => GetService<IPostManager>();

            public IRealTimeManager RealTimeManager => GetService<IRealTimeManager>();

            public ManagersProvider(IServiceProvider serviceProvider) : base(serviceProvider)
            {
            }

        }

        //public async Task ListenForPosts2(IEnumerable<int> ids)
        //{
        //    var tags = ids.Select(id => $"post_{id}").ToList();
        //    await _stateManager.RegisterAsync(tags, Context.ConnectionId);
        //}

        //public override Task OnDisconnectedAsync(Exception exception) => _stateManager.UnregisterAsync(Context.ConnectionId);

    }
}
