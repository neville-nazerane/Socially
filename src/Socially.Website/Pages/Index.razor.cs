﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.IdentityModel.Tokens;
using Socially.Apps.Consumer.Services;
using Socially.Models;
using Socially.Website.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Socially.Website.Pages
{
    public partial class Index
    {
        private IEnumerable<UserSummaryModel> friendRequests;
        private UserSummaryModel currentUser;
        private ICollection<PostDisplayModel> posts;

        [Inject]
        public IApiConsumer Consumer { get; set; }

        [Inject]
        public CachedContext CachedContext { get; set; }

        [Inject]
        public SignalRListener SignalR { get; set; }

        protected override Task OnInitializedAsync()
        {
            SignalR.OnUserUpdated += OnUserUpdated;
            return RunAllAsync(
                async () => friendRequests = await Consumer.GetFriendRequestsAsync(),
                async () => currentUser = await CachedContext.GetCurrentProfileInfoAsync(),
                GetPostsAsync
            );
        }

        private void OnUserUpdated(object sender, Models.RealtimeEventArgs.UserUpdatedEventArgs e)
        {
            StateHasChanged();
        }

        async Task GetPostsAsync()
        {
            posts = (await Consumer.GetHomePostsAsync(20)).ToList();
            var postIds = posts.Select(p => p.Id).ToList();
            await SignalR.ListenToPostsAsync(postIds);
            var userIds = posts.Select(p => p.CreatorId)
                                .Union(posts.SelectMany(p => p.Comments).Select(c => c.CreatorId))
                                .Distinct();
            await SignalR.ListenToUsersAsync(userIds);
        }

        Task RunAllAsync(params Func<Task>[] funcs) => Task.WhenAll(funcs.Select(f => f()));


        async Task RespondFriendRequest(int userId, bool isApproved)
        {
            await Consumer.RespondToFriendRequestAsync(userId, isApproved);
            friendRequests = await Consumer.GetFriendRequestsAsync();
        }
    
        

    }
}
