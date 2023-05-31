using System.Threading;
using System.Threading.Tasks;

namespace Socially.WebAPI.Services
{
    public interface ISignalRPublisher
    {
        void EnqueuePost(int id);
        Task KeepRunningAsync(CancellationToken cancellationToken = default);
    }
}