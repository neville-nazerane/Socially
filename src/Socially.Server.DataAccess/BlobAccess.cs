using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

        public Task DeleteAsync(string containerName,
                                string fileName,
                                CancellationToken cancellationToken = default)
            => WithContainer(containerName).DeleteBlobIfExistsAsync(fileName, cancellationToken: cancellationToken);

    }
}
