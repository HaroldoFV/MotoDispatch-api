namespace MotoDispatch.Infra.Storage.Configuration
{
    public class StorageServiceOptions
    {
        public const string ConfigurationSection = "Storage";
        public string BucketName { get; set; } = "";
        public bool UseLocalStorage { get; set; } = true;
    }
}