namespace MotoDispatch.Application.Interfaces;

public interface IStorageService
{
    Task<string?> Upload(
        string fileName,
        Stream fileStream,
        string contentType,
        CancellationToken cancellationToken);
}