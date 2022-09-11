using Socially.Server.Managers;
using System.Threading;
using System.Threading.Tasks;

namespace Socially.Website.Services
{
    public class InitializeService
    {

        private readonly IImageManager _imageManager;

        public InitializeService(IImageManager imageManager)
        {
            _imageManager = imageManager;
        }

        public Task InitAsync(CancellationToken cancellationToken = default)
        {
            return _imageManager.InitAsync(cancellationToken);
        }

    }
}
