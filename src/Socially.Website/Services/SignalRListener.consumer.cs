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
        public event EventHandler<CommentDeletedEventArgs> OnCommentDelete;
        public event EventHandler<CompletedEventArgs> OnCompleted;
        public event EventHandler<ErrorEventArgs> OnError;

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

            _dataUpdateConn.On("ErrorOccurred", (Guid requestId, string errorMessage) =>
            {
                OnError?.Invoke(this, new()
                {
                    RequestId = requestId,
                    ErrorMessage = errorMessage
                });
            });

            _dataUpdateConn.On("CommentDeleted", (int commentId) =>
            {
                OnCommentDelete?.Invoke(this, new()
                {
                    CommentId = commentId
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

        public async Task<Guid> DeleteCommentAsync(int commentId)
        {
            var requestId = Guid.NewGuid();
            await _dataUpdateConn.InvokeAsync("DeleteComment", requestId, commentId);
            return requestId;
        }

    }
}
