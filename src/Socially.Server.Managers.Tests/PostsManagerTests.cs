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


        [Fact, Trait("action", "add"), Trait("happyPath", "valid")]
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


        [Fact, Trait("action", "delete"), Trait("happyPath", "invalid")]
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

        [Fact, Trait("action", "delete"), Trait("happyPath", "invalid")]
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

        [Fact, Trait("action", "delete"), Trait("happyPath", "valid")]
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


        [Fact, Trait("action", "add"), Trait("happyPath", "valid")]
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


        [Fact, Trait("action", "add"), Trait("happyPath", "invalid")]
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

        [Fact, Trait("action", "delete"), Trait("happyPath", "invalid")]
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

        [Fact, Trait("action", "delete"), Trait("happyPath", "valid")]
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

        [Fact, Trait("action", "get"), Trait("happyPath", "valid")]
        public async Task GetProfilePosts_ValidUser_GetUsersPosts()
        {
            // ARRANGE
            await SetupManagerAsync();
            await DbContext.Posts.AddRangeAsync(new Post[]
            {
                new Post
                {
                    Text = "first post",
                    CreatedOn = DateTime.UtcNow,
                    CreatorId = 10,
                    Comments = new Comment[]
                    {
                        new Comment
                        {
                            Id = 1,
                            CreatedOn = DateTime.UtcNow,
                            Text = "first comment"
                        },
                        new Comment
                        {
                            Id = 2,
                            CreatedOn = DateTime.UtcNow,
                            Text = "Second comment",
                        },
                        new Comment
                        {
                            Id = 3,
                            ParentCommentId = 2,
                            Text = "fore-ty",
                            CreatedOn = DateTime.UtcNow
                        }
                        ,
                        new Comment
                        {
                            Id = 4,
                            ParentCommentId = 3,
                            Text = "inner inner comment",
                            CreatedOn = DateTime.UtcNow
                        }
                    }
                },
                new Post
                {
                    Text = "second post",
                    CreatorId = 11,
                    CreatedOn = DateTime.UtcNow
                },
                new Post
                {
                    Text = "third post",
                    CreatorId = 10,
                    CreatedOn = DateTime.UtcNow
                }
            });
            await DbContext.SaveChangesAsync();


            // ACT
            var res = await manager.GetProfilePostsAsync(10);

            
            // ASSERT
            
            Assert.NotNull(res);
            Assert.NotEmpty(res);
            Assert.Equal(2, res.Count());

            var comment2 = res.SingleOrDefault(p => p.Text == "first post")?.Comments?.SingleOrDefault(c => c.Id == 2);
            Assert.NotNull(comment2);
            Assert.Equal("Second comment", comment2.Text);
            Assert.NotEmpty(comment2.Comments);
            Assert.Single(comment2.Comments);
            Assert.NotEmpty(comment2.Comments.First().Comments);
            Assert.Single(comment2.Comments.First().Comments);

        }

    }
}
