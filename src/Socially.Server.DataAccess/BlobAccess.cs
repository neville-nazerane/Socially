using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Socially.Server.DataAccess
{

    public class BlobAccess : IBlobAccess
    {

        private readonly BlobServiceClient _client;

        public BlobAccess(string connString)
        {
            _client = new BlobServiceClient(connString);
        }

        BlobContainerClient WithContainer(string containerName) => _client.GetBlobContainerClient(containerName);

        public Task CreateContainerIfNotExistAsync(string containerName,
                                                   PublicAccessType publicAccessType = PublicAccessType.None,
                                                   CancellationToken cancellationToken = default)
            => WithContainer(containerName).CreateIfNotExistsAsync(publicAccessType, cancellationToken: cancellationToken);

        public Task UploadAsync(string containerName,
                                string fileName,
                                string contentType,
                                Stream stream,
                                CancellationToken cancellationToken = default)
        {
           return WithContainer(containerName).GetBlobClient(fileName)
                                                .UploadAsync(stream, new BlobUploadOptions
                                                {
                                                    HttpHeaders = new BlobHttpHeaders
                                                    {
                                                        ContentType = contentType
                                                    }
                                                }, cancellationToken);
        }

        public async Task UploadAsync(string containerName,
                                string fileName,
                                object data,
                                CancellationToken cancellationToken = default)
        {
            await using var stream = new MemoryStream();
            await JsonSerializer.SerializeAsync(stream, data, cancellationToken: cancellationToken);

            await WithContainer(containerName).GetBlobClient(fileName)
                                              .UploadAsync(stream, cancellationToken);
        }

        public async Task<TModel> DownloadAsync<TModel>(string containerName,
                                                        string fileName,
                                                        CancellationToken cancellationToken = default)
            where TModel : class
        {
            var result = await WithContainer(containerName).GetBlobClient(fileName)
                                                           .DownloadAsync(cancellationToken);
            return await JsonSerializer.DeserializeAsync<TModel>(result.Value.Content, 
                                                                 cancellationToken: cancellationToken);
        }

        public Task DeleteAsync(string containerName,
                                string fileName,
                                CancellationToken cancellationToken = default)
            => WithContainer(containerName).DeleteBlobIfExistsAsync(fileName, cancellationToken: cancellationToken);

    }
}
