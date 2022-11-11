using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.Features;
using Socially.Apps.Consumer.Services;
using Socially.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Socially.Website.Pages.Profile
{
    public partial class Friends
    {
        IEnumerable<UserSummaryModel> friends;
        IEnumerable<UserSummaryModel> requests;
        IEnumerable<SearchedUserModel> searchResults = Array.Empty<SearchedUserModel>();

        bool addNew = false;

        string search;

        [Parameter]
        public int Id { get; set; }

        [Inject]
        public IApiConsumer Consumer { get; set; }

        protected override Task OnInitializedAsync() => RefreshAllAsync();

        void SwapAddNew()
        {
            addNew = !addNew;
        }

        async Task RequestAsync(int userId)
        {
            await Consumer.RequestFriendAsync(userId);
            await RefreshAllAsync();
        }

        async Task RespondAsync(int userId, bool isAccepted)
        {
            await Consumer.RespondToFriendRequestAsync(userId, isAccepted);
            await RefreshAllAsync();
        }

        async Task RefreshAllAsync()
        {
            friends = await Consumer.GetFriendsAsync();
            requests = await Consumer.GetFriendRequestsAsync();
            await SearchAsync();
        }

        async Task SearchAsync()
        {
            if (string.IsNullOrEmpty(search)) searchResults = Array.Empty<SearchedUserModel>();
            else searchResults = await Consumer.SearchUserAsync(search);
        }

    }
}
