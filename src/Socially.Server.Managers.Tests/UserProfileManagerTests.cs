using Microsoft.EntityFrameworkCore;
using Socially.Models;
using Socially.Models.Enums;
using Socially.Server.DataAccess;
using Socially.Server.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Socially.Server.Managers.Tests
{
    public class UserProfileManagerTests : TestBase
    {

        private UserProfileManager manager;

        private Task SetupManagerAsync()
        {
            manager = new UserProfileManager(DbContext);
            return Task.CompletedTask;
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
                ProfilePictureId = 20,
                CreatedOn = DateTime.UtcNow,
                Id = 44
            });
            await DbContext.ProfileImages.AddAsync(new ProfileImage
            {
                Id = 21,
                UserId = 44,
                FileName = "hello.png"
            });
            await DbContext.SaveChangesAsync();

            // ACT
            var model = new ProfileUpdateModel
            {
                DateOfBirth = new DateTime(2000, 04, 01),
                FirstName = "Changed",
                ProfilePictureFileName = "hello.png"
            };
            await manager.UpdateAsync(44, model);
            var stored = await DbContext.Users.FindAsync(44);


            // ASSERT
            Assert.NotEqual("Unchanged", stored.FirstName);
            Assert.Equal("Changed", stored.FirstName);
            Assert.NotEqual(20, stored.ProfilePictureId);
            Assert.Equal(21, stored.ProfilePictureId);
        }

        [Fact]
        public async Task Update_InvalidImageAndValidModel_UpdatesWithoutImage()
        {
            // ARRANGE
            await SetupManagerAsync();
            await DbContext.Users.AddAsync(new User
            {
                FirstName = "Unchanged",
                CreatedOn = DateTime.UtcNow,
                Id = 44
            });
            await DbContext.ProfileImages.AddRangeAsync(new ProfileImage
            {
                Id = 213,
                UserId = 12,
                FileName = "hello.png"
            },
            new ProfileImage
            {
                Id = 21,
                UserId = 44,
                FileName = "hi.png"
            });
            await DbContext.SaveChangesAsync();

            // ACT
            var model = new ProfileUpdateModel
            {
                DateOfBirth = new DateTime(2000, 04, 01),
                FirstName = "Changed",
                ProfilePictureFileName = "hello.png"
            };
            await manager.UpdateAsync(44, model);
            var stored = await DbContext.Users.FindAsync(44);


            // ASSERT
            Assert.NotEqual("Unchanged", stored.FirstName);
            Assert.Equal("Changed", stored.FirstName);
            Assert.Null(stored.ProfilePictureId);
        }

        [Fact]
        public async Task Update_RemoveProfilePic_ProfilePicIdIsNull()
        {
            // ARRANGE
            await SetupManagerAsync();
            await DbContext.Users.AddAsync(new User
            {
                FirstName = "Unchanged",
                CreatedOn = DateTime.UtcNow,
                Id = 44,
                ProfilePictureId = 10
            });
            await DbContext.SaveChangesAsync();

            // ACT
            var model = new ProfileUpdateModel
            {
                DateOfBirth = new DateTime(2000, 04, 01),
                FirstName = "Changed",
                ProfilePictureFileName = null
            };
            await manager.UpdateAsync(44, model);
            var stored = await DbContext.Users.FindAsync(44);


            // ASSERT
            Assert.NotEqual("Unchanged", stored.FirstName);
            Assert.Equal("Changed", stored.FirstName);
            Assert.NotEqual(10, stored.ProfilePictureId);
            Assert.Null(stored.ProfilePictureId);
        }

        [Fact]
        public async Task CreateRefreshToken_Creating_Creates()
        {
            // ARRANGE
            await SetupManagerAsync();

            // ACT
            string token = await manager.CreateRefreshTokenAsync(10, TimeSpan.Zero);

            // ASSERT
            var otherUserExists = await DbContext.UserRefreshTokens.AnyAsync(u => u.UserId != 10 && u.RefreshToken == token);
            var exists = await DbContext.UserRefreshTokens.AnyAsync(u => u.UserId == 10 && u.RefreshToken == token);
            Assert.False(otherUserExists, "Refresh token created for different user user");
            Assert.True(exists, "No refresh token created for current user");

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

        [Fact]
        public async Task GetUserNameByEmail_NoUsersInDb_ReturnsNull()
        {
            // ARRANGE
            await SetupManagerAsync();

            // ACT
            var user = await manager.GetUserByEmailAsync("any@email.com");

            // ASSERT
            Assert.Null(user);
        }

        [Fact]
        public async Task GetUserByEmail_InvalidEmail_ReturnsNull()
        {
            // ARRANGE
            await SetupManagerAsync();
            await DbContext.Users.AddAsync(new User
            {
                CreatedOn = DateTime.UtcNow,
                UserName = "user1",
                Email = "email@valid.com"
            });
            await DbContext.SaveChangesAsync();

            // ACT
            var user = await manager.GetUserByEmailAsync("any@email.com");

            // ASSERT
            Assert.Null(user);
        }

        [Fact]
        public async Task GetUserByEmail_ValidEmail_ReturnsUser()
        {
            // ARRANGE
            await SetupManagerAsync();
            await DbContext.Users.AddAsync(new User
            {
                CreatedOn = DateTime.UtcNow,
                UserName = "user1",
                Email = "email@valid.com"
            });
            await DbContext.SaveChangesAsync();

            // ACT
            var user = await manager.GetUserByEmailAsync("email@valid.com");

            // ASSERT
            Assert.NotNull(user);
            Assert.Equal("user1", user.UserName);
        }


        [Fact]
        public async Task GetUserByUsername_NoUsersInDb_ReturnsNull()
        {
            // ARRANGE
            await SetupManagerAsync();

            // ACT
            var user = await manager.GetUserByUsernameAsync("anyuser");

            // ASSERT
            Assert.Null(user);
        }

        [Fact]
        public async Task GetUserByUsername_InvalidUsername_ReturnsNull()
        {
            // ARRANGE
            await SetupManagerAsync();
            await DbContext.Users.AddAsync(new User
            {
                CreatedOn = DateTime.UtcNow,
                UserName = "user1",
                Email = "Username@valid.com"
            });
            await DbContext.SaveChangesAsync();

            // ACT
            var user = await manager.GetUserByUsernameAsync("user2");

            // ASSERT
            Assert.Null(user);
        }

        [Fact]
        public async Task GetUserByUsername_ValidUsername_ReturnsUser()
        {
            // ARRANGE
            await SetupManagerAsync();
            await DbContext.Users.AddAsync(new User
            {
                CreatedOn = DateTime.UtcNow,
                UserName = "user1",
                Email = "Username@valid.com"
            });
            await DbContext.SaveChangesAsync();

            // ACT
            var user = await manager.GetUserByUsernameAsync("user1");

            // ASSERT
            Assert.NotNull(user);
            Assert.Equal("user1", user.UserName);
        }


        [Theory]
        [InlineData("e", 3)]
        [InlineData("E", 3)]
        [InlineData("a", 4)]
        [InlineData("A", 4)]
        [InlineData("kent", 0)]
        [InlineData("uce", 1)]
        public async Task Search_ByQuery_ReturnsFilteredUsers(string q, int expectedCount)
        {
            // ARRANGE
            await SetupManagerAsync();
            int currentUserId = 2;
            await DbContext.AddRangeAsync(new User[]
            {
                new User
                {
                    Id = 1,
                    FirstName = "Bruce",
                    LastName = "Wayne",
                    SentFriendRequests = new List<FriendRequest>
                    {
                        new FriendRequest
                        {
                            ForId = currentUserId
                        }
                    }
                },
                new User
                {
                    Id = 2,
                    FirstName = "Clark",
                    LastName = "Kent"
                },
                new User
                {
                    Id = 3,
                    FirstName = "Diana",
                    LastName = "Prince",
                    RecievedFriendRequests = new List<FriendRequest>
                    {
                        new FriendRequest
                        {
                            RequesterId = currentUserId
                        }
                    }
                },
                new User
                {
                    Id = 4,
                    FirstName = "Hal",
                    LastName = "Jordan",
                    RecievedFriendRequests = new List<FriendRequest>
                    {
                        new FriendRequest
                        {
                            RequesterId = currentUserId,
                            IsAccepted = true
                        },
                        new FriendRequest
                        {
                            RequesterId = 100,
                            IsAccepted = true
                        }
                    }
                },
                new User
                {
                    Id = 5,
                    FirstName = "John",
                    LastName = "Stewart"
                },
                new User
                {
                    Id = 6,
                    FirstName = "J'onn",
                    LastName = "J'onzz",
                    RecievedFriendRequests = new List<FriendRequest>
                    {
                        new FriendRequest
                        {
                            RequesterId = currentUserId,
                            IsAccepted = false
                        }
                    }
                }

            });
            await DbContext.SaveChangesAsync();

            // ACT
            var result = await manager.SearchAsync(currentUserId, q);

            // ASSERT
            Assert.NotNull(result);
            Assert.Equal(expectedCount, result.Count());

            var hal = result.SingleOrDefault(u => u.FirstName == "Hal");
            if (hal != null)
            {
                Assert.Equal(UserFriendState.Friend, hal.FriendState);
            }

            var bruce = result.SingleOrDefault(u => u.FirstName == "Bruce");
            if (bruce != null)
            {
                Assert.Equal(UserFriendState.RecievedRequest, bruce.FriendState);
            }

            var diana = result.SingleOrDefault(u => u.FirstName == "Diana");
            if (diana != null)
            {
                Assert.Equal(UserFriendState.SentRequest, diana.FriendState);
            }

            var mm = result.SingleOrDefault(u => u.FirstName == "J'onn");
            if (mm != null)
            {
                Assert.Equal(UserFriendState.None, mm.FriendState);
            }

        }

        //[Fact]
        //public async Task SearchNonFriends_HasNonFriends_FindsByQuery()
        //{
        //    // ARRANGE
        //    await SetupManagerAsync();
        //    int currentUserId = 10;
        //    await DbContext.Users.AddAsync(new User
        //    {
        //        Id = currentUserId,
        //        CreatedOn = DateTime.UtcNow,
        //        Friends = new Friend[]
        //        {
        //            new Friend
        //            {
        //                FriendUser = new User
        //                {
        //                    CreatedOn = DateTime.UtcNow,
        //                    FirstName = "First",
        //                    LastName = "friend"
        //                }
        //            },
        //            new Friend
        //            {
        //                FriendUser = new User
        //                {
        //                    CreatedOn = DateTime.UtcNow,
        //                    FirstName = "Second",
        //                    LastName = "friend"
        //                }
        //            }
        //        }
        //    });
        //    await DbContext.Users.AddRangeAsync(new User[]
        //    {
        //        new User
        //        {
        //            CreatedOn = DateTime.UtcNow,
        //            FirstName = "somekindda",
        //            LastName = "friends"
        //        },
        //        new User
        //        {
        //            CreatedOn = DateTime.UtcNow,
        //            FirstName = "friendly",
        //            LastName = "dude"
        //        },
        //        new User
        //        {
        //            CreatedOn = DateTime.UtcNow,
        //            FirstName = "sweet",
        //            LastName = "person"
        //        }
        //    });
        //    await DbContext.SaveChangesAsync();

        //    // ACT
        //    var result = await manager.SearchNonFriendsAsync(currentUserId, "FRIEnd");

        //    // ASSERT
        //    Assert.NotNull(result);
        //    Assert.NotEmpty(result);
        //    Assert.Equal(2, result.Count());

        //}


    }
}
