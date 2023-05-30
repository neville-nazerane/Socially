
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.StaticFiles;
using MimeTypes.Core;
using System.ComponentModel;
using System.IO;
using System.Reflection;

const int argsCount = 3;
ParallelOptions parallelOptions = new ParallelOptions
{
    MaxDegreeOfParallelism = 10
};

if (args.Length != argsCount)
{
    throw new IndexOutOfRangeException($"Wrong number of arguments passed in. Found {args.Length}");
}

string connString = args[0];
string containerName = args[1];
string srcPath = args[2];

var blobClient = new BlobServiceClient(connString);
var containerClient = blobClient.GetBlobContainerClient(containerName);

var existingBlobs = containerClient.GetBlobsByHierarchyAsync();

await Parallel.ForEachAsync(existingBlobs,
                            parallelOptions,
                            async (blob, ct) => await containerClient.DeleteBlobAsync(blob.Blob.Name));

var files = Directory.GetFiles(srcPath, "*.*", SearchOption.AllDirectories);

await UploadAsync(string.Empty);


async Task UploadAsync(string path)
{
    var fullPath = Path.Combine(srcPath, path);
    await Parallel.ForEachAsync(Directory.GetFiles(fullPath), parallelOptions,
        async (file, ct) =>
        {
            var fileInfo = new FileInfo(file);
            string fullName = Path.Combine(path, fileInfo.Name);
            await using var stream = fileInfo.OpenRead();
            Console.WriteLine("Uploading " + file);
            var blobClient = containerClient.GetBlobClient(fullName);
            string mimeType = MimeTypeMap.GetMimeType(fileInfo.Extension, string.Empty);
            var blobHttpHeader = new BlobHttpHeaders { ContentType = mimeType };
            await blobClient.UploadAsync(stream, new BlobUploadOptions { HttpHeaders = blobHttpHeader }, ct);
        });

    await Parallel.ForEachAsync(Directory.GetDirectories(fullPath), parallelOptions,
       async (dir, ct) =>
       {
           var info = new DirectoryInfo(dir);
           var newPath = Path.Combine(path, info.Name);
           await UploadAsync(newPath);
       });

    //       foreach (var dir in Directory.GetDirectories(fullPath))
    //{
    //    var info = new DirectoryInfo(dir);
    //    var newPath = Path.Combine(path, info.Name);
    //    await UploadAsync(newPath);
    //}
}