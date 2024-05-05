using MotoDispatch.Application.Interfaces;

namespace MotoDispatch.Infra.Storage.Services;

public class LocalStorageService(string baseDirectory) : IStorageService
{
    public async Task<string?> Upload(string fileName, Stream fileStream, string contentType,
        CancellationToken cancellationToken)
    {
        string storageDirectory = Path.Combine(baseDirectory, "uploads");

        if (!Directory.Exists(storageDirectory))
        {
            Directory.CreateDirectory(storageDirectory);
        }

        string? fileDirectory = Path.GetDirectoryName(fileName);
        string fullDirectoryPath = Path.Combine(storageDirectory, fileDirectory ?? string.Empty);

        if (!Directory.Exists(fullDirectoryPath))
        {
            Directory.CreateDirectory(fullDirectoryPath);
        }
        else
        {
            DeleteExistingFiles(fullDirectoryPath);
        }

        string? filePath = Path.Combine(fullDirectoryPath, Path.GetFileName(fileName));

        using (var outputStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        {
            await fileStream.CopyToAsync(outputStream, cancellationToken);
        }

        return filePath;
    }

    private void DeleteExistingFiles(string directoryPath)
    {
        var files = Directory.GetFiles(directoryPath);

        foreach (var file in files)
        {
            File.Delete(file);
        }
    }
}