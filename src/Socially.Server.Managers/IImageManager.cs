using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Socially.Server.Managers
{
    public interface IImageManager
    {
        Task<string> AddAsync(int userId, string fileExtension, string contentType, Stream stream, CancellationToken cancellationToken = default);
        Task DeleteByNameAsync(int userId, string fileName, CancellationToken cancellationToken = default);
        Task<IEnumerable<string>> GetAllForUserAsync(int userId, CancellationToken cancellationToken = default);
        Task InitAsync(CancellationToken cancellationToken = default);
    }
}