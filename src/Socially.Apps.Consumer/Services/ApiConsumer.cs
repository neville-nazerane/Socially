using Socially.Apps.Consumer.Utils;
using Socially.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Socially.Apps.Consumer.Services
{
    public class ApiConsumer : IApiConsumer
    {
        private readonly HttpClient _httpClient;

        public ApiConsumer(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public void SetJwt(string jwtHeader)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", jwtHeader);
        }

        public async Task SignupAsync(SignUpModel model,
                                CancellationToken cancellationToken = default)
        {
            var res = await _httpClient.PostAsJsonAsync("signup",
                                                        model,
                                                        cancellationToken);
            res.EnsureSuccessStatusCode();
        }

        public async Task<TokenResponseModel> LoginAsync(LoginModel model,
                                     CancellationToken cancellationToken = default)
        {
            var res = await _httpClient.PostAsJsonAsync("login",
                                                        model,
                                                        cancellationToken);
            res.EnsureSuccessStatusCode();
            return await res.Content.ReadFromJsonAsync<TokenResponseModel>(cancellationToken: cancellationToken);
        }

        public Task<ProfileUpdateModel> GetUpdateProfileAsync(CancellationToken cancellationToken = default)
            => _httpClient.GetFromJsonAsync<ProfileUpdateModel>("profile", cancellationToken);

        public Task<HttpResponseMessage> UpdateProfileAsync(ProfileUpdateModel model, CancellationToken cancellationToken = default)
            => _httpClient.PutAsJsonAsync("profile", model, cancellationToken);

        public Task<HttpResponseMessage> ResetPasswordAsync(PasswordResetModel model, CancellationToken cancellationToken = default)
            => _httpClient.PutAsJsonAsync("profile/resetPassword", model, cancellationToken);

        public Task<HttpResponseMessage> ResetForgottenPasswordAsync(ForgotPasswordModel model, CancellationToken cancellationToken = default)
            => _httpClient.PutAsJsonAsync("resetForgottenPassword", model, cancellationToken);

        public Task<HttpResponseMessage> ForgotPasswordAsync(string email, CancellationToken cancellationToken = default)
            => _httpClient.PostAsync($"forgotPassword/{email}", null, cancellationToken);

        public Task<HttpResponseMessage> UploadAsync(ImageUploadModel model, CancellationToken cancellationToken = default)
            => _httpClient.PostAsync("image", model.MakeForm(), cancellationToken);

        public Task<IEnumerable<string>> GetAllImagesOfUserAsync(CancellationToken cancellationToken = default)
            => _httpClient.GetFromJsonAsync<IEnumerable<string>>("images", cancellationToken);

        public Task<HttpResponseMessage> DeleteImageAsync(string fileName, 
                                                          CancellationToken cancellationToken = default)
            => _httpClient.DeleteAsync($"image/{fileName}", cancellationToken);

        #region users

        public async Task<IEnumerable<UserSummaryModel>> GetUsersByIdsAsync(IEnumerable<int> userIds,
                                                                            CancellationToken cancellationToken = default)
        {
            var res = await _httpClient.PostAsJsonAsync("users/getById", userIds, cancellationToken);
            res.EnsureSuccessStatusCode();
            return await res.Content.ReadFromJsonAsync<IEnumerable<UserSummaryModel>>();
        }

        public Task<IEnumerable<SearchedUserModel>> SearchUserAsync(string q, CancellationToken cancellationToken = default)
            => _httpClient.GetFromJsonAsync<IEnumerable<SearchedUserModel>>($"users?q={q}", cancellationToken);

        #endregion

        #region friends

        public Task<HttpResponseMessage> RequestFriendAsync(int forId, CancellationToken cancellationToken = default)
            => _httpClient.PostAsync($"friend/request/{forId}", null, cancellationToken);

        public async Task<bool> RespondToFriendRequestAsync(int requesterId,
                                bool isAccepted,
                                CancellationToken cancellationToken = default)
        {
            var res = await _httpClient.PutAsync($"friend/respond/{requesterId}/{isAccepted}", null, cancellationToken);
            res.EnsureSuccessStatusCode();
            return bool.Parse(await res.Content.ReadAsStringAsync(cancellationToken));
        }

        public Task<IEnumerable<UserSummaryModel>> GetFriendRequestsAsync(CancellationToken cancellationToken = default)
            => _httpClient.GetFromJsonAsync<IEnumerable<UserSummaryModel>>("friend/requests", cancellationToken);

        public Task<IEnumerable<UserSummaryModel>> GetFriendsAsync(CancellationToken cancellationToken = default)
            => _httpClient.GetFromJsonAsync<IEnumerable<UserSummaryModel>>("friends", cancellationToken);

        #endregion



        #region posts

        public async Task<int> AddPostAsync(AddPostModel addPostModel,
                                        CancellationToken cancellationToken = default)
        {
            var res = await _httpClient.PostAsJsonAsync("post", addPostModel, cancellationToken);
            res.EnsureSuccessStatusCode();
            return int.Parse(await res.Content.ReadAsStringAsync(cancellationToken));
        }

        public Task<HttpResponseMessage> DeletePostAsync(int id,
                         CancellationToken cancellationToken = default)
            => _httpClient.DeleteAsync($"/post/{id}", cancellationToken);

        public async Task<int> AddCommentAsync(AddCommentModel model,
                                                         CancellationToken cancellationToken = default)
        {
            var res = await _httpClient.PostAsJsonAsync("post/comment", model, cancellationToken);
            res.EnsureSuccessStatusCode();
            return int.Parse(await res.Content.ReadAsStringAsync());
        }

        public Task<HttpResponseMessage> DeleteCommentAsync(int id,
                                                            CancellationToken cancellationToken = default)
            => _httpClient.DeleteAsync($"/post/comment/{id}", cancellationToken);

        public async Task<bool> SwapPostLikeAsync(int postId,
                                                       CancellationToken cancellationToken = default)
        {
            var res = await _httpClient.PutAsync($"post/{postId}/like", null, cancellationToken);
            res.EnsureSuccessStatusCode();
            return bool.Parse(await res.Content.ReadAsStringAsync(cancellationToken));
        }

        public async Task<bool> SwapCommentLikeAsync(int postId,
                                                              int commentId,
                                                              CancellationToken cancellationToken = default)
        {
            var res = await _httpClient.PutAsync($"post/{postId}/comment/{commentId}/like", null, cancellationToken);
            res.EnsureSuccessStatusCode();
            return bool.Parse(await res.Content.ReadAsStringAsync(cancellationToken));
        }

        public Task<IEnumerable<PostDisplayModel>> GetCurrentUserPostsAsync(int pageSize,
                                                                             DateTime? since = null,
                                                                             CancellationToken cancellationToken = default)
            => _httpClient.GetFromJsonAsync<IEnumerable<PostDisplayModel>>($"posts/me?pageSize={pageSize}&since={since}",
                                                                           cancellationToken);

        public Task<IEnumerable<PostDisplayModel>> GetProfilePostsAsync(int userId,
                                                                        int pageSize,
                                                                        DateTime? since = null,
                                                                        CancellationToken cancellationToken = default)
            => _httpClient.GetFromJsonAsync<IEnumerable<PostDisplayModel>>($"posts/profile/{userId}?pageSize={pageSize}&since={since}", 
                                                                           cancellationToken);

        public Task<IEnumerable<PostDisplayModel>> GetHomePostsAsync(int pageSize,
                                                                     DateTime? since = null,
                                                                     CancellationToken cancellationToken = default)
            => _httpClient.GetFromJsonAsync<IEnumerable<PostDisplayModel>>($"posts/home?pageSize={pageSize}&since={since}", cancellationToken);

        #endregion


    }
}
