using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TbSense.Backend.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class AddLongitudeLatitudeToTree : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "latitude",
                table: "trees",
                type: "double precision",
                precision: 10,
                scale: 7,
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "longitude",
                table: "trees",
                type: "double precision",
                precision: 10,
                scale: 7,
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "latitude",
                table: "trees");

            migrationBuilder.DropColumn(
                name: "longitude",
                table: "trees");
        }
    }
}
