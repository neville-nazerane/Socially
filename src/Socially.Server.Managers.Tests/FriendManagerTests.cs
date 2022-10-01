using Castle.Core.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Socially.Models;
using Socially.Server.Entities;
using Socially.Server.Managers.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace Socially.Server.Managers.Tests
{
    public class FriendManagerTests : TestBase
    {
        private Mock<ILogger<FriendManager>> mockedLogger;
        private FriendManager manager;

        private ValueTask SetupManagerAsync()
        {
            mockedLogger = new Mock<ILogger<FriendManager>>();
            manager = new FriendManager(DbContext, mockedLogger.Object);
            return ValueTask.CompletedTask;
        }

        [Fact, Trait("action", "add"), Trait("happyPath", "fail")]
        public async Task Request_AlreadyRequestedByCurrentUser_Throws()
        {
            // ARRANGE
            int currentUserId = 10;
            await SetupManagerAsync();
            DateTime createdTime = DateTime.UtcNow;
            await DbContext.FriendRequests.AddAsync(new Entities.FriendRequest
            {
                ForId = 8,
                RequesterId = currentUserId,
                RequestedOn = createdTime
            });
            await DbContext.SaveChangesAsync();

            // ACT & ASSERT
            var exception = await Assert.ThrowsAsync<FriendRequestExistsException>(() => manager.RequestAsync(currentUserId, 8));
            Assert.Equal(currentUserId, exception.Model.RequesterId);
            Assert.Equal(createdTime, exception.Model.RequestedOn);

        }

        [Fact, Trait("action", "add"), Trait("happyPath", "fail")]
        public async Task Request_AlreadyRequestedByOtherUser_Throws()
        {
            // ARRANGE
            int currentUserId = 10;
            await SetupManagerAsync();
            DateTime createdTime = DateTime.UtcNow;
            await DbContext.FriendRequests.AddAsync(new Entities.FriendRequest
            {
                ForId = currentUserId,
                RequesterId = 8,
                RequestedOn = createdTime
            });
            await DbContext.SaveChangesAsync();

            // ACT & ASSERT
            var exception = await Assert.ThrowsAsync<FriendRequestExistsException>(() => manager.RequestAsync(currentUserId, 8));
            Assert.Equal(8, exception.Model.RequesterId);
            Assert.Equal(createdTime, exception.Model.RequestedOn);
        }

        [Fact, Trait("action", "add"), Trait("happyPath", "success")]
        public async Task Request_RequestDoesntExist_Requests()
        {
            // ARRANGE
            await SetupManagerAsync();
            int currentUserId = 10;

            // ACT
            await manager.RequestAsync(currentUserId, 11);

            // ASSERT
            var requests = await DbContext.FriendRequests.ToListAsync();
            Assert.NotEmpty(requests);
            Assert.Single(requests);

            var request = requests.First();
            Assert.Equal(currentUserId, request.RequesterId);
            Assert.Equal(11, request.ForId);

        }

        [Fact, Trait("action", "add"), Trait("happyPath", "success")]
        public async Task Request_CurrentUserRequestedOthers_Requests()
        {
            // ARRANGE
            await SetupManagerAsync();
            int currentUserId = 10;
            await DbContext.FriendRequests.AddAsync(new Entities.FriendRequest
            {
                ForId = 8,
                RequesterId = currentUserId,
                RequestedOn = DateTime.UtcNow
            });
            await DbContext.SaveChangesAsync();

            // ACT
            await manager.RequestAsync(currentUserId, 11);

            // ASSERT
            var requests = await DbContext.FriendRequests.ToListAsync();
            Assert.NotEmpty(requests);
            Assert.Equal(2, requests.Count);

        }

        [Fact, Trait("action", "add"), Trait("happyPath", "success")]
        public async Task Request_CurrentUserHasOtherRequests_Requests()
        {
            // ARRANGE
            await SetupManagerAsync();
            int currentUserId = 10;
            await DbContext.FriendRequests.AddAsync(new Entities.FriendRequest
            {
                ForId = currentUserId,
                RequesterId = 8,
                RequestedOn = DateTime.UtcNow
            });
            await DbContext.SaveChangesAsync();

            // ACT
            await manager.RequestAsync(currentUserId, 11);

            // ASSERT
            var requests = await DbContext.FriendRequests.ToListAsync();
            Assert.NotEmpty(requests);
            Assert.Equal(2, requests.Count);

        }


        [Fact]
        public async Task Respond_RequestDoesntExist_Warns()
        {
            // ARRANGE
            await SetupManagerAsync();

            // ACT
            bool success = await manager.RespondAsync(10, 20, true);

            // ASSERT
            Assert.False(success);
            mockedLogger.VerifyLog(l => l.LogWarning(It.IsAny<string>(), It.IsIn(10), It.IsIn(20)));
        }

        [Fact]
        public async Task Respond_RequestedByCurrent_Warns()
        {
            // ARRANGE
            await SetupManagerAsync();
            int currentUserId = 10;
            await DbContext.FriendRequests.AddAsync(new Entities.FriendRequest
            {
                RequestedOn = DateTime.UtcNow,
                RequesterId = currentUserId,
                ForId = 11
            });
            await DbContext.SaveChangesAsync();

            // ACT
            bool success = await manager.RespondAsync(11, currentUserId, true);

            // ASSERT
            Assert.False(success);
            mockedLogger.VerifyLog(l => l.LogWarning(It.IsAny<string>(), It.IsIn(11), It.IsIn(currentUserId)));

        }

        [Fact]
        public async Task Respond_RequestedByDifferentUser_Warns()
        {
            // ARRANGE
            await SetupManagerAsync();
            int currentUserId = 10;
            await DbContext.FriendRequests.AddAsync(new Entities.FriendRequest
            {
                RequestedOn = DateTime.UtcNow,
                RequesterId = 31,
                ForId = currentUserId
            });
            await DbContext.SaveChangesAsync();

            // ACT
            bool success = await manager.RespondAsync(11, currentUserId, true);

            // ASSERT
            Assert.False(success);
            mockedLogger.VerifyLog(l => l.LogWarning(It.IsAny<string>(), It.IsIn(11), It.IsIn(currentUserId)));

        }

        [Fact]
        public async Task Respond_RejectRequestByTargetUser_MarksAsRejected()
        {
            // ARRANGE
            await SetupManagerAsync();
            int currentUserId = 10;
            await DbContext.FriendRequests.AddAsync(new Entities.FriendRequest
            {
                RequestedOn = DateTime.UtcNow,
                RequesterId = 11,
                ForId = currentUserId
            });
            await DbContext.SaveChangesAsync();

            // ACT
            bool success = await manager.RespondAsync(11, currentUserId, false);

            // ASSERT
            Assert.True(success);

            var requests = await DbContext.FriendRequests.SingleAsync();
            Assert.False(requests.IsAccepted);

            var friends = await DbContext.Friends.ToListAsync();
            Assert.Empty(friends);
        }

        [Fact]
        public async Task Respond_ApproveRequestByTargetUser_CreatesFriend()
        {
            // ARRANGE
            await SetupManagerAsync();
            int currentUserId = 10;
            await DbContext.FriendRequests.AddAsync(new Entities.FriendRequest
            {
                RequestedOn = DateTime.UtcNow,
                RequesterId = 11,
                ForId = currentUserId
            });
            await DbContext.SaveChangesAsync();

            // ACT
            bool success = await manager.RespondAsync(11, currentUserId, true);

            // ASSERT
            Assert.True(success);

            var requests = await DbContext.FriendRequests.SingleAsync();
            Assert.True(requests.IsAccepted);

            var friends = await DbContext.Friends.ToListAsync();
            Assert.NotEmpty(friends);
            Assert.Equal(2, friends.Count);

            var friend1 = friends.SingleOrDefault(f => f.OwnerUserId == currentUserId);
            Assert.NotNull(friend1);
            Assert.Equal(11, friend1.FriendUserId);

            var friend2 = friends.SingleOrDefault(f => f.OwnerUserId == 11);
            Assert.NotNull(friend2);
            Assert.Equal(currentUserId, friend2.FriendUserId);

        }

        [Fact]
        public async Task SearchNonFriends_HasNonFriends_FindsByQuery()
        {
            // ARRANGE
            await SetupManagerAsync();
            int currentUserId = 10;
            await DbContext.Users.AddAsync(new User
            {
                Id = currentUserId,
                CreatedOn = DateTime.UtcNow,
                Friends = new Friend[]
                {
                    new Friend
                    {
                        FriendUser = new User
                        {
                            CreatedOn = DateTime.UtcNow,
                            FirstName = "First",
                            LastName = "friend"
                        }
                    },
                    new Friend
                    {
                        FriendUser = new User
                        {
                            CreatedOn = DateTime.UtcNow,
                            FirstName = "Second",
                            LastName = "friend"
                        }
                    }
                }
            });
            await DbContext.Users.AddRangeAsync(new User[]
            {
                new User
                {
                    CreatedOn = DateTime.UtcNow,
                    FirstName = "somekindda",
                    LastName = "friends"
                },
                new User
                {
                    CreatedOn = DateTime.UtcNow,
                    FirstName = "friendly",
                    LastName = "dude"
                },
                new User
                {
                    CreatedOn = DateTime.UtcNow,
                    FirstName = "sweet",
                    LastName = "person"
                }
            });
            await DbContext.SaveChangesAsync();

            // ACT
            var result = await manager.SearchNonFriendsAsync(currentUserId, "FRIEnd");

            // ASSERT
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count());

        }

        [Fact]
        public async Task GetRequests_HasData_GetsOnlyForCurrentUser()
        {
            // ARRANGE
            await SetupManagerAsync();
            int currentUserId = 40;

            // adding users
            #region setting data
            await DbContext.Users.AddRangeAsync(
                new User
                {
                    CreatedOn = DateTime.UtcNow,
                    Id = 11,
                    FirstName = "ele",
                    LastName = "evn",
                    ProfilePicture = new ProfileImage
                    {
                        FileName = "someName"
                    }
                },
                new User
                {
                    CreatedOn = DateTime.UtcNow,
                    Id = 12,
                    FirstName = "twel",
                    LastName = "ve"
                },
                new User
                {
                    CreatedOn = DateTime.UtcNow,
                    Id = 22,
                    FirstName = "twenty",
                    LastName = "two"
                },
                new User
                {
                    CreatedOn = DateTime.UtcNow,
                    Id = 30,
                    FirstName = "thir",
                    LastName = "ty"
                },
                new User
                {
                    CreatedOn = DateTime.UtcNow,
                    Id = 32,
                    FirstName = "thirty",
                    LastName = "two"
                },
                new User
                {
                    CreatedOn = DateTime.UtcNow,
                    Id = currentUserId,
                    FirstName = "current",
                    LastName = "user"
                });

            await DbContext.FriendRequests.AddRangeAsync(new FriendRequest
            {
                RequestedOn = DateTime.UtcNow,
                RequesterId = 11,
                ForId = currentUserId
            },
            new FriendRequest
            {
                RequestedOn = DateTime.UtcNow,
                RequesterId = 12,
                ForId = currentUserId,
            },
            new FriendRequest
            {
                RequestedOn = DateTime.UtcNow,
                RequesterId = currentUserId,
                ForId = 22
            },
            new FriendRequest
            {
                RequestedOn = DateTime.UtcNow,
                RequesterId = 32,
                ForId = 30
            });
            await DbContext.SaveChangesAsync();
            #endregion

            // ACT
            var result = await manager.GetRequestsAsync(currentUserId);

            // ASSERT
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count());

        }

        [Fact]
        public async Task GetRequests_HasRespondedRequests_ShowsOnlyUnApproved()
        {
            // ARRANGE
            await SetupManagerAsync();
            int currentUserId = 40;

            // adding users
            #region setting data
            await DbContext.Users.AddRangeAsync(new User[]
            {
                 new User
                {
                    CreatedOn = DateTime.UtcNow,
                    Id = 11,
                    FirstName = "ele",
                    LastName = "evn",
                    ProfilePicture = new ProfileImage
                    {
                        FileName = "someName"
                    }
                },
                new User
                {
                    CreatedOn = DateTime.UtcNow,
                    Id = 12,
                    FirstName = "twel",
                    LastName = "ve"
                },
                new User
                {
                    CreatedOn = DateTime.UtcNow,
                    Id = 22,
                    FirstName = "twenty",
                    LastName = "two"
                },
                new User
                {
                    CreatedOn = DateTime.UtcNow,
                    Id = 30,
                    FirstName = "thir",
                    LastName = "ty"
                },
                new User
                {
                    CreatedOn = DateTime.UtcNow,
                    Id = 32,
                    FirstName = "thirty",
                    LastName = "two"
                },
                new User
                {
                    CreatedOn = DateTime.UtcNow,
                    Id = currentUserId,
                    FirstName = "current",
                    LastName = "user"
                }
            });
            await DbContext.FriendRequests.AddRangeAsync(new FriendRequest[]
            {
                new FriendRequest
                {
                    RequestedOn = DateTime.UtcNow,
                    RequesterId = 11,
                    ForId = currentUserId
                },
                new FriendRequest
                {
                    RequestedOn = DateTime.UtcNow,
                    RequesterId = 12,
                    ForId = currentUserId,
                },
                new FriendRequest
                {
                    RequestedOn = DateTime.UtcNow,
                    RequesterId = 12,
                    ForId = currentUserId,
                },
                new FriendRequest
                {
                    RequestedOn = DateTime.UtcNow,
                    RequesterId = 12,
                    ForId = currentUserId,
                    IsAccepted = false,
                },
                new FriendRequest
                {
                    RequestedOn = DateTime.UtcNow,
                    RequesterId = 12,
                    ForId = currentUserId,
                    IsAccepted = true
                }
            });
            await DbContext.SaveChangesAsync();
            #endregion

            // ACT
            var result = await manager.GetRequestsAsync(currentUserId);

            // ASSERT
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(3, result.Count());

        }

        [Fact]
        public async Task GetFriends_HasData_ShowsOnlyCUrrentUserFriends()
        {
            // ARRANGE
            await SetupManagerAsync();
            int currentUserId = 40;

            // adding users
            #region setting data
            await DbContext.Users.AddRangeAsync(
                new User
                {
                    CreatedOn = DateTime.UtcNow,
                    Id = 11,
                    FirstName = "ele",
                    LastName = "evn",
                    ProfilePicture = new ProfileImage
                    {
                        FileName = "someName"
                    }
                },
                new User
                {
                    CreatedOn = DateTime.UtcNow,
                    Id = 12,
                    FirstName = "twel",
                    LastName = "ve"
                },
                new User
                {
                    CreatedOn = DateTime.UtcNow,
                    Id = 22,
                    FirstName = "twenty",
                    LastName = "two"
                },
                new User
                {
                    CreatedOn = DateTime.UtcNow,
                    Id = 30,
                    FirstName = "thir",
                    LastName = "ty"
                },
                new User
                {
                    CreatedOn = DateTime.UtcNow,
                    Id = 32,
                    FirstName = "thirty",
                    LastName = "two"
                },
                new User
                {
                    CreatedOn = DateTime.UtcNow,
                    Id = currentUserId,
                    FirstName = "current",
                    LastName = "user"
                });

            await DbContext.Friends.AddRangeAsync(
                new Friend
                {
                    FriendUserId = 11,
                    OwnerUserId = currentUserId
                },
                new Friend
                {
                    FriendUserId = 12,
                    OwnerUserId = currentUserId,
                },
                new Friend
                {
                    FriendUserId = currentUserId,
                    OwnerUserId = 22
                },
                new Friend
                {
                    FriendUserId = 32,
                    OwnerUserId = 30
                });
            await DbContext.SaveChangesAsync();
            #endregion

            // ACT
            var result = await manager.GetFriendsAsync(currentUserId);

            // ASSERT
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count());

        }

    }
}
