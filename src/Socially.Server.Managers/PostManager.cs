using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Socially.Models;
using Socially.Server.DataAccess;
using Socially.Server.Entities;
using Socially.Server.Managers.Exceptions;
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
    public class PostManager
    {

        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<PostManager> _logger;

        public PostManager(ApplicationDbContext dbContext, 
                            ILogger<PostManager> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<int> AddAsync(int userId,
                                        AddPostModel addPostModel,
                                        CancellationToken cancellationToken = default)
        {
            var entity = new Post
            {
                CreatorId = userId,
                Text = addPostModel.Text,
                CreatedOn = DateTime.UtcNow,
            };

            await _dbContext.Posts.AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return entity.Id;
        }

        public async Task DeleteAsync(int userId, int postId, CancellationToken cancellationToken = default)
        {
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

        public async Task AddCommentAsync(int userId, AddCommentModel model, CancellationToken cancellationToken = default)
        {
            var entity = new Comment
            {
                Text = model.Text,
                CreatorId = userId,
                PostId = model.PostId,
                ParentCommentId = model.ParentCommentId,
                CreatedOn = DateTime.UtcNow
            };
            await _dbContext.AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteCommentAsync(int userId,
                                             int commentId,
                                             CancellationToken cancellationToken = default)
        {
            var comment = await _dbContext.Comments.SingleOrDefaultAsync(s => s.CreatorId == userId && s.Id == commentId,
                                                                   cancellationToken);
            if (comment == null)
            {
                _logger.LogWarning("Did not find comment {commentId} to delete with user {userId}", commentId, userId);
                return;
            }
            _dbContext.Comments.Remove(comment);
            await _dbContext.SaveChangesAsync(CancellationToken.None);
        }

        public async Task<IEnumerable<PostDisplayModel>> GetProfilePostsAsync(int userId, 
                                                                              int pageSize,
                                                                              DateTime? since = null,
                                                                              CancellationToken cancellationToken = default)
        {

            return await ProjectPostAsync(_dbContext.Posts.Where(p => p.CreatorId == userId), 
                                          pageSize, since, cancellationToken);

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
        }

        public async Task<IEnumerable<PostDisplayModel>> GetHomePostsAsync(int userId,
                                                                              int pageSize,
                                                                              DateTime? since = null,
                                                                              CancellationToken cancellationToken = default)
            => await ProjectPostAsync(_dbContext.Posts.Where(p => p.Creator.Friends.Select(f => f.FriendUserId).Contains(userId)),
                                      pageSize, since, cancellationToken);

        async Task<IEnumerable<PostDisplayModel>> ProjectPostAsync(IQueryable<Post> querablePosts,
                                                                   int pageSize,
                                                                   DateTime? since = null,
                                                                   CancellationToken cancellationToken = default)
        {
            var data = await querablePosts.AsNoTracking()
                                          .Where(p => since == null || p.CreatedOn > since)
                                          .OrderBy(p => p.CreatedOn)
                                          .Take(pageSize)
                                                    .Select(p => new {
                                                            Comments = p.Comments.ToArray(),
                                                            Post = new PostDisplayModel
                                                            {
                                                                Id = p.Id,
                                                                Text = p.Text,
                                                                CreatorId = p.CreatorId,
                                                                CreatedOn = p.CreatedOn
                                                            }
                                                        })
                                                        .ToArrayAsync(cancellationToken);

            var allComments = data.SelectMany(r => r.Comments).ToArray();
            var postResults = data.Select(r => r.Post).ToArray();

            foreach (var p in postResults)
                p.Comments = MapComments(allComments.Where(c => c.PostId == p.Id).ToArray()).ToList();

            return postResults;
        }

        private static IEnumerable<DisplayCommentModel> MapComments(IEnumerable<Comment> comments, int? parentId = null)
        {
            var result = new List<DisplayCommentModel>();
            var dictonary = comments.ToDictionary(c => c.Id, c => c.ToDisplayModel());
            foreach (var comment in comments)
            {
                if (comment.ParentCommentId.HasValue && dictonary.ContainsKey(comment.ParentCommentId.Value))
                    dictonary[comment.ParentCommentId.Value].Comments.Add(dictonary[comment.Id]);

                if (comment.ParentCommentId == parentId) result.Add(dictonary[comment.Id]);
            }
            
            return result;
        }

        //private IEnumerable<Comment> FlattenComments(IEnumerable<Comment> comments)
        //{
        //    return comments.Union(
        //            FlattenComments(comments.Where(c => c.Comments?.Any() == true)
        //                                    .SelectMany(c => c.Comments))
        //        );
        //}

    }

}
