
using Azure.Storage.Blobs;

const int argsCount = 3;

if (args.Length != argsCount)
{
    throw new IndexOutOfRangeException($"Wrong number of arguments passed in. Found {args.Length}");
}

string connString = args[0];
string containerName = args[1];
string srcPath = args[2];


var blobClient = new BlobServiceClient(connString);
var containerClient = blobClient.GetBlobContainerClient(containerName);

var files = Directory.GetFiles(srcPath, "*.*", SearchOption.AllDirectories);

foreach (var file in files)
{
    Console.WriteLine($"Uploading {file}");
    var fileInfo = new FileInfo(file);
    string fullName = fileInfo.FullName.Replace(srcPath, "");
    await using var stream = fileInfo.OpenRead();
    await containerClient.UploadBlobAsync(fullName, stream);
}

