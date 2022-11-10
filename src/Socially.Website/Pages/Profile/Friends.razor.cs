using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.Features;
using Socially.Apps.Consumer.Services;
using Socially.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Socially.Website.Pages.Profile
{
    public partial class Friends
    {
        private IEnumerable<UserSummaryModel> friends;
        private IEnumerable<UserSummaryModel> requests;

        [Parameter]
        public int Id { get; set; }

        [Inject]
        public IApiConsumer Consumer { get; set; }

        protected override async Task OnInitializedAsync()
        {
            friends = await Consumer.GetFriendsAsync();
            requests = await Consumer.GetFriendRequestsAsync();


        }


    }
}
