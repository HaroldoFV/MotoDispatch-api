using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MotoDispatch.Domain.Entity;

namespace MotoDispatch.Infra.Data.EF.Configurations;

public class RentalPlanConfiguration : IEntityTypeConfiguration<RentalPlan>
{
    public void Configure(EntityTypeBuilder<RentalPlan> builder)
    {
        builder.ToTable("RentalPlans");

        builder.HasKey(rp => rp.Id);

        builder.Property(rp => rp.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(rp => rp.Days)
            .IsRequired();

        builder.Property(rp => rp.DailyRate)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(rp => rp.PenaltyRate)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(rp => rp.FixedAdditionalRate)
            .IsRequired()
            .HasColumnType("decimal(18,2)");
    }
}