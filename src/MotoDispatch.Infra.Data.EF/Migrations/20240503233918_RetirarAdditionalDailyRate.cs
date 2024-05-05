using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotoDispatch.Infra.Data.EF.Migrations
{
    /// <inheritdoc />
    public partial class RetirarAdditionalDailyRate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalDailyRate",
                table: "RentalPlans");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AdditionalDailyRate",
                table: "RentalPlans",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
