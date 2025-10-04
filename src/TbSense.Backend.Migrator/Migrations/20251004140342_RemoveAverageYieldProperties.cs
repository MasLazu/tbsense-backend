using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TbSense.Backend.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAverageYieldProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "monthly_average_yield_kg",
                table: "plantation_harvests");

            migrationBuilder.DropColumn(
                name: "yearly_average_yield_kg",
                table: "plantation_harvests");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "monthly_average_yield_kg",
                table: "plantation_harvests",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "yearly_average_yield_kg",
                table: "plantation_harvests",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
