using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Socially.Models;
using Socially.Server.DataAccess;
using Socially.Server.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Socially.Server.Managers
{
    public class PostsManager
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<PostsManager> _logger;

        public PostsManager(ApplicationDbContext dbContext, 
                            ILogger<PostsManager> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<int> AddAsync(int userId, AddPostModel addPostModel, CancellationToken cancellationToken = default)
        {
            var entity = new Post
            {
                Text = addPostModel.Text,
                CreatorId = userId,
                CreatedOn = DateTime.UtcNow
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
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> AddCommentAsync(int userId, AddCommentModel model, CancellationToken cancellationToken = default)
        {
            var entity = new Comment
            {
                Text = model.Text,
                CreatorId = userId,
                PostId = model.PostId,
                CreatedOn = DateTime.UtcNow,
                ParentCommentId = model.ParentCommentId
            };
            await _dbContext.Comments.AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return entity.Id;
        }

        public async Task DeleteCommentAsync(int userId, int commentId, CancellationToken cancellationToken = default)
        {
            var entity = await _dbContext.Comments.SingleOrDefaultAsync(c => c.CreatorId == userId && c.Id == commentId,
                                                                        cancellationToken);
            if (entity == null)
            {
                _logger.LogWarning("Can't find comment {commentId} for user {userId}", commentId, userId);
                return;
            }
            _dbContext.Comments.Remove(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
