using Microsoft.AspNetCore.Components;
using Socially.Apps.Consumer.Services;
using Socially.ClientUtils;
using Socially.Models;
using Socially.Website.Models;
using Socially.Website.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace Socially.Website.Pages.Profile
{

    public partial class Profile : IDisposable
    {

        [Inject]
        public IApiConsumer Consumer { get; set; }

        [Inject]
        public CachedContext CachedContext { get; set; }

        [Inject]
        public SignalRListener SignalRListener { get; set; }

        bool isPostsLoading;

        // data fields
        ICollection<PostDisplayModel> posts;


        protected override async Task OnInitializedAsync()
        {
            SignalRListener.OnPostDeleted += OnPostDeleted;

            await RunAllAsync(
                async () => posts = (await Consumer.GetCurrentUserPostsAsync(10)).ToList()
            );
            StateHasChanged();
            var postIds = posts.Select(p => p.Id).ToList();
            await SignalRListener.ListenToPostsAsync(postIds);
            var requiredUserIds = posts.GetAllCreatedIds();
            await CachedContext.UpdateUserProfilesIfNotExistAsync(requiredUserIds);
        }

        private void OnPostDeleted(object sender, Models.RealtimeEventArgs.PostDeletedEventArgs e)
        {
            var deletedPost = posts.SingleOrDefault(p => p.Id == e.PostId);
            if (deletedPost is not null)
                posts.Remove(deletedPost);
            StateHasChanged();
        }

        static Task RunAllAsync(params Func<Task>[] tasks)
        {
            return Task.WhenAll(tasks.Select(t => t()));
        }

        public void Dispose()
        {
            SignalRListener.OnPostDeleted -= OnPostDeleted;
        }
    }

}
