using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Socially.Server.DataAccess
{
    public interface IRealTimeManager
    {
        IAsyncEnumerable<string> GetPostConnectionIdsAsync(int postId);
        Task SubscribeForPostsAsync(string connectionId, IEnumerable<int> postIds, CancellationToken cancellationToken = default);
        Task UnsubscribeForConnectionAsync(string connectionId, CancellationToken cancellationToken = default);
    }
}