﻿using Microsoft.AspNetCore.Components;
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

    public partial class Profile
    {

        [Inject]
        public IApiConsumer Consumer { get; set; }

        [Inject]
        public CachedContext CachedContext { get; set; }

        bool isPostsLoading;

        AddPostModel addPostModel = new();

        // data fields
        ICollection<PostDisplayModel> posts;


        protected override async Task OnInitializedAsync()
        {
            await RunAllAsync(
                async () => posts = (await Consumer.GetCurrentUserPostsAsync(10)).ToList()
            );
            StateHasChanged();
            var requiredUserIds = posts.GetAllCreatedIds();
            await CachedContext.UpdateUserProfilesIfNotExistAsync(requiredUserIds);
        }

        async Task AddPostAsyc()
        {
            if (string.IsNullOrEmpty(addPostModel.Text)) return;

            int id = await Consumer.AddPostAsync(addPostModel);
            posts.Add(new PostDisplayModel
            {
                Id = id,
                Text = addPostModel.Text,
                CreatorId = (await CachedContext.GetCurrentProfileInfoAsync()).Id
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

        static Task RunAllAsync(params Func<Task>[] tasks)
        {
            return Task.WhenAll(tasks.Select(t => t()));
        }

    }

}
