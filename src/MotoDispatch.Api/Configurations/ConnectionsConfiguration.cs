using Microsoft.EntityFrameworkCore;
using MotoDispatch.Infra.Data.EF;

namespace MotoDispatch.Api.Configurations;

public static class ConnectionsConfiguration
{
    public static IServiceCollection AddAppConections(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddDbConnection(configuration);
        return services;
    }

    private static IServiceCollection AddDbConnection(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var connectionString = configuration
            .GetConnectionString("MotoDispatchDb");

        services.AddDbContext<MotoDispatchDbContext>(
            options => options.UseNpgsql(
                    connectionString
                )
                .LogTo(Console.WriteLine, LogLevel.Information)
        );
        /*services.AddDbContext<MotoDispatchDbContext>(
            options => options.UseInMemoryDatabase(
                "InMemory-DSV-Database"
            )
        );*/
        return services;
    }
}