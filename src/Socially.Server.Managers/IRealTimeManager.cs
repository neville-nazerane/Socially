using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Socially.Server.Managers
{
    public interface IRealTimeManager
    {
        IAsyncEnumerable<string> GetPostConnectionIdsAsync(int postId);
        IAsyncEnumerable<string> GetUserConnectionIdsAsync(int userId);
        Task SubscribeForPostsAsync(string connectionId, IEnumerable<int> postIds, CancellationToken cancellationToken = default);
        Task SubscribeForUsersAsync(string connectionId, IEnumerable<int> userIds, CancellationToken cancellationToken = default);
        Task UnsubscribeForConnectionAsync(string connectionId, CancellationToken cancellationToken = default);
        Task UnsubscribeForUserConnectionAsync(string connectionId, CancellationToken cancellationToken = default);
    }
}