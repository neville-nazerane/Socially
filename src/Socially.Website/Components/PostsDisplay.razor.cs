using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Socially.Apps.Consumer.Services;
using Socially.ClientUtils;
using Socially.Models;
using Socially.Website.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Socially.Website.Components
{
    public partial class PostsDisplay : IDisposable
    {
        [Parameter]
        public ICollection<PostDisplayModel> Posts { get; set; }

        [Inject]
        public IApiConsumer Consumer { get; set; }

        [Inject]
        public CachedContext CachedContext { get; set; }

        [Inject]
        public SignalRListener SignalRListener { get; set; }

        [Inject]
        public ICachedStorage<int, PostDisplayModel> PostsCache { get; set; }

        UserSummaryModel currentUser;

        Guid? likeRequestId;
        bool isLiking;

        Guid? deleteReqId;
        bool isDeleting;


        protected override void OnInitialized()
        {
            SignalRListener.OnLiked += OnLiked;
            SignalRListener.OnCompleted += OnCompleted;
        }

        private void OnCompleted(object sender, Models.RealtimeEventArgs.CompletedEventArgs e)
        {
            if (e.RequestId == likeRequestId)
            {
                isLiking = false;
                likeRequestId = null;
            }
            else if (e.RequestId == deleteReqId)
            {
                deleteReqId = null;
                isDeleting = false;
            }
        }

        private void OnLiked(object sender, Models.RealtimeEventArgs.LikedEventArgs e)
        {
            if (e.CommentId is null)
            {
                var post = Posts.SingleOrDefault(p => p.Id == e.PostId);
                if (post is not null)
                {
                    post.LikeCount = e.LikeCount;
                    StateHasChanged(); 
                }
            }
        }

        protected override async Task OnParametersSetAsync()
        {
            currentUser ??= await CachedContext.GetCurrentProfileInfoAsync();
            if (Posts is not null)
            {
                await PostsCache.UpdateAsync(Posts);
                await CachedContext.UpdateUserProfilesIfNotExistAsync(Posts.GetAllCreatedIds());
            }
        }

        async Task DeleteAsync(int postId)
        {
            isDeleting = true;
            deleteReqId = await SignalRListener.DeletePostAsync(postId);
        }

        async Task LikeAsync(PostDisplayModel post)
        {
            isLiking = true;
            likeRequestId = await SignalRListener.LikePostOrCommentAsync(post.Id, null);
            //await Consumer.SwapPostLikeAsync(post.Id);
            //if (post.IsLikedByCurrentUser)
            //    post.LikeCount--;
            //else
            //    post.LikeCount++;
            //post.IsLikedByCurrentUser = !post.IsLikedByCurrentUser;
        }

        public void Dispose()
        {
            SignalRListener.OnLiked -= OnLiked;
        }
    }
}
