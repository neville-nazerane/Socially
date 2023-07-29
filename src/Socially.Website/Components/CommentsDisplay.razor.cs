using Microsoft.AspNetCore.Components;
using Socially.Apps.Consumer.Services;
using Socially.Models;
using Socially.Website.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Socially.Website.Components
{
    public partial class CommentsDisplay
    {

        bool isShowingLogo = false;

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

        [Inject]
        public SignalRListener SignalRListener { get; set; }


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
            await SignalRListener.AddCommentAsync(addModel);
            //int id = await Consumer.AddCommentAsync(addModel);
            //Comments.Add(new DisplayCommentModel
            //{
            //    Text = addModel.Text,
            //    Id = id,
            //    CreatorId = currentUser.Id
            //});
            //addModel = BuildNewModel();
            StateHasChanged();
        }

        async Task DeleteAsync(DisplayCommentModel comment)
        {
            await Consumer.DeleteCommentAsync(comment.Id);
            Comments.Remove(comment);
            StateHasChanged();
        }

        async Task LikeAsync(DisplayCommentModel comment)
        {
            await Consumer.SwapCommentLikeAsync(PostId, comment.Id);
            if (comment.IsLikedByCurrentUser.Value)
                comment.LikeCount--;
            else
                comment.LikeCount++;
            comment.IsLikedByCurrentUser = !comment.IsLikedByCurrentUser;
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

        void ShowLogo() => isShowingLogo = true;
        void HideLogo() => isShowingLogo = false;

    }
}
