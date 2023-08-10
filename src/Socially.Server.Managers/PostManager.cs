using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Socially.Models;
using Socially.Server.DataAccess;
using Socially.Server.Entities;
using Socially.Server.Managers.Exceptions;
using Socially.Server.Managers.Utils;
using Socially.Server.ModelMappings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Comment = Socially.Server.Entities.Comment;

namespace Socially.Server.Managers
{
    public class PostManager : IPostManager
    {

        private readonly ApplicationDbContext _dbContext;
        private readonly CurrentContext _currentContext;
        private readonly ILogger<PostManager> _logger;

        public PostManager(ApplicationDbContext dbContext,
                           CurrentContext currentContext,
                            ILogger<PostManager> logger)
        {
            _dbContext = dbContext;
            _currentContext = currentContext;
            _logger = logger;
        }

        public async Task<int> AddAsync(AddPostModel addPostModel,
                                        CancellationToken cancellationToken = default)
        {
            var entity = new Post
            {
                CreatorId = _currentContext.UserId,
                Text = addPostModel.Text,
                CreatedOn = DateTime.UtcNow,
            };

            await _dbContext.Posts.AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return entity.Id;
        }

        public Task<int?> GetPostIdForCommentAsync(int commentId, CancellationToken cancellationToken = default)
            => _dbContext.Comments
                         .Where(c => c.Id == commentId)
                         .Select(c => c.PostId)
                         .SingleOrDefaultAsync(cancellationToken);

        public async Task DeleteAsync(int postId, CancellationToken cancellationToken = default)
        {
            int userId = _currentContext.UserId;
            var post = await _dbContext.Posts.SingleOrDefaultAsync(s => s.CreatorId == userId && s.Id == postId,
                                                                   cancellationToken);
            if (post == null)
            {
                _logger.LogWarning("Did not find post {postId} to delete with user {userId}", postId, userId);
                return;
            }
            _dbContext.Posts.Remove(post);

            await _dbContext.SaveChangesAsync(CancellationToken.None);
        }

        public async Task<DisplayCommentModel> AddCommentAsync(AddCommentModel model, CancellationToken cancellationToken = default)
        {
            var entity = new Comment
            {
                Text = model.Text,
                CreatorId = _currentContext.UserId,
                PostId = model.PostId,
                ParentCommentId = model.ParentCommentId,
                CreatedOn = DateTime.UtcNow
            };
            await _dbContext.AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return entity.ToDisplayModel();
        }

        public async Task<Comment> DeleteCommentAsync(int commentId,
                                                                  CancellationToken cancellationToken = default)
        {
            int userId = _currentContext.UserId;
            var comment = await _dbContext.Comments.SingleOrDefaultAsync(s => s.CreatorId == userId && s.Id == commentId,
                                                                        cancellationToken);
            if (comment == null)
            {
                _logger.LogWarning("Did not find comment {commentId} to delete with user {userId}", commentId, userId);
                return null;
            }
            _dbContext.Comments.Remove(comment);
            await _dbContext.SaveChangesAsync(CancellationToken.None);
            return comment;
        }

