using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using Socially.Models;
using Socially.Server.DataAccess;
using Socially.Server.Entities;
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

        #region Post


        public async Task ListenToPosts(IEnumerable<int> postIds)
        {
            await using var provider = CreateHubScope(null);
            await provider.RealTimeManager.SubscribeForPostsAsync(Context.ConnectionId, postIds);
        }

        public async Task DeletePost(Guid requestId, int postId)
        {
            await using var scope = CreateHubScope(requestId);
            try
            {
                await scope.PostManager.DeleteAsync(postId);
                var connectionIds = scope.RealTimeManager.GetPostConnectionIdsAsync(postId);
                await SendToAllAsync(connectionIds, c => c.SendAsync("PostDeleted", postId));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete post");
                await scope.SendErrorAsync("Failed to delete post");
            }
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

        public async Task LikePostOrComment(Guid requestId, int postId, int? commentId)
        {
            await using var scope = CreateHubScope(requestId);
            try
            {
                int likeCount = await scope.PostManager.SwapLikeAsync(postId, commentId);
                var connectionIds = scope.RealTimeManager.GetPostConnectionIdsAsync(postId);
                await SendToAllAsync(connectionIds, c => c.SendAsync("Liked", postId, commentId, likeCount));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to like");
                await scope.SendErrorAsync("Failed to like");
            }
        }

        #endregion

        #region User

        public async Task ListenToUsers(IEnumerable<int> userIds)
        {
            await using var provider = CreateHubScope(null);
            await provider.RealTimeManager.SubscribeForUsersAsync(Context.ConnectionId, userIds);
        }

        public async Task UpdateUser(Guid requestId, ProfileUpdateModel model)
        {
            await using var scope = CreateHubScope(requestId);
            try
            {
                await scope.UserService.UpdateProfileAsync(model);
                int userId = Context.User.GetUserId();
                var user = await scope.UserProfileManager.GetUserAsync(userId);
                var userIds = scope.RealTimeManager.GetUserConnectionIdsAsync(userId);
                await SendToAllAsync(userIds, c => c.SendAsync("UserUpdated", user));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update user");
                await scope.SendErrorAsync("Failed to update user");
            }
        }



        #endregion
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
