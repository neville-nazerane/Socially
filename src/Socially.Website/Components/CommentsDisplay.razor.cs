﻿using Microsoft.AspNetCore.Components;
using Socially.Apps.Consumer.Services;
using Socially.Models;
using Socially.Website.Services;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Socially.Website.Components
{
    public partial class CommentsDisplay
    {

        [Parameter]
        public int PostId { get; set; }

        [Parameter]
        public int? ParentCommentId { get; set; }

        [Parameter]
        public ICollection<DisplayCommentModel> Comments { get; set; }


        [Inject]
        public CachedContext CachedContext { get; set; }

        [Inject]
        public IApiConsumer Consumer { get; set; }


        AddCommentModel addModel = new();

        UserSummaryModel currentUser;

        protected override async Task OnInitializedAsync()
        {
            currentUser = await CachedContext.GetCurrentProfileInfoAsync();
        }


        protected override void OnParametersSet()
        {
            addModel = BuildNewModel();
        }

        async Task AddCommentAsync()
        {
            if (addModel.Text == null) return;
            int id = await Consumer.AddCommentAsync(addModel);
            Comments.Add(new DisplayCommentModel
            {
                Text = addModel.Text,
                Id = id,
                CreatorId = currentUser.Id
            });
            addModel = BuildNewModel();
            StateHasChanged();
        }

        async Task DeleteAsync(DisplayCommentModel comment)
        {
            await Consumer.DeleteCommentAsync(comment.Id);
            Comments.Remove(comment);
            StateHasChanged();
        }

        AddCommentModel BuildNewModel() => new AddCommentModel
        {
            PostId = PostId,
            ParentCommentId = ParentCommentId
        };

        void ShowComments(DisplayCommentModel comment)
        {
            comment.Comments ??= new List<DisplayCommentModel>();
        }

    }
}