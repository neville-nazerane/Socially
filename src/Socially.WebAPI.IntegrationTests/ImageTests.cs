using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Socially.Apps.Consumer.Services;
using Socially.Core.Entities;
using Socially.Server.DataAccess;
using Socially.Server.Services.Models;
using Socially.WebAPI.IntegrationTests.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Socially.WebAPI.IntegrationTests
{
    public class ImageTests : IClassFixture<CustomWebApplicationFactory>
    {


        private readonly CustomWebApplicationFactory _factory;

        public ImageTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ImageUpload()
        {

            // ARRANGE
            var consumer = new ApiConsumer(_factory.CreateClient());

            var mockBlobAccess = _factory.Services.GetService<Mock<IBlobAccess>>();
            await using var scope = _factory.Services.CreateAsyncScope();
            var scopeProvider = scope.ServiceProvider;
            var dbContext = scopeProvider.GetService<ApplicationDbContext>();

            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();

            await consumer.TestSignupAndLoginAsync();
            var user = await dbContext.GetTestUserAsync();

            // ACT
            await using var stream = new MemoryStream(Array.Empty<byte>());
            using var response = await consumer.UploadAsync(new Core.Models.ImageUploadModel
            {
                ImageContext = new Core.Models.UploadContext
                {
                    FileName = "helloWorld.png",
                    Stream = stream
                }
            });

            // ASSERT
            Assert.Equal(200, (int)response.StatusCode);
            var fileName = await response.Content.ReadAsStringAsync();
            var result = await dbContext.ProfileImages.ToListAsync();
            mockBlobAccess.Verify(b => b.UploadAsync(It.IsAny<string>(),
                                                     It.IsIn(fileName),
                                                     It.IsIn("image/png"),
                                                     It.IsAny<Stream>(),
                                                     It.IsAny<CancellationToken>()),
                                  Times.Once);
            Assert.NotEmpty(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task GetImages_HasImages_ReturnsImages()
        {
            // ARRANGE
            var consumer = new ApiConsumer(_factory.CreateClient());

            await using var scope = _factory.Services.CreateAsyncScope();
            var scopeProvider = scope.ServiceProvider;
            var dbContext = scopeProvider.GetService<ApplicationDbContext>();
            //var context = _factory.Services.GetService<CurrentContext>();

            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();

            await consumer.TestSignupAndLoginAsync();

            var user = await dbContext.GetTestUserAsync();

            await dbContext.ProfileImages.AddRangeAsync(new ProfileImage
            {
                FileName = "hello.txt",
                UserId = user.Id
            },
            new ProfileImage
            {
                FileName = "bye.txt",
                UserId = user.Id
            },
            new ProfileImage
            {
                FileName = "bye.txt",
                UserId = 11
            },
            new ProfileImage
            {
                FileName = "seeya.png",
                UserId = user.Id
            });
            await dbContext.SaveChangesAsync();

            // ACT
            var res = await consumer.GetAllImagesOfUserAsync();

            // ASSERT
            Assert.NotEmpty(res);
            Assert.Equal(3, res.Count());

        }

        [Fact]
        public async Task GetImages_NoImages_ReturnEmpty()
        {
            // ARRANGE
            var consumer = new ApiConsumer(_factory.CreateClient());

            //var context = _factory.Services.GetService<CurrentContext>();
            await using var scope = _factory.Services.CreateAsyncScope();
            var scopeProvider = scope.ServiceProvider;
            var dbContext = scopeProvider.GetService<ApplicationDbContext>();

            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();

            await consumer.TestSignupAndLoginAsync();
            var user = await dbContext.GetTestUserAsync();

            await dbContext.ProfileImages.AddRangeAsync(new ProfileImage
            {
                FileName = "hello.txt",
                UserId = user.Id + 5
            },
            new ProfileImage
            {
                FileName = "bye.txt",
                UserId = user.Id + 2
            },
            new ProfileImage
            {
                FileName = "bye.txt",
                UserId = user.Id + 11
            },
            new ProfileImage
            {
                FileName = "seeya.png",
                UserId = user.Id + 2
            });
            await dbContext.SaveChangesAsync();

            // ACT
            var res = await consumer.GetAllImagesOfUserAsync();

            // ASSERT
            Assert.Empty(res);

        }

        [Fact]
        public async Task DeleteImage_HasImages_DeletesOne()
        {
            // ARRANGE
            var consumer = new ApiConsumer(_factory.CreateClient());

            await using var scope = _factory.Services.CreateAsyncScope();
            var scopeProvider = scope.ServiceProvider;
            var dbContext = scopeProvider.GetService<ApplicationDbContext>();

            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();

            await consumer.TestSignupAndLoginAsync();

            var user = await dbContext.GetTestUserAsync();

            await dbContext.ProfileImages.AddRangeAsync(new ProfileImage
            {
                FileName = "hello.txt",
                UserId = user.Id
            },
            new ProfileImage
            {
                FileName = "bye.txt",
                UserId = user.Id
            },
            new ProfileImage
            {
                FileName = "bye.txt",
                UserId = 11
            },
            new ProfileImage
            {
                FileName = "seeya.png",
                UserId = user.Id
            });
            await dbContext.SaveChangesAsync();

            // ACT
            var result = await consumer.DeleteImageAsync("hello.txt");

            // ASSERT
            Assert.Equal(200, (int)result.StatusCode);
            var imageCount = await dbContext.ProfileImages
                                            .Where(i => i.UserId == user.Id)
                                            .CountAsync();
            Assert.Equal(2, imageCount);
        }

        [Fact]
        public async Task DeleteImage_HasNoImagesForUser_DeletesNone()
        {
            // ARRANGE
            var consumer = new ApiConsumer(_factory.CreateClient());

            await using var scope = _factory.Services.CreateAsyncScope();
            var scopeProvider = scope.ServiceProvider;
            var dbContext = scopeProvider.GetService<ApplicationDbContext>();

            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();

            await consumer.TestSignupAndLoginAsync();

            var user = await dbContext.GetTestUserAsync();

            await dbContext.ProfileImages.AddRangeAsync(new ProfileImage
            {
                FileName = "hello.txt",
                UserId = user.Id + 3
            },
            new ProfileImage
            {
                FileName = "bye.txt",
                UserId = user.Id + 3
            },
            new ProfileImage
            {
                FileName = "bye.txt",
                UserId = user.Id + 11
            },
            new ProfileImage
            {
                FileName = "seeya.png",
                UserId = user.Id + 2
            });
            await dbContext.SaveChangesAsync();

            // ACT
            var result = await consumer.DeleteImageAsync("hello.txt");

            // ASSERT
            Assert.Equal(200, (int)result.StatusCode);
            var imageCount = await dbContext.ProfileImages
                                            .CountAsync();
            Assert.Equal(4, imageCount);
        }

    }
}
