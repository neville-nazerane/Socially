
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.StaticFiles;
using System.ComponentModel;
using System.IO;
using System.Reflection;

const int argsCount = 3;

if (args.Length != argsCount)
{
    throw new IndexOutOfRangeException($"Wrong number of arguments passed in. Found {args.Length}");
}

string connString = args[0];
string containerName = args[1];
string srcPath = args[2];

var typeProvider = new FileExtensionContentTypeProvider();

var blobClient = new BlobServiceClient(connString);
var containerClient = blobClient.GetBlobContainerClient(containerName);

var existingBlobs = containerClient.GetBlobsByHierarchyAsync();

await foreach (var blob in existingBlobs)
    await containerClient.DeleteBlobAsync(blob.Blob.Name);

var files = Directory.GetFiles(srcPath, "*.*", SearchOption.AllDirectories);

await UploadAsync(string.Empty);


async Task UploadAsync(string path)
{
    var fullPath = Path.Combine(srcPath, path);
    foreach (var file in Directory.GetFiles(fullPath))
    {
        var fileInfo = new FileInfo(file);
        string fullName = Path.Combine(path, fileInfo.Name);
        await using var stream = fileInfo.OpenRead();
        Console.WriteLine("Uploading " + file);
        var blobClient = containerClient.GetBlobClient(fullName);

        var blobHttpHeader = new BlobHttpHeaders { ContentType = typeProvider.Mappings[fileInfo.Extension] };
        await blobClient.UploadAsync(stream, new BlobUploadOptions { HttpHeaders = blobHttpHeader });
        
        //var info = await containerClient.UploadBlobAsync(fullName, stream);
    }
    
    foreach (var dir in Directory.GetDirectories(fullPath))
    {
        var info = new DirectoryInfo(dir);
        var newPath = Path.Combine(path, info.Name);
        await UploadAsync(newPath);
    }
}