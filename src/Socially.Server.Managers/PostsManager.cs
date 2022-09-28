using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Socially.Models;
using Socially.Server.DataAccess;
using Socially.Server.Entities;
using Socially.Server.Managers.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Comment = Socially.Server.Entities.NoSql.Comment;

namespace Socially.Server.Managers
{
    public class PostsManager
    {
        private const string containerName = "posts";

        private readonly ApplicationDbContext _dbContext;
        private readonly IBlobAccess _blobAccess;
        private readonly ILogger<PostsManager> _logger;

        public PostsManager(ApplicationDbContext dbContext, 
                            IBlobAccess blobAccess,
                            ILogger<PostsManager> logger)
        {
            _dbContext = dbContext;
            _blobAccess = blobAccess;
            _logger = logger;
        }

        public Task InitAsync(CancellationToken cancellationToken = default)
            => _blobAccess.CreateContainerIfNotExistAsync(containerName,
                                                          Azure.Storage.Blobs.Models.PublicAccessType.None,
                                                          cancellationToken);

        public async Task<Guid> AddAsync(int userId,
                                        AddPostModel addPostModel,
                                        CancellationToken cancellationToken = default)
        {
            var entity = new Post
            {
                Id = Guid.NewGuid(),
                CreatorId = userId,
                CreatedOn = DateTime.UtcNow
            };

            await Task.WhenAll(new Task[]
            {
                _dbContext.Posts.AddAsync(entity, cancellationToken).AsTask(),

                _blobAccess.UploadAsync(containerName,
                                              entity.Id.ToString(),
                                              new Entities.NoSql.Post
                                              {
                                                  CreatedOn = entity.CreatedOn.Value,
                                                  CreatorId = userId,
                                                  Text = addPostModel.Text
                                              }, cancellationToken)
            });

            await _dbContext.SaveChangesAsync(cancellationToken);
            return entity.Id;
        }

        public async Task DeleteAsync(int userId, Guid postId, CancellationToken cancellationToken = default)
        {
            var post = await _dbContext.Posts.SingleOrDefaultAsync(s => s.CreatorId == userId && s.Id == postId,
                                                                   cancellationToken);
            if (post == null)
            {
                _logger.LogWarning("Did not find post {postId} to delete with user {userId}", postId, userId);
                return;
            }
            _dbContext.Posts.Remove(post);

            await Task.WhenAll(new Task[]
            {
                _dbContext.SaveChangesAsync(CancellationToken.None),
                _blobAccess.DeleteAsync(containerName, post.Id.ToString(), CancellationToken.None)
            });
        }

        public async Task AddCommentAsync(int userId, AddCommentModel model, CancellationToken cancellationToken = default)
        {
            var entity = new Comment
            {
                Id = Guid.NewGuid(),
                Text = model.Text,
                CreatorId = userId,
                PostId = model.PostId,
                CreatedOn = DateTime.UtcNow
            };

            var post = await _blobAccess.DownloadAsync<Entities.NoSql.Post>(containerName,
                                                                            model.PostId.ToString(),
                                                                            cancellationToken);
            if (model.ParentCommentId is null)
            {
                post.Comments ??= Array.Empty<Comment>();
                post.Comments = post.Comments.Union(new Comment[] { entity });
            }
            else
            {
                var parent = FlattenComments(post.Comments).SingleOrDefault(c => c.Id == model.ParentCommentId);
                throw new RequiredDataNotFoundException(model.ParentCommentId, "comment");
            }

            await _blobAccess.DeleteAsync(containerName, model.PostId.ToString(), cancellationToken);
            await _blobAccess.UploadAsync(containerName, model.PostId.ToString(), post, CancellationToken.None);
        }

        private IEnumerable<Comment> FlattenComments(IEnumerable<Comment> comments)
        {
            return comments.Union(  
                    FlattenComments(comments.Where(c => c.Comments?.Any() == true)
                                            .SelectMany(c => c.Comments))
                );
        }

        public async Task DeleteCommentAsync(int userId,
                                             Guid postId,
                                             Guid commentId,
                                             CancellationToken cancellationToken = default)
        {

        }
    }
}
