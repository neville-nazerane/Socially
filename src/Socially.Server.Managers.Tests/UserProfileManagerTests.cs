using Microsoft.EntityFrameworkCore;
using Socially.Core.Entities;
using Socially.Core.Models;
using Socially.Server.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Socially.Server.Managers.Tests
{
    public class UserProfileManagerTests : TestBase
    {

        private UserProfileManager manager;

        private async Task SetupManagerAsync()
        {
            await DbContext.Database.EnsureDeletedAsync();
            await DbContext.Database.EnsureCreatedAsync();
            manager = new UserProfileManager(DbContext);
        }

        [Fact]
        public async Task EmailExists_EmptyTable_ReturnsFalse()
        {
            // ARRANGE
            await SetupManagerAsync();

            // ACT
            bool result = await manager.EmailExistsAsync("Doesnt@matter.com");

            // ASSERT
            Assert.False(result);
        }

        [Fact]
        public async Task EmailExists_NonExistingEmail_ReturnsFalse()
        {
            // ARRANGE
            await SetupManagerAsync();
            await DbContext.Users.AddAsync(new User
            {
                CreatedOn = DateTime.UtcNow,
                Email = "something@valid.com"
            });
            await DbContext.SaveChangesAsync();

            // ACT
            bool result = await manager.EmailExistsAsync("Doesnt@matter.com");

            // ASSERT
            Assert.False(result);
        }

        [Fact]
        public async Task EmailExists_ExistingEmail_ReturnsTrue()
        {
            // ARRANGE
            await SetupManagerAsync();
            await DbContext.Users.AddAsync(new User
            {
                CreatedOn = DateTime.UtcNow,
                Email = "something@valid.com"
            });
            await DbContext.SaveChangesAsync();

            // ACT
            bool result = await manager.EmailExistsAsync("something@valid.com");

            // ASSERT
            Assert.True(result);
        }


        [Fact]
        public async Task UserNameExists_EmptyTable_ReturnsFalse()
        {
            // ARRANGE
            await SetupManagerAsync();

            // ACT
            bool result = await manager.UserNameExistsAsync("myuser");

            // ASSERT
            Assert.False(result);
        }

        [Fact]
        public async Task UserNameExists_NonExistingUserName_ReturnsFalse()
        {
            // ARRANGE
            await SetupManagerAsync();
            await DbContext.Users.AddAsync(new User
            {
                CreatedOn = DateTime.UtcNow,
                UserName = "myUser"
            });
            await DbContext.SaveChangesAsync();

            // ACT
            bool result = await manager.UserNameExistsAsync("otherUser");

            // ASSERT
            Assert.False(result);
        }

        [Fact]
        public async Task UserNameExists_ExistingUserName_ReturnsTrue()
        {
            // ARRANGE
            await SetupManagerAsync();
            await DbContext.Users.AddAsync(new User
            {
                CreatedOn = DateTime.UtcNow,
                UserName = "myUser"
            });
            await DbContext.SaveChangesAsync();

            // ACT
            bool result = await manager.UserNameExistsAsync("myUser");

            // ASSERT
            Assert.True(result);
        }

        [Fact]
        public async Task GetUpdatableProfile_EmptyRecords_ReturnsNull()
        {
            // ARRANGE
            await SetupManagerAsync();

            // ACT
            var profile = await manager.GetUpdatableProfileAsync(100);

            // ASSERT
            Assert.Null(profile);
        }

        [Fact]
        public async Task GetUpdatableProfile_NonExistantId_ReturnsNull()
        {

            // ARRANGE
            await SetupManagerAsync();
            await DbContext.Users.AddAsync(new User
            {
                CreatedOn = DateTime.UtcNow,
                UserName = "sampleUser",
                Id = 40
            });
            await DbContext.SaveChangesAsync();

            // ACT
            var profile = await manager.GetUpdatableProfileAsync(100);

            // ASSERT
            Assert.Null(profile);
        }

        [Fact]
        public async Task GetUpdatableProfile_ValidId_ReturnsProfile()
        {
            // ARRANGE
            await SetupManagerAsync();
            await DbContext.Users.AddAsync(new User
            {
                CreatedOn = DateTime.UtcNow,
                FirstName = "sampleUser",
                Id = 40
            });
            await DbContext.SaveChangesAsync();

            // ACT
            var profile = await manager.GetUpdatableProfileAsync(40);

            // ASSERT
            Assert.NotNull(profile);
            Assert.Equal("sampleUser", profile.FirstName);
        }

        [Fact]
        public async Task Update_InvalidIdAndNullModel_ThrowsArgumentNullException()
        {
            // ARRANGE
            await SetupManagerAsync();
            await DbContext.Users.AddAsync(new User
            {
                CreatedOn = DateTime.UtcNow,
                Id = 44
            });
            await DbContext.SaveChangesAsync();

            // ACT & ASSERT
            await Assert.ThrowsAsync<ArgumentNullException>(() => manager.UpdateAsync(22, null));
        }

        [Fact]
        public async Task Update_NoData_ThrowsNullReferenceException()
        {
            // ARRANGE
            await SetupManagerAsync();

            // ACT & ASSERT
            await Assert.ThrowsAsync<NullReferenceException>(() => manager.UpdateAsync(22, new ProfileUpdateModel()));
        }

        [Fact]
        public async Task Update_InvalidId_ThrowsNullReferenceException()
        {
            // ARRANGE
            await SetupManagerAsync();
            await DbContext.Users.AddAsync(new User
            {
                CreatedOn = DateTime.UtcNow,
                Id = 44
            });
            await DbContext.SaveChangesAsync();

            // ACT & ASSERT
            await Assert.ThrowsAsync<NullReferenceException>(() => manager.UpdateAsync(22, new ProfileUpdateModel()));
        }

        [Fact]
        public async Task Update_ValidIdAndNullModel_ThrowsArgumentNullException()
        {
            // ARRANGE
            await SetupManagerAsync();
            await DbContext.Users.AddAsync(new User
            {
                CreatedOn = DateTime.UtcNow,
                Id = 44
            });
            await DbContext.SaveChangesAsync();

            // ACT & ASSERT
            await Assert.ThrowsAsync<ArgumentNullException>(() => manager.UpdateAsync(44, null));
        }

        [Fact]
        public async Task Update_ValidIdAndValidModel_Updates()
        {
            // ARRANGE
            await SetupManagerAsync();
            await DbContext.Users.AddAsync(new User
            {
                FirstName = "Unchanged",
                CreatedOn = DateTime.UtcNow,
                Id = 44
            });
            await DbContext.SaveChangesAsync();

            // ACT
            var model = new ProfileUpdateModel
            {
                DateOfBirth = new DateTime(2000, 04, 01),
                FirstName = "Changed"
            };
            await manager.UpdateAsync(44, model);
            var stored = await DbContext.Users.FindAsync(44);


            // ASSERT
            Assert.NotEqual("Unchanged", stored.FirstName);
            Assert.Equal("Changed", stored.FirstName);

        }

        [Fact]
        public async Task CreateRefreshToken_Creating_Creates()
        {
            // ARRANGE
            await SetupManagerAsync();

            // ACT
            string token = await manager.CreateRefreshTokenAsync(10, TimeSpan.Zero);

            // ASSERT
            var exists = await DbContext.UserRefreshTokens.AnyAsync(u => u.UserId == 10 && u.RefreshToken == token);
            var exists2 = await DbContext.UserRefreshTokens.AnyAsync(u => u.UserId == 11 && u.RefreshToken == token);
            Assert.True(exists);
            Assert.False(exists2);

        }

        [Fact]
        public async Task VerifyRefreshToken_InvalidToken_ReturnsFalse()
        {
            // ARRANGE
            await SetupManagerAsync();
            await DbContext.UserRefreshTokens.AddRangeAsync(new UserRefreshToken
            {
                UserId = 10, 
                IsEnabled = true,
                ExpiresOn = DateTime.UtcNow.AddMinutes(5),
                RefreshToken = "sampleToken"
            });
            await DbContext.SaveChangesAsync();

            // ACT
            bool isValid = await manager.VerifyRefreshToken(10, "notASample");

            // ASSERT
            Assert.False(isValid);
        }

        [Fact]
        public async Task VerifyRefreshToken_InvalidUser_ReturnsFalse()
        {
            // ARRANGE
            await SetupManagerAsync();
            await DbContext.UserRefreshTokens.AddRangeAsync(new UserRefreshToken
            {
                UserId = 10,
                IsEnabled = true,
                ExpiresOn = DateTime.UtcNow.AddMinutes(5),
                RefreshToken = "sampleToken"
            });
            await DbContext.SaveChangesAsync();

            // ACT
            bool isValid = await manager.VerifyRefreshToken(11, "sampleToken");

            // ASSERT
            Assert.False(isValid);
        }

        [Fact]
        public async Task VerifyRefreshToken_DisabledToken_ReturnsFalse()
        {
            // ARRANGE
            await SetupManagerAsync();
            await DbContext.UserRefreshTokens.AddRangeAsync(new UserRefreshToken
            {
                UserId = 10,
                IsEnabled = false,
                ExpiresOn = DateTime.UtcNow.AddMinutes(5),
                RefreshToken = "sampleToken"
            });
            await DbContext.SaveChangesAsync();

            // ACT
            bool isValid = await manager.VerifyRefreshToken(10, "sampleToken");

            // ASSERT
            Assert.False(isValid);
        }


        [Fact]
        public async Task VerifyRefreshToken_Expired_ReturnsFalse()
        {
            // ARRANGE
            await SetupManagerAsync();
            await DbContext.UserRefreshTokens.AddRangeAsync(new UserRefreshToken
            {
                UserId = 10,
                IsEnabled = true,
                ExpiresOn = DateTime.UtcNow.AddMinutes(-5),
                RefreshToken = "sampleToken"
            });
            await DbContext.SaveChangesAsync();

            // ACT
            bool isValid = await manager.VerifyRefreshToken(10, "sampleToken");

            // ASSERT
            Assert.False(isValid);
        }

        [Fact]
        public async Task VerifyRefreshToken_Valid_ReturnsTrue()
        {
            // ARRANGE
            await SetupManagerAsync();
            await DbContext.UserRefreshTokens.AddRangeAsync(new UserRefreshToken
            {
                UserId = 10,
                IsEnabled = true,
                ExpiresOn = DateTime.UtcNow.AddMinutes(5),
                RefreshToken = "sampleToken"
            });
            await DbContext.SaveChangesAsync();

            // ACT
            bool isValid = await manager.VerifyRefreshToken(10, "sampleToken");

            // ASSERT
            Assert.True(isValid);
        }

        [Fact]
        public async Task DisableRefreshToken_Vaild_Updates()
        {
            // ARRANGE
            await SetupManagerAsync();
            UserRefreshToken entity = new()
            {
                UserId = 10,
                IsEnabled = true,
                ExpiresOn = DateTime.UtcNow.AddMinutes(5),
                RefreshToken = "sampleToken"
            };
            await DbContext.UserRefreshTokens.AddRangeAsync(entity);
            await DbContext.SaveChangesAsync();

            // ACT
            await manager.DisableRefreshTokenAsync(10, "sampleToken");
            entity = await DbContext.UserRefreshTokens.AsNoTracking().SingleOrDefaultAsync(u => u.Id == entity.Id);

            // ASSERT
            Assert.True(!entity.IsEnabled);
        }

        [Fact]
        public async Task GetUserById_InvalidId_ReturnsNull()
        {
            // ARRANGE
            await SetupManagerAsync();
            UserRefreshToken entity = new()
            {
                UserId = 10,
                IsEnabled = true,
                ExpiresOn = DateTime.UtcNow.AddMinutes(5),
                RefreshToken = "sampleToken"
            };
            await DbContext.UserRefreshTokens.AddRangeAsync(entity);
            await DbContext.SaveChangesAsync();

            // ACT
            int invalidId = entity.Id + 5;
            var res = await manager.GetUserByIdAsync(invalidId);


            // ASSERT 
            Assert.Null(res);
        }

        [Fact]
        public async Task GetUserById_NoUsersInDb_ReturnsNull()
        {
            // ARRANGE
            await SetupManagerAsync();

            // ACT
            var res = await manager.GetUserByIdAsync(5);

            // ASSERT 
            Assert.Null(res);
        }

        [Fact]
        public async Task GetUserById_ValidId_ReturnsEntity()
        {
            // ARRANGE
            await SetupManagerAsync();
            User entity = new()
            {
                CreatedOn = DateTime.UtcNow,
                Email = "something@valid.com"
            };
            await DbContext.Users.AddRangeAsync(entity);
            await DbContext.SaveChangesAsync();

            // ACT
            var res = await manager.GetUserByIdAsync(entity.Id);

            // ASSERT 
            Assert.NotNull(res);
            Assert.Equal("something@valid.com", res.Email);
        }

    }
}
