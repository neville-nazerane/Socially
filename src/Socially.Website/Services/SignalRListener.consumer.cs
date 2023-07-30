using Microsoft.AspNetCore.SignalR.Client;
using Socially.Models;
using Socially.Website.Models.RealtimeEventArgs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Socially.Website.Services
{
    public partial class SignalRListener
    {

        public event EventHandler<CommentAddedEventArgs> OnCommentAdded;
        public event EventHandler<CompletedEventArgs> OnCompleted;

        public void ListenToAll()
        {
            _dataUpdateConn.On("CommentAdded", (int postId, int? parentCommentId, DisplayCommentModel comment) =>
            {
                OnCommentAdded?.Invoke(this, new()
                {
                    PostId = postId,
                    ParentCommentId = parentCommentId,
                    Comment = comment
                });
            });

            _dataUpdateConn.On("Completed", (Guid requestId) =>
            {
                OnCompleted?.Invoke(this, new()
                {
                    RequestId = requestId
                });
            });
        }

        public Task ListenToPostsAsync(IEnumerable<int> postIds) 
            => _dataUpdateConn.InvokeAsync("ListenToPosts", postIds);

        public async Task<Guid> AddCommentAsync(AddCommentModel comment)
        {
            var requestId = Guid.NewGuid();
            await _dataUpdateConn.InvokeAsync("AddComment", requestId, comment);
            return requestId;
        }

    }
}
