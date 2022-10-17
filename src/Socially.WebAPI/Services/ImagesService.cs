using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Socially.Server.Managers;
using Socially.Server.Managers.Utils;
using Socially.Website.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Socially.WebAPI.Services
{
    public class ImagesService : IImagesService
    {

        private static readonly FileExtensionContentTypeProvider contentTypeProvider = new();

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

        public Task<string> UploadAsync(IFormFile formFile, CancellationToken cancellationToken = default)
        {
            var ext = $".{formFile.FileName.Split(".").Last()}";
            if (!contentTypeProvider.Mappings.ContainsKey(ext))
                throw new BadHttpRequestException("Invalid file format");

            return _imageManager.AddAsync(_currentContext.UserId,
                                          ext,
                                          contentTypeProvider.Mappings[ext],
                                          formFile.OpenReadStream(),
                                          cancellationToken);
        }

        public Task DeleteAsync(string fileName, CancellationToken cancellationToken = default)
            => _imageManager.DeleteByNameAsync(_currentContext.UserId, fileName, cancellationToken);

        public Task<IEnumerable<string>> GetAllForUserAsync(CancellationToken cancellationToken = default)
            => _imageManager.GetAllForUserAsync(_currentContext.UserId, cancellationToken);

    }
}
