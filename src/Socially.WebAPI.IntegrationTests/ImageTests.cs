using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Socially.Apps.Consumer.Services;
using Socially.Core.Entities;
using Socially.Server.DataAccess;
using Socially.Server.Services.Models;
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

            // ACT
            await using var stream = new MemoryStream(Array.Empty<byte>());
            using var response = await consumer.UploadAsync(new Core.Models.ImageUploadModel
            {
                ImageContext = new Core.Models.UploadContext
                {
                    FileName = "helloWorld.txt",
                    Stream = stream
                }
            });

            // ASSERT
            Assert.Equal(200, (int)response.StatusCode);
            var fileName = await response.Content.ReadAsStringAsync();
            var result = await dbContext.ProfileImages.ToListAsync();
            mockBlobAccess.Verify(b => b.UploadAsync(It.IsAny<string>(),
                                                     It.IsIn(fileName),
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
            var context = _factory.Services.GetService<CurrentContext>();

            context.UserId = 10;
            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();
            await dbContext.ProfileImages.AddRangeAsync(new ProfileImage
            {
                FileName = "hello.txt",
                UserId = context.UserId
            },
            new ProfileImage
            {
                FileName = "bye.txt",
                UserId = context.UserId
            },
            new ProfileImage
            {
                FileName = "bye.txt",
                UserId = 11
            },
            new ProfileImage
            {
                FileName = "seeya.png",
                UserId = context.UserId
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

            var context = _factory.Services.GetService<CurrentContext>();
            await using var scope = _factory.Services.CreateAsyncScope();
            var scopeProvider = scope.ServiceProvider;
            var dbContext = scopeProvider.GetService<ApplicationDbContext>();

            context.UserId = 10;
            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();
            await dbContext.ProfileImages.AddRangeAsync(new ProfileImage
            {
                FileName = "hello.txt",
                UserId = 5
            },
            new ProfileImage
            {
                FileName = "bye.txt",
                UserId = 2
            },
            new ProfileImage
            {
                FileName = "bye.txt",
                UserId = 11
            },
            new ProfileImage
            {
                FileName = "seeya.png",
                UserId = 2
            });
            await dbContext.SaveChangesAsync();

            // ACT
            var res = await consumer.GetAllImagesOfUserAsync();

            // ASSERT
            Assert.Empty(res);

        }

    }
}
