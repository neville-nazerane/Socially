using Microsoft.AspNetCore.Components;
using Socially.Apps.Consumer.Services;
using Socially.Models;
using Socially.Website.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace Socially.Website.Pages.Profile
{

    public partial class Profile
    {

        [Inject]
        public IApiConsumer Consumer { get; set; }

        [Inject]
        public UserContext UserContext { get; set; }


        bool isPostsLoading;

        AddPostModel addPostModel = new AddPostModel();

        // data fields
        ICollection<PostDisplayModel> posts;
        ProfileUpdateModel profileInfo;


        protected override async Task OnInitializedAsync()
        {
            await RunAllAsync(
                async () => profileInfo = await UserContext.GetProfileInfoAsync(),
                async () => posts = (await Consumer.GetCurrentUserPostsAsync(10)).ToList()
            );
        }

        async Task AddPostAsyc()
        {
            if (string.IsNullOrEmpty(addPostModel.Text)) return;

            int id = await Consumer.AddPostAsync(addPostModel);
            posts.Add(new PostDisplayModel
            {
                Id = id,
                Text = addPostModel.Text
            });
            addPostModel = new AddPostModel();
            StateHasChanged();
        }

        async Task DeletePostAsync(int postId)
        {
            var res = await Consumer.DeletePostAsync(postId);
            res.EnsureSuccessStatusCode();
            posts = posts.Where(p => p.Id != postId).ToList();
            StateHasChanged();
        }

        Task RunAllAsync(params Func<Task>[] tasks)
        {
            return Task.WhenAll(tasks.Select(t => t()));
        }

    }

}
