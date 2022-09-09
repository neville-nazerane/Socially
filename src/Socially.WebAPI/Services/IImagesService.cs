using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Socially.WebAPI.Services
{
    public interface IImagesService
    {
        Task<IEnumerable<string>> GetAllForUserAsync(CancellationToken cancellationToken = default);
        Task UploadAsync(IFormFile formFile, CancellationToken cancellationToken = default);
    }
}