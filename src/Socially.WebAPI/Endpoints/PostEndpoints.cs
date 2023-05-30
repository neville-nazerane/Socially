using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Socially.Models;
using Socially.WebAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Socially.Server.Managers;
using System.Xml.XPath;
using System;
using Socially.Server.Entities;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Socially.WebAPI.Services;

namespace Socially.WebAPI.Endpoints
{
    public class PostEndpoints : EndpointsBase
    {


        public override IEndpointConventionBuilder Aggregate(RouteHandlerBuilder builder)
        {
            return builder.RequireAuthorization()
                          .WithTags("posts");
        }


        public override IEnumerable<RouteHandlerBuilder> Setup(IEndpointRouteBuilder endpoints)
        {
            return new RouteHandlerBuilder[]
            {

                endpoints.MapPost("/post", AddAsync),
                endpoints.MapDelete("/post/{id}", DeleteAsync),

                endpoints.MapPost("/post/comment", AddCommentAsync),
                endpoints.MapDelete("/post/comment/{id}", DeleteCommentAsync),

                endpoints.MapPut("/post/{postId}/like", SwapLikeAsync),
                endpoints.MapPut("/post/{postId}/comment/{commentId}/like", SwapLikeAsync),

                endpoints.MapGet("/posts/me", GetCurrentUserPostsAsync),
                endpoints.MapGet("/posts/profile/{userId}", GetProfilePostsAsync),
                endpoints.MapGet("/posts/home", GetHomePostsAsync)

            };
        }

        Task<int> AddAsync(IPostManager manager,
                                 AddPostModel addPostModel,
                                        CancellationToken cancellationToken = default)
            => manager.AddAsync(addPostModel, cancellationToken);

        Task DeleteAsync(IPostManager manager,
                         int id,
                         CancellationToken cancellationToken = default)
            => manager.DeleteAsync(id, cancellationToken);

        async Task<int> AddCommentAsync(IPostManager manager,
                                        SignalRPublisher signalRPublisher,
                                        AddCommentModel model,
                                        CancellationToken cancellationToken = default)
        {
            var res = await manager.AddCommentAsync(model, cancellationToken);
            signalRPublisher.EnqueuePost(model.PostId);
            return res;
        }

        async Task DeleteCommentAsync(IPostManager manager,
                                      SignalRPublisher signalRPublisher,
                                      int id,
                                      CancellationToken cancellationToken = default)
        {
            int? postId = await manager.GetPostIdForCommentAsync(id, cancellationToken);
            await manager.DeleteCommentAsync(id, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            if (postId.HasValue) 
                signalRPublisher.EnqueuePost(postId.Value);
        }

        Task<bool> SwapLikeAsync(IPostManager manager,
                           int postId,
                           int? commentId,
                           CancellationToken cancellationToken = default)
            => manager.SwapLikeAsync(postId, commentId, cancellationToken);

        Task<IEnumerable<PostDisplayModel>> GetCurrentUserPostsAsync(IPostManager manager,
                                                                     int pageSize,
                                                                     string since = null,
                                                                     CancellationToken cancellationToken = default)
            => manager.GetCurrentUserPostsAsync(pageSize, since.ToDateTime(), cancellationToken);

        Task<IEnumerable<PostDisplayModel>> GetProfilePostsAsync(IPostManager manager,
                                                                 int userId,
                                                                 int pageSize,
                                                                 string since = null,
                                                                 CancellationToken cancellationToken = default)
            => manager.GetProfilePostsAsync(userId, pageSize, since.ToDateTime(), cancellationToken);

        Task<IEnumerable<PostDisplayModel>> GetHomePostsAsync(IPostManager manager,
                                                              int pageSize,
                                                              string since = null,
                                                              CancellationToken cancellationToken = default)
            => manager.GetHomePostsAsync(pageSize, since.ToDateTime(), cancellationToken);


    }
}
