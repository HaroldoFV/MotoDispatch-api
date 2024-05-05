using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MotoDispatch.Domain.Entity;

namespace MotoDispatch.Infra.Data.EF.Configurations;

class MotorcycleConfiguration : IEntityTypeConfiguration<Motorcycle>
{
    public void Configure(EntityTypeBuilder<Motorcycle> builder)
    {
        builder.HasKey(motorcycle => motorcycle.Id);

        builder.Property(motorcycle => motorcycle.Year)
            .IsRequired();

        builder.Property(motorcycle => motorcycle.LicensePlate)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(motorcycle => motorcycle.Model)
            .HasMaxLength(255)
            .IsRequired();
    }
}