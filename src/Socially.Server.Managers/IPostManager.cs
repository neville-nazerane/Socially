using Socially.Models;
using Socially.Server.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Socially.Server.Managers
{
    public interface IPostManager
    {
        Task<int> AddAsync(AddPostModel addPostModel, CancellationToken cancellationToken = default);
        Task<DisplayCommentModel> AddCommentAsync(AddCommentModel model, CancellationToken cancellationToken = default);
        Task DeleteAsync(int postId, CancellationToken cancellationToken = default);
        Task<Comment> DeleteCommentAsync(int commentId, CancellationToken cancellationToken = default);
        Task<IEnumerable<PostDisplayModel>> GetHomePostsAsync(int pageSize, DateTime? since = null, CancellationToken cancellationToken = default);
        Task<IEnumerable<PostDisplayModel>> GetCurrentUserPostsAsync(int pageSize, DateTime? since = null, CancellationToken cancellationToken = default);
        Task<int> SwapLikeAsync(int postId, int? commentId, CancellationToken cancellationToken = default);
        Task<IEnumerable<PostDisplayModel>> GetProfilePostsAsync(int userId, int pageSize, DateTime? since = null, CancellationToken cancellationToken = default);
        Task<PostDisplayModel> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<int?> GetPostIdForCommentAsync(int commentId, CancellationToken cancellationToken = default);
    }
}