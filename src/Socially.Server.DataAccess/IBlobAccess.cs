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

        Task UploadAsync(string containerName,
                         string fileName,
                         Stream stream,
                         CancellationToken cancellationToken = default);

    }
}