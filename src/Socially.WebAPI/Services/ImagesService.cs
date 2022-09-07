using Microsoft.AspNetCore.Http;
using Socially.Server.Managers;
using Socially.Server.Services.Models;
using Socially.Website.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Socially.WebAPI.Services
{
    public class ImagesService : IImagesService
    {
        private readonly IImageManager _imageManager;
        private readonly ConfigsSettings configsSettings;
        private readonly CurrentContext _currentContext;

        public ImagesService(IImageManager imageManager,
                            ConfigsSettings configsSettings,
                            CurrentContext currentContext)
        {
            _imageManager = imageManager;
            this.configsSettings = configsSettings;
            _currentContext = currentContext;
        }

        public Task UploadAsync(IFormFile formFile, CancellationToken cancellationToken = default)
            => _imageManager.AddAsync(_currentContext.UserId,
                                      formFile.FileName.Split(".").Last(),
                                      formFile.OpenReadStream(),
                                      cancellationToken);

        public Task<IEnumerable<string>> GetAllForUserAsync(CancellationToken cancellationToken = default)
            => _imageManager.GetAllForUserAsync(_currentContext.UserId, cancellationToken);

    }
}
