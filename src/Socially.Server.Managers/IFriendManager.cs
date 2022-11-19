using Socially.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Socially.Server.Managers
{
    public interface IFriendManager
    {
        Task<IEnumerable<UserSummaryModel>> GetFriendsAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<UserSummaryModel>> GetRequestsAsync(CancellationToken cancellationToken = default);
        Task<int> RemoveFriendAsync(int friendId, CancellationToken cancellationToken = default);
        Task RequestAsync(int approverId, CancellationToken cancellationToken = default);
        Task<bool> RespondAsync(int forId, bool isAccepted, CancellationToken cancellationToken = default);
    }
}