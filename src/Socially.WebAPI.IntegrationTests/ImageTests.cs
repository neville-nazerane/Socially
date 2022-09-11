using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Socially.Apps.Consumer.Services;
using Socially.Server.DataAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
            var dbContext = _factory.Services.GetService<ApplicationDbContext>();

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

            var result = await dbContext.ProfileImages.ToListAsync();
            Assert.NotEmpty(result);
            Assert.Single(result);
        }

    }
}
