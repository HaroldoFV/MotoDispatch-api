using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Options;
using MotoDispatch.Application.Interfaces;
using MotoDispatch.Infra.Storage.Configuration;

namespace MotoDispatch.Infra.Storage.Services;

public class StorageService : IStorageService
{
    private readonly StorageClient _storageClient;
    private readonly StorageServiceOptions _options;

    public StorageService(
        StorageClient storageClient,
        IOptions<StorageServiceOptions> options)
    {
        _storageClient = storageClient;
        _options = options.Value;
    }

    public async Task<string?> Upload(
        string fileName,
        Stream fileStream,
        string contentType,
        CancellationToken cancellationToken)
    {
        var objectName = $"{fileName}";

        await _storageClient.UploadObjectAsync(
            _options.BucketName, fileName, contentType, fileStream,
            cancellationToken: cancellationToken);

        return GetPublicUrl(_options.BucketName, objectName);
    }


    private string? GetPublicUrl(string bucket, string objectName)
    {
        return $"https://storage.googleapis.com/{bucket}/{objectName}";
    }
}