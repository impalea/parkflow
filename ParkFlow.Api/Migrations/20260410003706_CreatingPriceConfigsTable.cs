using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkFlow.Api.Migrations
{
    /// <inheritdoc />
    public partial class CreatingPriceConfigsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PriceConfigs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ToleranceMinutes = table.Column<int>(type: "int", nullable: false),
                    FirstHourValue = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    AdditionalHourValue = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DailyValue = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceConfigs", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "PriceConfigs",
                columns: new[] { "Id", "AdditionalHourValue", "DailyValue", "FirstHourValue", "IsActive", "LastUpdatedAt", "ToleranceMinutes" },
                values: new object[] { 1, 5.00m, 50.00m, 10.00m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 15 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PriceConfigs");
        }
    }
}
