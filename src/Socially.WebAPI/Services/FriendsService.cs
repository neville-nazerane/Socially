using Socially.Models;
using Socially.Server.Managers;
using Socially.Server.Services.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Socially.WebAPI.Services
{
    public class FriendsService : IFriendsService
    {
        private readonly IFriendManager _friendManager;
        private readonly CurrentContext _currentContext;

        public FriendsService(IFriendManager friendManager, CurrentContext currentContext)
        {
            _friendManager = friendManager;
            _currentContext = currentContext;
        }

        public Task RequestAsync(int forId, CancellationToken cancellationToken = default)
            => _friendManager.RequestAsync(_currentContext.UserId, forId, cancellationToken);

        public Task<IEnumerable<UserSummaryModel>> GetRequestsAsync(CancellationToken cancellationToken = default)
            => _friendManager.GetRequestsAsync(_currentContext.UserId, cancellationToken);

        public Task<bool> RespondAsync(int requesterId, bool isAccepted, CancellationToken cancellationToken = default)
            => _friendManager.RespondAsync(requesterId, _currentContext.UserId, isAccepted, cancellationToken);

        //public Task<IEnumerable<UserSummaryModel>> SearchNonFriendsAsync(string query, CancellationToken cancellationToken = default)
        //    => _friendManager.SearchNonFriendsAsync(_currentContext.UserId, query, cancellationToken);

        public Task<IEnumerable<UserSummaryModel>> GetFriendsAsync(CancellationToken cancellationToken = default)
            => _friendManager.GetFriendsAsync(_currentContext.UserId, cancellationToken);

    }
}
