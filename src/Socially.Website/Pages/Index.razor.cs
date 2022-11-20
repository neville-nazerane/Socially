using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.Features;
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

        protected override Task OnInitializedAsync()
        {
            return RunAllAsync(
                async () => friendRequests = await Consumer.GetFriendRequestsAsync(),
                async () => currentUser = await CachedContext.GetCurrentProfileInfoAsync(),
                async () => posts = (await Consumer.GetHomePostsAsync(20)).ToList()
                );
        }

        Task RunAllAsync(params Func<Task>[] funcs) => Task.WhenAll(funcs.Select(f => f()));


        async Task RespondFriendRequest(int userId, bool isApproved)
        {
            await Consumer.RespondToFriendRequestAsync(userId, isApproved);
            friendRequests = await Consumer.GetFriendRequestsAsync();
        }
    }
}
