using Socially.Core.Entities;
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

        private void SetupManager()
        {
            manager = new UserProfileManager(DbContext);
        }

        [Fact]
        public async Task EmailExists_EmptyTable_ReturnsFalse()
        {
            // ARRANGE
            SetupManager();

            // ACT
            bool result = await manager.EmailExistsAsync("Doesnt@matter.com");

            // ASSERT
            Assert.False(result);
        }

        [Fact]
        public async Task EmailExists_NonExistingEmail_ReturnsFalse()
        {
            // ARRANGE
            SetupManager();
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
            SetupManager();
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
            SetupManager();

            // ACT
            bool result = await manager.UserNameExistsAsync("myuser");

            // ASSERT
            Assert.False(result);
        }

        [Fact]
        public async Task UserNameExists_NonExistingUserName_ReturnsFalse()
        {
            // ARRANGE
            SetupManager();
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
            SetupManager();
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
            SetupManager();

            // ACT
            var profile = await manager.GetUpdatableProfileAsync(100);

            // ASSERT
            Assert.Null(profile);
        }

        [Fact]
        public async Task GetUpdatableProfile_NonExistantId_ReturnsNull()
        {

            // ARRANGE
            SetupManager();
            DbContext.Users.Add(new User
            {
                UserName = "sampleUser",
                Id = 40
            });

            // ACT
            var profile = await manager.GetUpdatableProfileAsync(100);

            // ASSERT
            Assert.Null(profile);
        }

        [Fact]
        public async Task GetUpdatableProfile_ValidId_ReturnsProfile()
        {
            // ARRANGE
            SetupManager();
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

    }
}
