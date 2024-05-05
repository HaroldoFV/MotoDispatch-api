using MotoDispatch.Application.Interfaces;
using MotoDispatch.Application.UseCases.Motorcycle.CreateMotorcycle;
using MotoDispatch.Domain.Repository;
using MotoDispatch.Infra.Data.EF;
using MotoDispatch.Infra.Data.EF.Repositories;

namespace MotoDispatch.Api.Configurations;

public static class UseCasesConfiguration
{
    public static IServiceCollection AddUseCases(
        this IServiceCollection services
    )
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(CreateMotorcycle).Assembly)
        );
        services.AddRepositories();
        return services;
    }

    private static IServiceCollection AddRepositories(
        this IServiceCollection services
    )
    {
        services.AddTransient<IMotorcycleRepository, MotorcycleRepository>();
        services.AddTransient<IDeliveryDriverRepository, DeliveryDriverRepository>();
        services.AddTransient<IRentalRepository, RentalRepository>();
        services.AddTransient<IRentalPlanRepository, RentalPlanRepository>();
        services.AddTransient<IUnitOfWork, UnitOfWork>();
        return services;
    }
}