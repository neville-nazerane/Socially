using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Socially.WebAPI.Services
{
    public interface IImagesService
    {
        Task UploadAsync(IFormFile formFile, CancellationToken cancellationToken = default);
    }
}