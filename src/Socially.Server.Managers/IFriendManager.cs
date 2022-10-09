using Socially.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Socially.Server.Managers
{
    public interface IFriendManager
    {
        Task<IEnumerable<UserSummaryModel>> GetFriendsAsync(int userId, CancellationToken cancellationToken = default);
        Task<IEnumerable<UserSummaryModel>> GetRequestsAsync(int userId, CancellationToken cancellationToken = default);
        Task RequestAsync(int requesterId, int approverId, CancellationToken cancellationToken = default);
        Task<bool> RespondAsync(int requesterId, int forId, bool isAccepted, CancellationToken cancellationToken = default);
    }
}