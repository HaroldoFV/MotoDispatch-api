using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotoDispatch.Infra.Data.EF.Migrations
{
    /// <inheritdoc />
    public partial class CreateDeliveryDriverTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeliveryDrivers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CNPJ = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CNHNumber = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false),
                    CNHType = table.Column<int>(type: "integer", nullable: false),
                    CNHImagePath = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryDrivers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryDrivers_CNHNumber",
                table: "DeliveryDrivers",
                column: "CNHNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryDrivers_CNPJ",
                table: "DeliveryDrivers",
                column: "CNPJ",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeliveryDrivers");
        }
    }
}
