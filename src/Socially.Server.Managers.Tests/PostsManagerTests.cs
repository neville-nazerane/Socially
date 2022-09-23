using Azure.Storage.Blobs.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Moq;
using Socially.Models;
using Socially.Server.DataAccess;
using Socially.Server.Entities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Socially.Server.Managers.Tests
{
    public class PostsManagerTests : TestBase
    {
        private Mock<ILogger<PostsManager>> loggerMock;
        private PostsManager manager;

        private Task SetupManagerAsync()
        {
            loggerMock = new Mock<ILogger<PostsManager>>();
            manager = new PostsManager(DbContext, loggerMock.Object);
            return Task.CompletedTask;
        }

        [Fact]
        public async Task Add_ValidPost_AddsPost()
        {
            // ARRANGE
            await SetupManagerAsync();

            // ACT
            await manager.AddAsync(10, new AddPostModel
            {
                Text = "Sample post"
            }, CancellationToken.None);
            
            // ASSERT
            var storedPosts = await DbContext.Posts.Where(p => p.CreatorId == 10).ToListAsync();
            Assert.NotEmpty(storedPosts);
            Assert.Single(storedPosts);
            Assert.NotNull(storedPosts.First().CreatedOn);

        }

        [Fact]
        public async Task Remove_NonExistingPost_Warns() 
        {
            // ARRANGE
            await SetupManagerAsync();
            var post = new Post
            {
                CreatedOn = DateTime.Now,
                Text = "simple",
                CreatorId = 10
            };
            await DbContext.Posts.AddAsync(post);
            await DbContext.SaveChangesAsync();
            int invalidId = post.Id + 1;

            // ACT
            await manager.DeleteAsync(10, invalidId, CancellationToken.None);


            // ASSERT
            var posts = await DbContext.Posts.ToListAsync();
            Assert.NotEmpty(posts);
            Assert.Single(posts);
            loggerMock.VerifyLog(l => l.LogWarning(It.IsAny<string>(),
                                                   It.IsIn(30),
                                                   It.IsIn(invalidId)),
                                Times.Once);

        }

        [Fact]
        public async Task Remove_ExistingPostInvalidUser_Warns()
        {
            // ARRANGE
            await SetupManagerAsync();
 
            var post = new Post
            {
                CreatedOn = DateTime.Now,
                Text = "simple",
                CreatorId = 10
            };
            await DbContext.Posts.AddAsync(post);
            await DbContext.SaveChangesAsync();
            int postId = post.Id;

            // ACT
            await manager.DeleteAsync(30, postId, CancellationToken.None);

            // ASSERT
            var posts = await DbContext.Posts.ToListAsync();
            Assert.NotEmpty(posts);
            Assert.Single(posts);
            loggerMock.VerifyLog(l => l.LogWarning(It.IsAny<string>(),
                                                   It.IsIn(30),
                                                   It.IsIn(postId)),
                                Times.Once);

        }

        [Fact]
        public async Task Remove_ExistingPostValidUser_Removes()
        {
            // ARRANGE
            await SetupManagerAsync();

            var post = new Entities.Post
            {
                CreatedOn = DateTime.Now,
                Text = "simple",
                CreatorId = 10
            };
            await DbContext.Posts.AddAsync(post);
            await DbContext.SaveChangesAsync();
            int postId = post.Id;

            // ACT
            await manager.DeleteAsync(10, postId, CancellationToken.None);

            // ASSERT
            var posts = await DbContext.Posts
                                       .Where(p => p.CreatorId == postId)
                                       .ToListAsync();
            Assert.Empty(posts);
            loggerMock.VerifyLog(l => l.LogWarning(It.IsAny<string>()), Times.Never);

        }

        [Fact]
        public async Task AddComment_ValidComment_AddsComment()
        {
            // ARRANGE
            await SetupManagerAsync();

            // ACT
            await manager.AddCommentAsync(11, new AddCommentModel
            {
                ParentCommentId = 20,
                PostId = 2,
                Text = "bye"
            }, CancellationToken.None);

            // ASSERT
            var comments = await DbContext.Comments.ToListAsync();

            Assert.NotEmpty(comments);
            Assert.Single(comments);

        }

        [Fact]
        public async Task DeleteComment_InvalidCommentValidUser_Warns()
        {
            // ARRANGE
            await SetupManagerAsync();
            var comment = new Comment
            {
                Text = "my little comment",
                PostId = 50,
                ParentCommentId = 20,
                CreatorId = 10,
                CreatedOn = DateTime.UtcNow
            };
            await DbContext.Comments.AddAsync(comment);
            await DbContext.SaveChangesAsync();
            int invalidCommentId = comment.Id + 1;

            // ACT
            await manager.DeleteCommentAsync(10, invalidCommentId, CancellationToken.None);

            // ASSERT
            var comments = await DbContext.Comments.ToListAsync();
            Assert.NotEmpty(comments);
            Assert.Single(comments);
            loggerMock.VerifyLog(l => l.LogWarning(It.IsAny<string>(),
                                                   It.IsIn(invalidCommentId),
                                                   It.IsIn(10)),
                                Times.Once);
        }

        [Fact]
        public async Task DeleteComment_ValidCommitInvalidUser_Warns()
        {
            // ARRANGE
            await SetupManagerAsync();
            var comment = new Comment
            {
                Text = "my little comment",
                PostId = 50,
                ParentCommentId = 20,
                CreatorId = 10,
                CreatedOn = DateTime.UtcNow
            };
            await DbContext.Comments.AddAsync(comment);
            await DbContext.SaveChangesAsync();
            int commentId = comment.Id;

            // ACT
            await manager.DeleteCommentAsync(11, commentId, CancellationToken.None);

            // ASSERT
            var comments = await DbContext.Comments.ToListAsync();
            Assert.NotEmpty(comments);
            Assert.Single(comments);
            loggerMock.VerifyLog(l => l.LogWarning(It.IsAny<string>(),
                                       It.IsIn(commentId),
                                       It.IsIn(11)),
                    Times.Once);
        }

        [Fact]
        public async Task DeleteComment_ValidCommitValidUser_Deletes()
        {
            // ARRANGE
            await SetupManagerAsync();
            var comment = new Comment
            {
                Text = "my little comment",
                PostId = 50,
                ParentCommentId = 20,
                CreatorId = 10,
                CreatedOn = DateTime.UtcNow
            };
            await DbContext.Comments.AddAsync(comment);
            await DbContext.SaveChangesAsync();
            int commentId = comment.Id;

            // ACT
            await manager.DeleteCommentAsync(10, commentId, CancellationToken.None);

            // ASSERT
            var comments = await DbContext.Comments.ToListAsync();
            Assert.Empty(comments);

        }

    }
}
