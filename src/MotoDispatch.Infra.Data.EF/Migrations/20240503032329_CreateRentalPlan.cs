using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotoDispatch.Infra.Data.EF.Migrations
{
    /// <inheritdoc />
    public partial class CreateRentalPlan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Plan_AdditionalDailyRate",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "Plan_DailyRate",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "Plan_Days",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "Plan_PenaltyRate",
                table: "Rentals");

            migrationBuilder.AddColumn<Guid>(
                name: "RentalPlanId",
                table: "Rentals",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "RentalPlans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Days = table.Column<int>(type: "integer", nullable: false),
                    DailyRate = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    PenaltyRate = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    AdditionalDailyRate = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    FixedAdditionalRate = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentalPlans", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_RentalPlanId",
                table: "Rentals",
                column: "RentalPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_RentalPlans_RentalPlanId",
                table: "Rentals",
                column: "RentalPlanId",
                principalTable: "RentalPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_RentalPlans_RentalPlanId",
                table: "Rentals");

            migrationBuilder.DropTable(
                name: "RentalPlans");

            migrationBuilder.DropIndex(
                name: "IX_Rentals_RentalPlanId",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "RentalPlanId",
                table: "Rentals");

            migrationBuilder.AddColumn<decimal>(
                name: "Plan_AdditionalDailyRate",
                table: "Rentals",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Plan_DailyRate",
                table: "Rentals",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Plan_Days",
                table: "Rentals",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Plan_PenaltyRate",
                table: "Rentals",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
