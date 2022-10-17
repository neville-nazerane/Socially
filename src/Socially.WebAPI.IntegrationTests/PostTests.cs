using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Socially.Apps.Consumer.Services;
using Socially.Server.DataAccess;
using Socially.Server.Entities;
using Socially.Server.Managers.Utils;
using Socially.WebAPI.IntegrationTests.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Socially.WebAPI.IntegrationTests
{
    public class PostTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public PostTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Add_ValidData_AddsPost()
        {
            // ARRANGE
            var consumer = new ApiConsumer(_factory.CreateClient());

            await using var scope = _factory.Services.CreateAsyncScope();
            var scopeProvider = scope.ServiceProvider;
            var dbContext = scopeProvider.GetService<ApplicationDbContext>();
            var context = scopeProvider.GetService<CurrentContext>();

            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();

            await consumer.TestSignupAndLoginAsync();

            var user = await dbContext.GetTestUserAsync();
            context.UserId = user.Id;

            // ACT
            var res = await consumer.AddPostAsync(new Socially.Models.AddPostModel
            {
                Text = "Sample post"
            });

            // ASSERT
            Assert.NotEqual(0, res);

            var currentPosts = await dbContext.Posts.ToListAsync();
            Assert.NotNull(currentPosts);
            Assert.NotEmpty(currentPosts);
            Assert.Single(currentPosts);

            var post = currentPosts.First();
            Assert.Equal("Sample post", post.Text);
            Assert.Equal(post.Id, res);
            Assert.Equal(user.Id, post.CreatorId);

        }

        [Fact]
        public async Task Delete_ExistingPost_Deletes()
        {
            // ARRANGE
            var consumer = new ApiConsumer(_factory.CreateClient());

            await using var scope = _factory.Services.CreateAsyncScope();
            var scopeProvider = scope.ServiceProvider;
            var dbContext = scopeProvider.GetService<ApplicationDbContext>();
            var context = scopeProvider.GetService<CurrentContext>();

            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();

            await consumer.TestSignupAndLoginAsync();

            var user = await dbContext.GetTestUserAsync();
            context.UserId = user.Id;

            await dbContext.Posts.AddRangeAsync(new Server.Entities.Post[]
            {
                new Server.Entities.Post
                {
                    Id = 2,
                    CreatorId = user.Id,
                    Text = "To delete"
                },
                new Server.Entities.Post
                {
                    Id = 3,
                    CreatorId = user.Id,
                    Text = "Not to delete"
                }
            });
            await dbContext.SaveChangesAsync();

            // ACT
            await consumer.DeletePostAsync(2);

            // ASSERT
            var posts = await dbContext.Posts.ToListAsync();
            Assert.NotNull(posts);
            Assert.NotEmpty(posts);
            Assert.Single(posts);
        }

        [Fact]
        public async Task AddComment_OnExistingPost_AddsComment()
        {
            // ARRANGE
            var consumer = new ApiConsumer(_factory.CreateClient());

            await using var scope = _factory.Services.CreateAsyncScope();
            var scopeProvider = scope.ServiceProvider;
            var dbContext = scopeProvider.GetService<ApplicationDbContext>();
            var context = scopeProvider.GetService<CurrentContext>();

            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();

            await consumer.TestSignupAndLoginAsync();

            var user = await dbContext.GetTestUserAsync();
            context.UserId = user.Id;

            await dbContext.Posts.AddRangeAsync(new Server.Entities.Post[]
            {
                new Server.Entities.Post
                {
                    Id = 1,
                    CreatorId = 10
                },
                new Server.Entities.Post
                {
                    Id = 2,
                    CreatorId = 10
                }
            });
            await dbContext.SaveChangesAsync();

            // ACT
            await consumer.AddCommentAsync(new Socially.Models.AddCommentModel
            {
                ParentCommentId = null,
                PostId = 1,
                Text = "Hello comment"
            });

            // ASSERT

            var currentPost = await dbContext.Posts.Include(p => p.Comments).SingleOrDefaultAsync(p => p.Id == 1);
            var otherPost = await dbContext.Posts.Include(p => p.Comments).SingleOrDefaultAsync(p => p.Id == 2);

            Assert.NotNull(currentPost.Comments);
            Assert.Single(currentPost.Comments);
            Assert.Equal("Hello comment", currentPost.Comments.First().Text);
            Assert.NotEqual(true, otherPost.Comments?.Any());
        }

        [Fact]
        public async Task AddComment_OnExistingComment_AddsCommentToPost()
        {
            // ARRANGE
            var consumer = new ApiConsumer(_factory.CreateClient());

            await using var scope = _factory.Services.CreateAsyncScope();
            var scopeProvider = scope.ServiceProvider;
            var dbContext = scopeProvider.GetService<ApplicationDbContext>();
            var context = scopeProvider.GetService<CurrentContext>();

            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();

            await consumer.TestSignupAndLoginAsync();

            var user = await dbContext.GetTestUserAsync();
            context.UserId = user.Id;

            await dbContext.Posts.AddRangeAsync(new Server.Entities.Post[]
            {
                new Server.Entities.Post
                {
                    Id = 1,
                    CreatorId = 11,
                    Comments = new List<Comment>
                    {
                        new Comment
                        {
                            Id = 2,
                            Text = "hello existing"
                        }
                    }
                }
            });
            await dbContext.SaveChangesAsync();

            // ACT
            await consumer.AddCommentAsync(new Socially.Models.AddCommentModel
            {
                Text = "new comment",
                ParentCommentId = 2,
                PostId = 1
            });

            // ASSERT
            var posts = await dbContext.Posts
                                       .Include(p => p.Comments)
                                       .ToListAsync();
            
            Assert.Single(posts);
            Assert.Equal(2, posts.First().Comments.Count());
            
            var newComment = posts.First().Comments.SingleOrDefault(c => c.ParentCommentId == 2);
            Assert.NotNull(newComment);
            Assert.Equal("new comment", newComment.Text);

        }

        [Fact]
        public async Task DeleteComment_OnExistingComment_DeletesComment()
        {
            // ARRANGE
            var consumer = new ApiConsumer(_factory.CreateClient());

            await using var scope = _factory.Services.CreateAsyncScope();
            var scopeProvider = scope.ServiceProvider;
            var dbContext = scopeProvider.GetService<ApplicationDbContext>();
            var context = scopeProvider.GetService<CurrentContext>();

            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();

            await consumer.TestSignupAndLoginAsync();

            var user = await dbContext.GetTestUserAsync();
            context.UserId = user.Id;

            await dbContext.Comments.AddRangeAsync(new Comment[]
            {
                new Comment
                {
                    Id = 1,
                    CreatorId = 1,
                    PostId = 1,
                    Text = "first comment"
                },
                new Comment
                {
                    Id = 2,
                    CreatorId = 1,
                    PostId = 1,
                    Text = "second comment"
                }
            });
            await dbContext.SaveChangesAsync();

            // ACT
            await consumer.DeleteCommentAsync(2);

            // ASSERT
            var comments = await dbContext.Comments.ToListAsync();

            Assert.NotNull(comments);
            Assert.Single(comments);

        }

        [Fact]
        public async Task SwapLike_ExistingPost_LikesPost()
        {
            // ARRANGE
            var consumer = new ApiConsumer(_factory.CreateClient());

            await using var scope = _factory.Services.CreateAsyncScope();
            var scopeProvider = scope.ServiceProvider;
            var dbContext = scopeProvider.GetService<ApplicationDbContext>();
            var context = scopeProvider.GetService<CurrentContext>();

            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();

            await consumer.TestSignupAndLoginAsync();

            var user = await dbContext.GetTestUserAsync();
            context.UserId = user.Id;

            await dbContext.Posts.AddRangeAsync(new Post[]
            {
                new Post
                {
                    Id = 1,
                    CreatorId = 1,
                    Text = "nice post"
                },
                new Post
                {
                    Id = 2,
                    CreatorId = 2,
                    Text = "boring post"
                }
            });
            await dbContext.SaveChangesAsync();

            // ACT
            bool liked = await consumer.SwapPostLikeAsync(2);

            // ASSERT
            Assert.True(liked);

            var likes = await dbContext.PostLikes.ToListAsync();

            Assert.NotNull(likes);
            Assert.Single(likes);
            
            Assert.Equal(2, likes.First().PostId);

        }

        [Fact]
        public async Task GetCurrentUserPosts_PostsFromMultipleUsers_GetCurrentUsersPosts()
        {

            // ARRANGE
            var consumer = new ApiConsumer(_factory.CreateClient());

            await using var scope = _factory.Services.CreateAsyncScope();
            var scopeProvider = scope.ServiceProvider;
            var dbContext = scopeProvider.GetService<ApplicationDbContext>();
            var context = scopeProvider.GetService<CurrentContext>();

            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();

            await consumer.TestSignupAndLoginAsync();

            var user = await dbContext.GetTestUserAsync();
            context.UserId = user.Id;

            await dbContext.Posts.AddRangeAsync(new Post[]
            {
                new Post
                {
                    CreatorId = user.Id + 3,
                    Text = "hello"
                },
                new Post
                {
                    CreatorId = user.Id,
                    Text = "current1"
                },
                new Post
                {
                    CreatorId = user.Id,
                    Text = "current2"
                }
            });
            await dbContext.SaveChangesAsync();

            // ACT
            var posts = await consumer.GetCurrentUserPostsAsync(10);

            // ASSERT
            Assert.NotNull(posts);
            Assert.NotEmpty(posts);
            Assert.Equal(2, posts.Count());

        }

        [Fact]
        public async Task GetProfilePosts_PostsFromMultipleUsers_GetsOtherUsersPosts()
        {

            // ARRANGE
            var consumer = new ApiConsumer(_factory.CreateClient());

            await using var scope = _factory.Services.CreateAsyncScope();
            var scopeProvider = scope.ServiceProvider;
            var dbContext = scopeProvider.GetService<ApplicationDbContext>();
            var context = scopeProvider.GetService<CurrentContext>();

            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();

            await consumer.TestSignupAndLoginAsync();

            var user = await dbContext.GetTestUserAsync();
            context.UserId = user.Id;

            await dbContext.Posts.AddRangeAsync(new Post[]
            {
                new Post
                {
                    CreatorId = user.Id + 3,
                    Text = "hello"
                },
                new Post
                {
                    CreatorId = user.Id,
                    Text = "current1"
                },
                new Post
                {
                    CreatorId = user.Id,
                    Text = "current2"
                }
            });
            await dbContext.SaveChangesAsync();

            // ACT
            var posts = await consumer.GetProfilePostsAsync(user.Id + 3, 8);

            // ASSERT
            Assert.NotEmpty(posts);
            Assert.Single(posts);

        }

        [Fact]
        public async Task GetHomePosts_PostsExists_GetsAllValidPosts()
        {
            // ARRANGE
            var consumer = new ApiConsumer(_factory.CreateClient());

            await using var scope = _factory.Services.CreateAsyncScope();
            var scopeProvider = scope.ServiceProvider;
            var dbContext = scopeProvider.GetService<ApplicationDbContext>();
            var context = scopeProvider.GetService<CurrentContext>();

            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();

            await consumer.TestSignupAndLoginAsync();

            var user = await dbContext.GetTestUserAsync();
            context.UserId = user.Id;

            await dbContext.Friends.AddRangeAsync(new Friend[]
            {
                new Friend
                {
                    FriendUserId = user.Id,
                    OwnerUser = new User
                    {
                        Posts = new List<Post>
                        {
                            new(),
                            new()
                        }
                    }
                },
                new Friend
                {
                    FriendUserId = user.Id + 10,
                    OwnerUser = new User
                    {
                        Posts = new List<Post>
                        {
                            new(),
                            new()
                        }
                    }
                }
            });
            await dbContext.Posts.AddAsync(new Post
            {
                CreatorId = user.Id,
            });
            await dbContext.SaveChangesAsync();

            // ACT
            var posts = await consumer.GetHomePostsAsync(50);

            // ASSERT
            Assert.NotEmpty(posts);
            Assert.Equal(2, posts.Count());

        }

    }
}
