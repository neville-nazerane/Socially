using Microsoft.AspNetCore.Components;
using Socially.Apps.Consumer.Services;
using Socially.Models;
using Socially.Website.Services;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Socially.Website.Components
{
    public partial class PostsDisplay
    {
        [Parameter]
        public ICollection<PostDisplayModel> Posts { get; set; }

        [Inject]
        public IApiConsumer Consumer { get; set; }

        [Inject]
        public CachedContext CachedContext { get; set; }

        UserSummaryModel currentUser;

        //protected override async Task OnInitializedAsync()
        //{
        //}

        protected override async Task OnParametersSetAsync()
        {
            currentUser ??= await CachedContext.GetCurrentProfileInfoAsync();
            if (Posts is not null)
                await CachedContext.UpdateUserProfilesIfNotExistAsync(Posts.GetAllCreatedIds());
        }

        async Task DeleteAsync(int postId)
        {
            var res = await Consumer.DeletePostAsync(postId);
            res.EnsureSuccessStatusCode();
            Posts = Posts.Where(p => p.Id != postId).ToList();
            StateHasChanged();
        }

    }
}
