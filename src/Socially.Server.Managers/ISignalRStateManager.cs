using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Socially.Server.Managers
{
    public interface ISignalRStateManager
    {
        IAsyncEnumerable<string> GetConnectionIdsAsync(string listenerTag, int pageSize);
        Task InitAsync(CancellationToken cancellationToken = default);
        Task RegisterAsync(IEnumerable<string> listenerTags, string connectionId, CancellationToken cancellationToken = default);
        Task UnregisterAsync(string connectionId);
    }
}