        public async Task<int> SwapLikeAsync(int postId,
                                              int? commentId,
                                              CancellationToken cancellationToken = default)
        {
            int userId = _currentContext.UserId;
            bool removal = false;
            int likeCount = 0;
            var existing = await _dbContext.PostLikes.SingleOrDefaultAsync(l => l.UserId == userId && l.PostId == postId && l.CommentId == commentId, cancellationToken);
            if (existing is not null)
            {
                removal = true;
                _dbContext.PostLikes.Remove(existing);
            }
            else
            {
                await _dbContext.PostLikes.AddAsync(new PostLike
                {
                    UserId = userId,
                    PostId = postId,
                    CommentId = commentId,
                    CreatedOn = DateTime.UtcNow,
                }, cancellationToken);
            }

            if (commentId == null)
            {
                var post = await _dbContext.Posts.SingleOrDefaultAsync(p => p.Id == postId, cancellationToken);
                post.LikeCount = (post.LikeCount ?? 0) + (removal ? -1 : 1);
                likeCount = post.LikeCount.GetValueOrDefault();
                if (post.LikeCount < 1) post.LikeCount = null;
            }
            else
            {
                var comment = await _dbContext.Comments.SingleOrDefaultAsync(p => p.Id == commentId, cancellationToken);
                comment.LikeCount = (comment.LikeCount ?? 0) + (removal ? -1 : 1);
                likeCount = comment.LikeCount.GetValueOrDefault();
                if (comment.LikeCount < 1) comment.LikeCount = null;
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
            return likeCount;
        }

        public Task<IEnumerable<PostDisplayModel>> GetCurrentUserPostsAsync(int pageSize,
                                                                              DateTime? since = null,
                                                                              CancellationToken cancellationToken = default)
            => GetProfilePostsAsync(_currentContext.UserId, pageSize, since, cancellationToken);

        public async Task<IEnumerable<PostDisplayModel>> GetProfilePostsAsync(int userId,
                                                                              int pageSize,
                                                                              DateTime? since = null,
                                                                              CancellationToken cancellationToken = default)
            => await ProjectPostAsync(_dbContext.Posts.Where(p => p.CreatorId == userId),
                                          pageSize,
                                          since,
                                          cancellationToken);

        //var data = await _dbContext.Posts.AsNoTracking()
        //                                .Where(p => p.CreatorId == userId && (since == null || p.CreatedOn > since))
        //                                .Take(pageSize)
        //                                .Select(p => new
        //                                {
        //                                    Comments = p.Comments.ToArray(),
        //                                    Post = new PostDisplayModel
        //                                    {
        //                                        Id = p.Id,
        //                                        Text = p.Text,
        //                                        CreatorId = p.CreatorId,
        //                                        CreatedOn = p.CreatedOn
        //                                    }
        //                                })
        //                                .ToArrayAsync(cancellationToken);

        //var allComments = data.SelectMany(r => r.Comments).ToArray();

        //var postResults = data.Select(r => r.Post).ToArray();

        //foreach (var p in postResults)
        //    p.Comments = MapComments(allComments.Where(c => c.PostId == p.Id).ToArray()).ToList();

        //return postResults;

        public async Task<IEnumerable<PostDisplayModel>> GetHomePostsAsync(int pageSize,
                                                                           DateTime? since = null,
                                                                           CancellationToken cancellationToken = default)
            => await ProjectPostAsync(_dbContext.Posts.Where(p => p.Creator.Friends.Select(f => f.FriendUserId).Contains(_currentContext.UserId)),
                                      pageSize, since, cancellationToken);

        public async Task<PostDisplayModel> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var queryable = _dbContext.Posts.Where(p => p.Id == id);
            var results = await ProjectPostAsync(queryable, 1, null, cancellationToken);
            return results.SingleOrDefault();
        }

        async Task<IEnumerable<PostDisplayModel>> ProjectPostAsync(IQueryable<Post> querablePosts,
                                                                   int pageSize,
                                                                   DateTime? since = null,
                                                                   CancellationToken cancellationToken = default)
        {
            var data = querablePosts.AsNoTracking()
                                          .Where(p => since == null || p.CreatedOn > since)
                                          .OrderBy(p => p.CreatedOn)
                                          .Take(pageSize)
                                                    .Select(p => new
                                                    {
                                                        Comments = p.Comments.ToArray(),
                                                        CommentLikes = p.Comments.Select(c => new
                                                        {
                                                            c.Id,
                                                            IsLikedByCurrentUser = c.Likes.Any(l => l.UserId == _currentContext.UserId)
                                                        }),
                                                        Post = new PostDisplayModel
                                                        {
                                                            Id = p.Id,
                                                            Text = p.Text,
                                                            CreatorId = p.CreatorId.Value,
                                                            CreatedOn = p.CreatedOn,
                                                            LikeCount = p.Likes.Count(),
                                                            IsLikedByCurrentUser = p.Likes.Any(l => l.UserId == _currentContext.UserId)
                                                        }
                                                    })
                                                    .AsAsyncEnumerable();

            List<PostDisplayModel> postResults = new();
            List<Comment> allComments = new();
            Dictionary<int, bool> likeMapping = new();
            await foreach (var item in data)
            {
                cancellationToken.ThrowIfCancellationRequested();
                postResults.Add(item.Post);
                allComments.AddRange(item.Comments);
                foreach (var mapping in item.CommentLikes)
                    likeMapping.Add(mapping.Id, mapping.IsLikedByCurrentUser);
            }

            foreach (var p in postResults)
                p.Comments = MapComments(allComments.Where(c => c.PostId == p.Id).ToArray(), likeMapping).ToList();

            return postResults;
        }

        static IEnumerable<DisplayCommentModel> MapComments(IEnumerable<Comment> comments,
                                                            Dictionary<int, bool> likeMapping,
                                                            int? parentId = null)
        {
            var result = new List<DisplayCommentModel>();
            var dictonary = comments.ToDictionary(c => c.Id, c => c.ToDisplayModel(likeMapping[c.Id]));
            foreach (var comment in comments)
            {
                if (comment.ParentCommentId.HasValue && dictonary.ContainsKey(comment.ParentCommentId.Value))
                    dictonary[comment.ParentCommentId.Value].Comments.Add(dictonary[comment.Id]);

                if (comment.ParentCommentId == parentId) result.Add(dictonary[comment.Id]);
            }
            return result;
        }

    }

}
