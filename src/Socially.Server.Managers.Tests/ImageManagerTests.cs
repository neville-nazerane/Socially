using Castle.Core.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Socially.Core.Entities;
using Socially.Server.DataAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Socially.Server.Managers.Tests
{
    public class ImageManagerTests : TestBase
    {
        private Mock<IBlobAccess> blobMock;
        private Mock<ILogger<ImageManager>> loggerMock;
        private ImageManager manager;

        private async Task SetupManagerAsync()
        {
            blobMock = new Mock<IBlobAccess>();
            loggerMock = new Mock<ILogger<ImageManager>>();
            manager = new ImageManager(DbContext, loggerMock.Object, blobMock.Object);
        }

        [Fact]
        public async Task Add_PassingValidData_Succeeds()
        {
            // ARRANGE
            await SetupManagerAsync();
            var cancellationToken = new CancellationToken();

            // ACT
            var fileName = await manager.AddAsync(4, ".sample", null, cancellationToken);

            // ASSERT
            blobMock.Verify(b => b.UploadAsync(It.IsIn("userprofiles"),
                                               It.Is<string>(s => s.EndsWith(".sample")),
                                               It.IsAny<Stream>(),
                                               It.IsIn(cancellationToken)), 
                            Times.Once);

            Assert.Equal("sample", fileName.Split(".").LastOrDefault());
            var item = await DbContext.ProfileImages.SingleOrDefaultAsync();
            Assert.NotNull(item);
            Assert.Equal(fileName, item.FileName);
        }

        [Fact]
        public async Task GetAllForUser_NoData_ReturnsEmpty()
        {
            // ARRANGE
            await SetupManagerAsync();

            // ACT
            var result = await manager.GetAllForUserAsync(30);

            // ASSERT
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllForUser_DifferentUser_ReturnsEmpty()
        {
            // ARRANGE
            await SetupManagerAsync();
            await DbContext.ProfileImages.AddRangeAsync(new ProfileImage
            {
                UserId = 43,
                FileName = "firstName"
            },
            new ProfileImage
            {
                UserId = 43,
                FileName = "secondName"
            });
            await DbContext.SaveChangesAsync();

            // ACT
            var result = await manager.GetAllForUserAsync(30);

            // ASSERT
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllForUser_CorrectUser_ReturnsResults()
        {
            // ARRANGE
            await SetupManagerAsync();
            await DbContext.ProfileImages.AddRangeAsync(new ProfileImage
            {
                UserId = 43,
                FileName = "firstName"
            },
            new ProfileImage
            {
                UserId = 43,
                FileName = "secondName"
            },
            new ProfileImage
            {
                UserId = 44,
                FileName = "thirdName"
            });
            await DbContext.SaveChangesAsync();

            // ACT
            var result = await manager.GetAllForUserAsync(43);

            // ASSERT
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count());
            Assert.Equal("firstName", result.First());
            Assert.Equal("secondName", result.Last());
        }

        [Fact]
        public async Task DeleteByName_DeleteNonExisting_DoesNothing()
        {
            // ARRANGE
            await SetupManagerAsync();
            await DbContext.ProfileImages.AddRangeAsync(new ProfileImage
            {
                UserId = 43,
                FileName = "firstName"
            },
            new ProfileImage
            {
                UserId = 43,
                FileName = "secondName"
            },
            new ProfileImage
            {
                UserId = 44,
                FileName = "thirdName"
            });
            await DbContext.SaveChangesAsync();

            // ACT
            await manager.DeleteByNameAsync(22, "nonName");

            // ASSERT
            loggerMock.VerifyLog(l => l.LogWarning(It.IsAny<string>(),
                                                   It.Is<object[]>(p => p.Contains("nonName") && p.Contains(22))),
                                Times.Once);
            var allImages = await DbContext.ProfileImages.ToListAsync();
            Assert.NotEmpty(allImages);
            Assert.Equal(3, allImages.Count);
        }

        [Fact]
        public async Task DeleteByName_DeleteWrongUserId_DoesNothing()
        {
            // ARRANGE
            await SetupManagerAsync();
            await DbContext.ProfileImages.AddRangeAsync(new ProfileImage
            {
                UserId = 43,
                FileName = "firstName"
            },
            new ProfileImage
            {
                UserId = 43,
                FileName = "secondName"
            },
            new ProfileImage
            {
                UserId = 44,
                FileName = "thirdName"
            });
            await DbContext.SaveChangesAsync();

            // ACT
            await manager.DeleteByNameAsync(21, "secondName");

            // ASSERT
            loggerMock.VerifyLog(l => l.LogWarning(It.IsAny<string>(),
                                       It.Is<object[]>(p => p.Contains("secondName") && p.Contains(21))),
                    Times.Once);
            var allImages = await DbContext.ProfileImages.ToListAsync();
            Assert.NotEmpty(allImages);
            Assert.Equal(3, allImages.Count);

        }

        [Fact]
        public async Task DeleteByName_ValidRecord_DeletesSingleRecord()
        {
            // ARRANGE
            await SetupManagerAsync();
            await DbContext.ProfileImages.AddRangeAsync(new ProfileImage
            {
                UserId = 43,
                FileName = "firstName"
            },
            new ProfileImage
            {
                UserId = 43,
                FileName = "secondName"
            },
            new ProfileImage
            {
                UserId = 44,
                FileName = "thirdName"
            });
            await DbContext.SaveChangesAsync();

            // ACT
            await manager.DeleteByNameAsync(43, "secondName");

            // ASSERT
            var allImages = await DbContext.ProfileImages.ToListAsync();
            Assert.NotEmpty(allImages);
            Assert.Equal(2, allImages.Count);
            Assert.Equal("firstName", allImages.First().FileName);
            Assert.Equal("thirdName", allImages.Last().FileName);

        }

    }
}
