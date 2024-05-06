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

        services.AddHealthChecks()
            .AddNpgSql(
                configuration.GetConnectionString("MotoDispatchDb")
                ?? throw new Exception("Postgres configuration section not found")
            );

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
                    ?? throw new Exception("Postgres configuration section not found")
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