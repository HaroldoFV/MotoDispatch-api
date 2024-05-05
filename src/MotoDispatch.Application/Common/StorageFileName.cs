namespace MotoDispatch.Application.Common;

public static class StorageFileName
{
    public static string CreateNameForCloud(Guid id, string propertyName, string extension)
        => $"{id}/{propertyName.ToLower()}.{extension.Replace(".", "")}";


    public static string Create(Guid id, string originalFileName, string extension)
    {
        extension = extension.TrimStart('.');

        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(originalFileName);

        return $"{id}/{fileNameWithoutExtension}.{extension}";
    }
}