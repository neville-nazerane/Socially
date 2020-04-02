using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Socially.Server.Managers.Tests
{
    public class UserVerificationManagerTests : TestBase
    {

        private UserVerificationManager manager;

        private void SetupManager()
        {
            manager = new UserVerificationManager(DbContext);
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
            await DbContext.Users.AddAsync(new Core.User { 
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
            await DbContext.Users.AddAsync(new Core.User
            {
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
            await DbContext.Users.AddAsync(new Core.User { 
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
            await DbContext.Users.AddAsync(new Core.User
            {
                UserName = "myUser"
            });
            await DbContext.SaveChangesAsync();

            // ACT
            bool result = await manager.UserNameExistsAsync("myUser");

            // ASSERT
            Assert.True(result);
        }


    }
}
