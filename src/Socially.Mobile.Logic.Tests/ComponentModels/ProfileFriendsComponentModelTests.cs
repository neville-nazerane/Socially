﻿using Socially.Apps.Consumer.Services;
using Socially.Mobile.Logic.ComponentModels;
using Socially.Mobile.Logic.Services;
using Socially.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Mobile.Logic.Tests.ComponentModels
{
    public class ProfileFriendsComponentModelTests
    {

        Mock<ISocialLogger> mockedLogger;
        Mock<IApiConsumer> mockedApiConsumer;
        ProfileFriendsComponentModel viewModel;

        void Init()
        {
            mockedLogger = new();
            mockedApiConsumer = new();
            viewModel = new(mockedLogger.Object, mockedApiConsumer.Object);
        }

        [Fact]
        public async Task Get_AllApiCallsResponds_Populates()
        {
            // ARRANGE
            Init();

            #region data
            var friends = new[]
    {
                new UserSummaryModel
                {
                    Id = 77
                }
            };
            var requests = new[]
            {
                new UserSummaryModel
                {
                    Id = 99
                }
            };
            #endregion

            mockedApiConsumer.Setup(c => c.GetFriendsAsync(It.IsAny<CancellationToken>()))
                             .ReturnsAsync(friends);
            mockedApiConsumer.Setup(c => c.GetFriendRequestsAsync(It.IsAny<CancellationToken>()))
                 .ReturnsAsync(requests);

            // ACT
            await viewModel.InitAsync();

            // ASSERT
            Assert.Single(viewModel.Friends);
            Assert.Equal(77, viewModel.Friends.First().Id);

            Assert.Single(viewModel.FriendRequests);
            Assert.Equal(99, viewModel.FriendRequests.First().Id);


            Assert.False(viewModel.IsFriendsLoading);
            Assert.False(viewModel.IsFriendRequestsLoading);

        }


        [Fact]
        public async Task Get_FriendsApiCallFails_PopulatesRest()
        {
            // ARRANGE
            Init();

            #region data
            var ex = new Exception();
            var requests = new[]
            {
                new UserSummaryModel
                {
                    Id = 99
                }
            };
            #endregion

            mockedApiConsumer.Setup(c => c.GetFriendsAsync(It.IsAny<CancellationToken>()))
                             .Throws(ex);
            mockedApiConsumer.Setup(c => c.GetFriendRequestsAsync(It.IsAny<CancellationToken>()))
                 .ReturnsAsync(requests);

            // ACT
            await viewModel.InitAsync();

            // ASSERT

            mockedLogger.Verify(l => l.LogException(ex, It.IsAny<string>()), Times.Once);

            Assert.Null(viewModel.Friends);

            Assert.Single(viewModel.FriendRequests);
            Assert.Equal(99, viewModel.FriendRequests.First().Id);

            Assert.False(viewModel.IsFriendsLoading);
            Assert.False(viewModel.IsFriendRequestsLoading);

        }


        [Fact]
        public async Task Get_RequestsApiCallFails_PopulatesRest()
        {
            // ARRANGE
            Init();

            #region data
            var ex = new Exception();
            var friends = new[]
            {
                new UserSummaryModel
                {
                    Id = 77
                }
            };
            #endregion

            mockedApiConsumer.Setup(c => c.GetFriendsAsync(It.IsAny<CancellationToken>()))
                             .ReturnsAsync(friends);
            mockedApiConsumer.Setup(c => c.GetFriendRequestsAsync(It.IsAny<CancellationToken>()))
                 .Throws(ex);

            // ACT
            await viewModel.InitAsync();

            // ASSERT

            mockedLogger.Verify(l => l.LogException(ex, It.IsAny<string>()), Times.Once);

            Assert.Single(viewModel.Friends);
            Assert.Equal(77, viewModel.Friends.First().Id);

            Assert.Null(viewModel.FriendRequests);

            Assert.False(viewModel.IsFriendsLoading);
            Assert.False(viewModel.IsFriendRequestsLoading);


        }


    }
}