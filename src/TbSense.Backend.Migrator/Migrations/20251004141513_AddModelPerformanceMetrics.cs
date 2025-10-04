using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TbSense.Backend.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class AddModelPerformanceMetrics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "accuracy",
                table: "models",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "mae",
                table: "models",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "r2score",
                table: "models",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "rmse",
                table: "models",
                type: "double precision",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "accuracy",
                table: "models");

            migrationBuilder.DropColumn(
                name: "mae",
                table: "models");

            migrationBuilder.DropColumn(
                name: "r2score",
                table: "models");

            migrationBuilder.DropColumn(
                name: "rmse",
                table: "models");
        }
    }
}
