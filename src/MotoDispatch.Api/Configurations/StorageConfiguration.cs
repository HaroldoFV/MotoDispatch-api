using Google.Cloud.Storage.V1;
using MotoDispatch.Application.Interfaces;
using MotoDispatch.Infra.Storage.Configuration;
using MotoDispatch.Infra.Storage.Services;

namespace MotoDispatch.Api.Configurations;

public static class StorageConfiguration
{
    public static IServiceCollection AddStorage(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped(_ => StorageClient.Create());

        services.Configure<StorageServiceOptions>(
            configuration.GetSection(StorageServiceOptions.ConfigurationSection));


        var storageOptions = new StorageServiceOptions();
        configuration.GetSection(StorageServiceOptions.ConfigurationSection).Bind(storageOptions);

        if (storageOptions.UseLocalStorage)
        {
            services.AddSingleton<IStorageService>(_ =>
                new LocalStorageService(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
        }
        else
        {
            services.AddTransient<IStorageService, StorageService>();
        }


        return services;
    }
}