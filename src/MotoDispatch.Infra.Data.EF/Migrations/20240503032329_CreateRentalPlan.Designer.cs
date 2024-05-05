﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MotoDispatch.Infra.Data.EF;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MotoDispatch.Infra.Data.EF.Migrations
{
    [DbContext(typeof(MotoDispatchDbContext))]
    [Migration("20240503032329_CreateRentalPlan")]
    partial class CreateRentalPlan
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MotoDispatch.Domain.Entity.DeliveryDriver", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CNHImagePath")
                        .HasColumnType("text");

                    b.Property<string>("CNHNumber")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("character varying(11)");

                    b.Property<int>("CNHType")
                        .HasColumnType("integer");

                    b.Property<string>("CNPJ")
                        .IsRequired()
                        .HasMaxLength(14)
                        .HasColumnType("character varying(14)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.HasIndex("CNHNumber")
                        .IsUnique();

                    b.HasIndex("CNPJ")
                        .IsUnique();

                    b.ToTable("DeliveryDrivers");
                });

            modelBuilder.Entity("MotoDispatch.Domain.Entity.Motorcycle", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LicensePlate")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<int>("Year")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Motorcycles");
                });

            modelBuilder.Entity("MotoDispatch.Domain.Entity.Rental", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("ActualEndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("DeliveryDriverId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("MotorcycleId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("RentalPlanId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("DeliveryDriverId");

                    b.HasIndex("MotorcycleId");

                    b.HasIndex("RentalPlanId");

                    b.ToTable("Rentals", (string)null);
                });

            modelBuilder.Entity("MotoDispatch.Domain.Entity.RentalPlan", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("AdditionalDailyRate")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("DailyRate")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Days")
                        .HasColumnType("integer");

                    b.Property<decimal>("FixedAdditionalRate")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("PenaltyRate")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("RentalPlans", (string)null);
                });

            modelBuilder.Entity("MotoDispatch.Domain.Entity.Rental", b =>
                {
                    b.HasOne("MotoDispatch.Domain.Entity.DeliveryDriver", null)
                        .WithMany()
                        .HasForeignKey("DeliveryDriverId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("MotoDispatch.Domain.Entity.Motorcycle", null)
                        .WithMany()
                        .HasForeignKey("MotorcycleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("MotoDispatch.Domain.Entity.RentalPlan", "RentalPlan")
                        .WithMany()
                        .HasForeignKey("RentalPlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RentalPlan");
                });
#pragma warning restore 612, 618
        }
    }
}
