using Microsoft.AspNetCore.SignalR.Client;
using Socially.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Socially.Website.Services
{
    public partial class SignalRListener
    {

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
