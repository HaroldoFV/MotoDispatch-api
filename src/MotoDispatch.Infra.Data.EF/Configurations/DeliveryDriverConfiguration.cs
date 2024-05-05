using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MotoDispatch.Domain.Entity;

namespace MotoDispatch.Infra.Data.EF.Configurations;

class DeliveryDriverConfiguration : IEntityTypeConfiguration<DeliveryDriver>
{
    public void Configure(EntityTypeBuilder<DeliveryDriver> builder)
    {
        builder.HasKey(deliveryDriver => deliveryDriver.Id);

        builder.Property(deliveryDriver => deliveryDriver.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(deliveryDriver => deliveryDriver.CNPJ)
            .HasMaxLength(14)
            .IsRequired();

        builder.Property(deliveryDriver => deliveryDriver.DateOfBirth)
            .IsRequired();

        builder.Property(deliveryDriver => deliveryDriver.CNHNumber)
            .HasMaxLength(11)
            .IsRequired();

        builder.Property(deliveryDriver => deliveryDriver.CNHType)
            .IsRequired();

        builder.Property(deliveryDriver => deliveryDriver.CNHImagePath)
            .IsRequired(false);

        builder.Property(deliveryDriver => deliveryDriver.CreatedAt)
            .IsRequired();

        builder.HasIndex(deliveryDriver => deliveryDriver.CNPJ).IsUnique();
        builder.HasIndex(deliveryDriver => deliveryDriver.CNHNumber).IsUnique();
    }
}