using Microsoft.AspNetCore.Components;
using Socially.Apps.Consumer.Services;
using Socially.Models;
using Socially.Website.Models.RealtimeEventArgs;
using Socially.Website.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Socially.Website.Components
{
    public partial class CommentsDisplay : IDisposable
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

        SemaphoreSlim locker = new(1, 1);
        bool isAddCommentLoading;
        Guid addCommentRequestId;
        string addCommentErrorMessage;

        bool isDeleting;
        Guid deletingRequestId;
        string deleteErrorMessage;

        Guid likeRequestId;
        bool isLiking;

        protected override async Task OnInitializedAsync()
        {
            SignalRListener.OnCommentAdded += CommentAdded;
            SignalRListener.OnCompleted += OnCompleted;
            SignalRListener.OnError += OnError;
            SignalRListener.OnCommentDelete += OnDeleted;
            SignalRListener.OnLiked += OnLiked;
            currentUser = await CachedContext.GetCurrentProfileInfoAsync();
        }

        private void OnLiked(object sender, LikedEventArgs e)
        {
            if (e.PostId == PostId)
            {
                var comment = Comments.SingleOrDefault(c => c.Id == e.CommentId);
                if (comment is not null)
                {
                    comment.LikeCount = e.LikeCount;
                    StateHasChanged();
                }
            }
        }

        private void OnDeleted(object sender, CommentDeletedEventArgs e)
        {
            var comment = Comments.SingleOrDefault(c => c.Id == e.CommentId);
            if (comment is not null)
            {
                Comments.Remove(comment);
                StateHasChanged();
            }
        }

        private async void OnError(object sender, ErrorEventArgs e)
        {
            await locker.WaitAsync();
            try
            {
                if (addCommentRequestId == e.RequestId)
                    addCommentErrorMessage = e.ErrorMessage;
            }
            finally
            {
                locker.Release();
            }
        }

        private async void OnCompleted(object sender, CompletedEventArgs e)
        {
            await locker.WaitAsync();
            try
            {
                if (addCommentRequestId.Equals(e.RequestId))
                {
                    isAddCommentLoading = false;
                    if (addCommentErrorMessage is null)
                        addModel = BuildNewModel();
                }
                else if (deletingRequestId.Equals(e.RequestId))
                {
                    isDeleting = false;
                }
                else if (likeRequestId == e.RequestId)
                {
                    isLiking = false;
                }
                else
                    return;

                StateHasChanged();
            }
            finally
            {
                locker.Release();
            }
        }

        private void CommentAdded(object sender, CommentAddedEventArgs e)
        {
            if (PostId == e.PostId && ParentCommentId == e.ParentCommentId)
                Comments.Add(e.Comment);
            StateHasChanged();
        }


        protected override void OnParametersSet()
        {
            addModel = BuildNewModel();
        }

        async Task AddCommentAsync()
        {
            await locker.WaitAsync();
            try
            {
                if (addModel.Text == null) return;
                addCommentErrorMessage = null;
                addCommentRequestId = await SignalRListener.AddCommentAsync(addModel);
                isAddCommentLoading = true;
            }
            finally
            {
                locker.Release();
            }
        }

        async Task DeleteAsync(DisplayCommentModel comment)
        {
            await locker.WaitAsync();
            try
            {
                deleteErrorMessage = null;
                deletingRequestId = await SignalRListener.DeleteCommentAsync(comment.Id);
                isDeleting = true;
            }
            finally
            {
                locker.Release();
            }
            //await Consumer.DeleteCommentAsync(comment.Id);
            //Comments.Remove(comment);
            //StateHasChanged();
        }

        async Task LikeAsync(DisplayCommentModel comment)
        {
            isLiking = true;
            likeRequestId = await SignalRListener.LikePostOrCommentAsync(PostId, comment.Id);

            //await Consumer.SwapCommentLikeAsync(PostId, comment.Id);
            //if (comment.IsLikedByCurrentUser.Value)
            //    comment.LikeCount--;
            //else
            //    comment.LikeCount++;
            //comment.IsLikedByCurrentUser = !comment.IsLikedByCurrentUser;
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

        public void Dispose()
        {
            SignalRListener.OnCommentAdded -= CommentAdded;
            SignalRListener.OnCompleted -= OnCompleted;
            SignalRListener.OnError -= OnError;
            SignalRListener.OnCommentDelete -= OnDeleted;
            SignalRListener.OnLiked += OnLiked;
        }
    }
}
