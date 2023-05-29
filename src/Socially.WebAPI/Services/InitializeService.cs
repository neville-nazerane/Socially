using Socially.Server.Managers;
using Socially.WebAPI.Services;
using System.Threading;
using System.Threading.Tasks;

namespace Socially.Website.Services
{
    public class InitializeService
    {

        private readonly IImageManager _imageManager;
        private readonly ISignalRStateManager _signalRStateManager;

        public InitializeService(IImageManager imageManager,
                                 ISignalRStateManager signalRStateManager)
        {
            _imageManager = imageManager;
            _signalRStateManager = signalRStateManager;
        }


        public async Task InitAsync(CancellationToken cancellationToken = default)
        {
            await _imageManager.InitAsync(cancellationToken);
            await _signalRStateManager.InitAsync(cancellationToken);
        }

    }
}
