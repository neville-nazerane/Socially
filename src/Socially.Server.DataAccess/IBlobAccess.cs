using Azure.Storage.Blobs.Models;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Socially.Server.DataAccess
{
    public interface IBlobAccess
    {
        Task CreateContainerIfNotExistAsync(string containerName,
                                            PublicAccessType publicAccessType = PublicAccessType.None,
                                            CancellationToken cancellationToken = default);

        Task DeleteAsync(string containerName,
                         string fileName,
                         CancellationToken cancellationToken = default);
        Task<TModel> DownloadAsync<TModel>(string containerName, string fileName, CancellationToken cancellationToken = default) where TModel : class;
        Task UploadAsync(string containerName,
                         string fileName,
                         string contentType,
                         Stream stream,
                         CancellationToken cancellationToken = default);
        Task UploadAsync(string containerName, string fileName, object data, CancellationToken cancellationToken = default);
    }
}