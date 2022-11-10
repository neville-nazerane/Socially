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

        protected override async Task OnInitializedAsync()
        {
            friends = await Consumer.GetFriendsAsync();
            requests = await Consumer.GetFriendRequestsAsync();
        }

        void SwapAddNew()
        {
            addNew = !addNew;
        }

        async Task SearchAsync()
        {
            if (search is null) searchResults = Array.Empty<SearchedUserModel>();
            searchResults = await Consumer.SearchUserAsync(search);
        }

    }
}
