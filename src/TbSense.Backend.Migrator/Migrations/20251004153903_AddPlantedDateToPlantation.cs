using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TbSense.Backend.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class AddPlantedDateToPlantation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "planted_date",
                table: "plantations",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "planted_date",
                table: "plantations");
        }
    }
}
