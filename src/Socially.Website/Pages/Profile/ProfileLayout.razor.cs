using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Socially.Apps.Consumer.Services;
using Socially.Models;
using Socially.Website.Components;
using Socially.Website.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Socially.Website.Pages.Profile
{
    public partial class ProfileLayout
    {

        [Inject]
        public CachedContext CachedContext { get; set; }

        [Inject]
        public IApiConsumer Consumer { get; set; }

        [Inject]
        public IConfiguration Config { get; set; }

        UserSummaryModel profileInfo;
        private IEnumerable<string> images;
        private IEnumerable<UserSummaryModel> friends;

        protected override Task OnInitializedAsync()
        {
            return RunAllAsync(
               async () => profileInfo = await CachedContext.GetCurrentProfileInfoAsync(),
               async () => images = await Consumer.GetAllImagesOfUserAsync(),
               async () => friends = await Consumer.GetFriendsAsync()
            );

            
        }

        Task RunAllAsync(params Func<Task>[] funcs)
            => Task.WhenAll(funcs.Select(f => f()).ToArray());

    }
}
