using Socially.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Socially.Apps.Consumer.Services
{
    public interface IApiConsumer
    {
        Task<int> AddCommentAsync(AddCommentModel model, CancellationToken cancellationToken = default);
        Task<int> AddPostAsync(AddPostModel addPostModel, CancellationToken cancellationToken = default);
        Task<HttpResponseMessage> DeleteCommentAsync(int id, CancellationToken cancellationToken = default);
        Task<HttpResponseMessage> DeleteImageAsync(string fileName, CancellationToken cancellationToken = default);
        Task<HttpResponseMessage> DeletePostAsync(int id, CancellationToken cancellationToken = default);
        Task<HttpResponseMessage> ForgotPasswordAsync(string email, CancellationToken cancellationToken = default);
        Task<IEnumerable<string>> GetAllImagesOfUserAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<PostDisplayModel>> GetCurrentUserPostsAsync(int pageSize, DateTime? since = null, CancellationToken cancellationToken = default);
        Task<IEnumerable<UserSummaryModel>> GetFriendRequestsAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<UserSummaryModel>> GetFriendsAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<PostDisplayModel>> GetHomePostsAsync(int pageSize, DateTime? since = null, CancellationToken cancellationToken = default);
        Task<IEnumerable<PostDisplayModel>> GetProfilePostsAsync(int userId, int pageSize, DateTime? since = null, CancellationToken cancellationToken = default);
        Task<ProfileUpdateModel> GetUpdateProfileAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<UserSummaryModel>> GetUsersByIdsAsync(IEnumerable<int> userIds, CancellationToken cancellationToken = default);
        Task<TokenResponseModel> LoginAsync(LoginModel model, CancellationToken cancellationToken = default);
        Task<int> RemoveFriendAsync(int friendId, CancellationToken cancellationToken = default);
        Task<HttpResponseMessage> RequestFriendAsync(int forId, CancellationToken cancellationToken = default);
        Task<HttpResponseMessage> ResetForgottenPasswordAsync(ForgotPasswordModel model, CancellationToken cancellationToken = default);
        Task<HttpResponseMessage> ResetPasswordAsync(PasswordResetModel model, CancellationToken cancellationToken = default);
        Task<bool> RespondToFriendRequestAsync(int requesterId, bool isAccepted, CancellationToken cancellationToken = default);
        Task<IEnumerable<SearchedUserModel>> SearchUserAsync(string q, CancellationToken cancellationToken = default);
        void SetJwt(string jwtHeader);
        Task SignupAsync(SignUpModel model, CancellationToken cancellationToken = default);
        Task<bool> SwapCommentLikeAsync(int postId, int commentId, CancellationToken cancellationToken = default);
        Task<bool> SwapPostLikeAsync(int postId, CancellationToken cancellationToken = default);
        Task<HttpResponseMessage> UpdateProfileAsync(ProfileUpdateModel model, CancellationToken cancellationToken = default);
        Task<HttpResponseMessage> UploadAsync(ImageUploadModel model, CancellationToken cancellationToken = default);
    }
}