using Microsoft.EntityFrameworkCore;
using MotoDispatch.Domain.Entity;
using MotoDispatch.Infra.Data.EF.Configurations;

namespace MotoDispatch.Infra.Data.EF;

public class MotoDispatchDbContext(
    DbContextOptions<MotoDispatchDbContext> options
) : DbContext(options)
{
    public DbSet<Motorcycle> Motorcycles => Set<Motorcycle>();
    public DbSet<DeliveryDriver> DeliveryDrivers => Set<DeliveryDriver>();
    public DbSet<Rental> Rentals => Set<Rental>();
    public DbSet<RentalPlan> RentalPlans => Set<RentalPlan>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new MotorcycleConfiguration());
        builder.ApplyConfiguration(new DeliveryDriverConfiguration());
        builder.ApplyConfiguration(new RentalConfiguration());
        builder.ApplyConfiguration(new RentalPlanConfiguration());
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        return base.SaveChangesAsync(cancellationToken);
    }
}