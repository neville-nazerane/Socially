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
    public class PostManagerTests : TestBase
    {
        private Mock<ILogger<PostManager>> loggerMock;
        private PostManager manager;

        private Task SetupManagerAsync()
        {
            loggerMock = new Mock<ILogger<PostManager>>();
            manager = new PostManager(DbContext, CurrentContext, loggerMock.Object);
            return Task.CompletedTask;
        }


        [Fact]
        public async Task Add_ValidPost_AddsPost()
        {
            // ARRANGE
            await SetupManagerAsync();
            CurrentContext.UserId = 10;

            // ACT
            await manager.AddAsync(new AddPostModel
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
            CurrentContext.UserId = 10;
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
            await manager.DeleteAsync(invalidId, CancellationToken.None);


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
            CurrentContext.UserId = 30;

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
            await manager.DeleteAsync(postId, CancellationToken.None);

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
            CurrentContext.UserId= 10;

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
            await manager.DeleteAsync(postId, CancellationToken.None);

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
            await manager.AddCommentAsync(new AddCommentModel
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
            CurrentContext.UserId = 10;
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
            await manager.DeleteCommentAsync(invalidCommentId, CancellationToken.None);

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
            CurrentContext.UserId = 11;
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
            await manager.DeleteCommentAsync(commentId, CancellationToken.None);

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
            CurrentContext.UserId = 10;
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
            await manager.DeleteCommentAsync(commentId, CancellationToken.None);

            // ASSERT
            var comments = await DbContext.Comments.ToListAsync();
            Assert.Empty(comments);

        }

        [Fact]
        public async Task GetProfilePosts_ValidUser_GetUsersPosts()
        {
            // ARRANGE
            await SetupManagerAsync();
            CurrentContext.UserId = 10;
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

        [Fact]
        public async Task GetProfilePosts_ValidUserMultiplePages_GetEachPage()
        {
            // ARRANGE
            await SetupManagerAsync();
            int currentUserId = 10;
            CurrentContext.UserId = currentUserId;
            int timeIncrement = 0;
            await DbContext.Posts.AddRangeAsync(new Post[]
            {
                new Post
                {
                    Text = "first post",
                    CreatedOn = DateTime.UtcNow.AddMilliseconds(++timeIncrement),
                    CreatorId = currentUserId,
                },
                new Post
                {
                    Text = "second post",
                    CreatorId = 11,
                    CreatedOn = DateTime.UtcNow.AddMilliseconds(++timeIncrement)
                },
                new Post
                {
                    Text = "third post",
                    CreatorId = currentUserId,
                    CreatedOn = DateTime.UtcNow.AddMilliseconds(++ timeIncrement)
                },
                new Post
                {
                    Text = "fourth post",
                    CreatorId = currentUserId,
                    CreatedOn = DateTime.UtcNow.AddMilliseconds(++ timeIncrement)
                },
                new Post
                {
                    Text = "fifth post",
                    CreatorId = currentUserId,
                    CreatedOn = DateTime.UtcNow.AddMilliseconds(++ timeIncrement)
                },
                new Post
                {
                    Text = "sixth post",
                    CreatorId = currentUserId,
                    CreatedOn = DateTime.UtcNow.AddMilliseconds(++ timeIncrement)
                },
                new Post
                {
                    Text = "seventh post",
                    CreatorId = currentUserId,
                    CreatedOn = DateTime.UtcNow.AddMilliseconds(++ timeIncrement)
                }
            });
            await DbContext.SaveChangesAsync();


            // ACT
            var page1 = await manager.GetProfilePostsAsync(4);
            var token = page1.OrderBy(p => p.CreatedOn).LastOrDefault()?.CreatedOn;
            var page2 = await manager.GetProfilePostsAsync(4, token);


            // ASSERT

            Assert.NotNull(page1);
            Assert.NotEmpty(page1);
            Assert.Equal(4, page1.Count());

            Assert.NotNull(page2);
            Assert.NotEmpty(page2);
            Assert.Equal(2, page2.Count());

        }

        [Fact]
        public async Task GetHomePosts_ValidUser_GetUsersPosts()
        {
            // ARRANGE
            await SetupManagerAsync();
            int currentUserId = 10;
            CurrentContext.UserId = currentUserId;
            await DbContext.Users.AddAsync(new User
            {
                CreatedOn = DateTime.UtcNow,
                Id = 11,
                Friends = new Friend[]
                {
                    new Friend
                    {
                        FriendUserId = currentUserId
                    }
                }
            });

            await DbContext.Posts.AddRangeAsync(new Post[]
            {
                new Post
                {
                    Text = "first post",
                    CreatorId = currentUserId,
                    CreatedOn = DateTime.UtcNow
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
                    CreatorId = currentUserId,
                    CreatedOn = DateTime.UtcNow
                },
                 new Post
                {
                    Text = "fourth post",
                    CreatorId = 12,
                    CreatedOn = DateTime.UtcNow
                },
            });
            await DbContext.SaveChangesAsync();


            // ACT
            var res = await manager.GetHomePostsAsync(30);


            // ASSERT

            Assert.NotNull(res);
            Assert.NotEmpty(res);
            Assert.Single(res);

        }

        [Fact]
        public async Task SwapLike_ExistingPostNotLikedByAnyone_PostLiked()
        {
            // ASSERT
            await SetupManagerAsync();
            var currentUserId = 11;
            CurrentContext.UserId = currentUserId;
            int targetPostId = 10;
            await DbContext.Posts.AddRangeAsync(new Post[]
            {
                new Post
                {
                    Id = targetPostId,
                    CreatorId = currentUserId,
                    Text = "real post"
                },
                new Post
                {
                    Id = 30,
                    CreatorId = currentUserId,
                    Text = "other liked post",
                    LikeCount = 1
                }
            });
            await DbContext.PostLikes.AddRangeAsync(new PostLike[]
            {
                new PostLike
                {
                    PostId = 30,
                    UserId = currentUserId,
                }
            });

            await DbContext.SaveChangesAsync();

            // ACT
            await manager.SwapLikeAsync(targetPostId, null);

            // ASSERT
            var targetPost = await DbContext.Posts.SingleOrDefaultAsync(p => p.Id == targetPostId);
            var wrongPost = await DbContext.Posts.SingleOrDefaultAsync(p => p.Id == 30);
            Assert.Equal(1, targetPost.LikeCount);
            Assert.Equal(1, wrongPost.LikeCount);

            var recordExist = await DbContext.PostLikes.AnyAsync(l => l.PostId == targetPostId && l.UserId == currentUserId);
            Assert.True(recordExist);

        }

        [Fact]
        public async Task SwapLike_ExistingPostNotLikedByCurrentUser_PostLiked()
        {
            // ASSERT
            await SetupManagerAsync();
            var currentUserId = 11;
            CurrentContext.UserId = currentUserId;
            int targetPostId = 10;
            await DbContext.Users.AddRangeAsync(new User
            {
                CreatedOn = DateTime.UtcNow,
                Id = currentUserId
            });
            await DbContext.Posts.AddRangeAsync(new Post[]
            {
                new Post
                {
                    Id = targetPostId,
                    CreatorId = currentUserId,
                    Text = "real post",
                    CreatedOn = DateTime.UtcNow,
                    LikeCount = 1
                },
                new Post
                {
                    Id = 30,
                    CreatorId = currentUserId,
                    Text = "other liked post",
                    CreatedOn = DateTime.UtcNow,
                    LikeCount = 1
                }
            });
            await DbContext.PostLikes.AddRangeAsync(new PostLike[]
            {
                new PostLike
                {
                    PostId = 30,
                    UserId = currentUserId,
                },
                new PostLike
                {
                    PostId = targetPostId,
                    UserId = 29
                }
            });

            await DbContext.SaveChangesAsync();

            // ACT
            await manager.SwapLikeAsync(targetPostId, null);

            // ASSERT
            var targetPost = await DbContext.Posts.SingleOrDefaultAsync(p => p.Id == targetPostId);
            var wrongPost = await DbContext.Posts.SingleOrDefaultAsync(p => p.Id == 30);
            Assert.Equal(2, targetPost.LikeCount);
            Assert.Equal(1, wrongPost.LikeCount);

            var recordExist = await DbContext.PostLikes.AnyAsync(l => l.PostId == targetPostId && l.UserId == currentUserId);
            Assert.True(recordExist);

        }

        [Fact]
        public async Task SwapLike_ExistingLikedPost_PostUnLiked()
        {
            // ASSERT
            await SetupManagerAsync();
            var currentUserId = 11;
            CurrentContext.UserId = currentUserId;
            int targetPostId = 10;
            await DbContext.Posts.AddRangeAsync(new Post[]
            {
                new Post
                {
                    Id = targetPostId,
                    CreatorId = currentUserId,
                    Text = "real post",
                    CreatedOn = DateTime.UtcNow,
                    LikeCount = 1,
                    Likes = new List<PostLike>
                    {
                        new PostLike { UserId = currentUserId }
                    }
                },
                new Post
                {
                    Id = 30,
                    CreatorId = currentUserId,
                    Text = "other liked post",
                    CreatedOn = DateTime.UtcNow,
                    LikeCount = 1,
                    Likes = new List<PostLike>
                    {
                        new PostLike { UserId = currentUserId  }
                    }
                }
            });

            await DbContext.SaveChangesAsync();

            // ACT
            await manager.SwapLikeAsync(targetPostId, null);

            // ASSERT
            var targetPost = await DbContext.Posts.SingleOrDefaultAsync(p => p.Id == targetPostId);
            var wrongPost = await DbContext.Posts.SingleOrDefaultAsync(p => p.Id == 30);
            Assert.Null(targetPost.LikeCount);
            Assert.Equal(1, wrongPost.LikeCount);

            var recordExist = await DbContext.PostLikes.AnyAsync(l => l.PostId == targetPostId && l.UserId == currentUserId);
            Assert.False(recordExist);
        }

        [Fact]
        public async Task SwapLike_ExistingCommentNotLikedbyAnyOne_CommentLiked()
        {
            // ARRANGE
            await SetupManagerAsync();
            var currentUserId = 11;
            CurrentContext.UserId = currentUserId;
            int targetPostId = 10;
            int targetCommentId = 9;
            await DbContext.Posts.AddAsync(new Post
            {
                Id = targetPostId,
                CreatorId = currentUserId,
                Text = "real post",
                CreatedOn = DateTime.UtcNow,
                Comments = new Comment[]
                    {
                        new Comment
                        {
                            Id = targetCommentId
                        }
                    }
            });
            await DbContext.PostLikes.AddAsync(new PostLike
            {
                PostId = 30,
                UserId = currentUserId,
            });
            await DbContext.SaveChangesAsync();

            // ACT
            await manager.SwapLikeAsync(targetPostId, targetCommentId);

            // ASSERT
            var targetPost = await DbContext.Posts
                                            .Include(p => p.Comments)
                                            .SingleOrDefaultAsync(p => p.Id == targetPostId);
            Assert.Null(targetPost.LikeCount);
            Assert.Equal(1, targetPost.Comments.First().LikeCount);

            var recordExist = await DbContext.PostLikes.AnyAsync(l => l.PostId == targetPostId && l.UserId == currentUserId && l.CommentId == targetCommentId);
            Assert.True(recordExist);

        }

        [Fact]
        public async Task SwapLike_ExistingCommentNotLikedbyCurrentUser_CommentLiked()
        {
            // ARRANGE
            await SetupManagerAsync();
            var currentUserId = 11;
            CurrentContext.UserId = currentUserId;   
            int targetPostId = 10;
            int targetCommentId = 9;
            await DbContext.Posts.AddRangeAsync(new Post[]
            {
                new Post
                {
                    Id = targetPostId,
                    CreatorId = currentUserId,
                    Text = "real post",
                    CreatedOn = DateTime.UtcNow,
                    Comments = new Comment[]
                    {
                        new Comment
                        {
                            Id = targetCommentId,
                            LikeCount = 1
                        }
                    }
                }
            });
            await DbContext.PostLikes.AddRangeAsync(new PostLike[]
            {
                new PostLike
                {
                    PostId = 30,
                    UserId = currentUserId,
                },
                new PostLike
                {
                    PostId = targetPostId,
                    CommentId = currentUserId,
                    UserId = 29
                }
            });
            await DbContext.SaveChangesAsync();

            // ACT
            await manager.SwapLikeAsync(targetPostId, targetCommentId);

            // ASSERT
            var targetPost = await DbContext.Posts
                                            .Include(p => p.Comments)
                                            .SingleOrDefaultAsync(p => p.Id == targetPostId);
            Assert.Null(targetPost.LikeCount);
            Assert.Equal(2, targetPost.Comments.First().LikeCount);

            var recordExist = await DbContext.PostLikes.AnyAsync(l => l.PostId == targetPostId && l.UserId == currentUserId && l.CommentId == targetCommentId);
            Assert.True(recordExist);

        }

        [Fact]
        public async Task SwapLike_ExistingCommentLikedByCurrentUser_CommentUnLiked()
        {
            // ARRANGE
            await SetupManagerAsync();
            var currentUserId = 11;
            CurrentContext.UserId = currentUserId;
            int targetPostId = 10;
            int targetCommentId = 9;
            await DbContext.Posts.AddRangeAsync(new Post
            {
                Id = targetPostId,
                CreatorId = currentUserId,
                Text = "real post",
                CreatedOn = DateTime.UtcNow,
                Comments = new Comment[]
                    {
                        new Comment
                        {
                            Id = targetCommentId,
                            LikeCount = 1
                        }
                    }
            });
            await DbContext.PostLikes.AddAsync(new PostLike
            {
                PostId = targetPostId,
                CommentId = targetCommentId,
                UserId = currentUserId
            });
            await DbContext.SaveChangesAsync();

            // ACT
            await manager.SwapLikeAsync(targetPostId, targetCommentId);

            // ASSERT
            var targetPost = await DbContext.Posts
                                            .Include(p => p.Comments)
                                            .SingleOrDefaultAsync(p => p.Id == targetPostId);
            Assert.Null(targetPost.LikeCount);
            Assert.Null(targetPost.Comments.First().LikeCount);

            var recordExist = await DbContext.PostLikes.AnyAsync(l => l.PostId == targetPostId && l.UserId == currentUserId && l.CommentId == targetCommentId);
            Assert.False(recordExist);

        }

    }
}
