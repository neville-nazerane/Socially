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

namespace Socially.WebAPI.Endpoints
{
    public class PostEndpoints : EndpointsBase
    {
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
                endpoints.MapGet("/posts/home", GetHomePostsAsync),

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

        Task AddCommentAsync(IPostManager manager,
                            AddCommentModel model,
                            CancellationToken cancellationToken = default)
            => manager.AddCommentAsync(model, cancellationToken);

        Task DeleteCommentAsync(IPostManager manager,
                                int id,
                                CancellationToken cancellationToken = default)
            => manager.DeleteCommentAsync(id, cancellationToken);

        Task<bool> SwapLikeAsync(IPostManager manager,
                           int postId,
                           int? commentId,
                           CancellationToken cancellationToken = default)
            => manager.SwapLikeAsync(postId, commentId, cancellationToken);

        Task<IEnumerable<PostDisplayModel>> GetCurrentUserPostsAsync(IPostManager manager,
                                                                     int pageSize,
                                                                     DateTime? since = null,
                                                                     CancellationToken cancellationToken = default)
            => manager.GetCurrentUserPostsAsync(pageSize, since, cancellationToken);

        Task<IEnumerable<PostDisplayModel>> GetProfilePostsAsync(IPostManager manager,
                                                                 int userId,
                                                                 int pageSize,
                                                                 DateTime? since = null,
                                                                 CancellationToken cancellationToken = default)
            => manager.GetProfilePostsAsync(userId, pageSize, since, cancellationToken);

        Task<IEnumerable<PostDisplayModel>> GetHomePostsAsync(IPostManager manager,
                                                              int pageSize,
                                                              DateTime? since = null,
                                                              CancellationToken cancellationToken = default)
            => manager.GetHomePostsAsync(pageSize, since, cancellationToken);


    }
}
