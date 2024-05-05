using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MotoDispatch.Domain.Entity;

namespace MotoDispatch.Infra.Data.EF.Configurations;

public class RentalConfiguration : IEntityTypeConfiguration<Rental>
{
    public void Configure(EntityTypeBuilder<Rental> builder)
    {
        builder.ToTable("Rentals");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.StartDate)
            .IsRequired();

        builder.Property(r => r.EndDate)
            .IsRequired();

        builder.Property(r => r.ActualEndDate)
            .IsRequired(false);

        builder.HasOne<Motorcycle>()
            .WithMany()
            .HasForeignKey(r => r.MotorcycleId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<DeliveryDriver>()
            .WithMany()
            .HasForeignKey(r => r.DeliveryDriverId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.RentalPlan)
            .WithMany()
            .HasForeignKey(r => r.RentalPlanId)
            .IsRequired();
    }
}