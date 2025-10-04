using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TbSense.Backend.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "models",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    training_status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    file_path = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    is_used = table.Column<bool>(type: "boolean", nullable: false),
                    training_data_start = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    training_data_end = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_models", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "plantations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    land_area_hectares = table.Column<float>(type: "real", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_plantations", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "plantation_coordinates",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    plantation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    longitude = table.Column<double>(type: "double precision", nullable: false),
                    latitude = table.Column<double>(type: "double precision", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_plantation_coordinates", x => x.id);
                    table.ForeignKey(
                        name: "fk_plantation_coordinates_plantations_plantation_id",
                        column: x => x.plantation_id,
                        principalTable: "plantations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "plantation_harvests",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    plantation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    yield_kg = table.Column<float>(type: "real", nullable: false),
                    harvest_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    yearly_average_yield_kg = table.Column<float>(type: "real", nullable: false),
                    monthly_average_yield_kg = table.Column<float>(type: "real", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_plantation_harvests", x => x.id);
                    table.ForeignKey(
                        name: "fk_plantation_harvests_plantations_plantation_id",
                        column: x => x.plantation_id,
                        principalTable: "plantations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "plantation_yield_predictions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    plantation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    model_id = table.Column<Guid>(type: "uuid", nullable: false),
                    timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_plantation_yield_predictions", x => x.id);
                    table.ForeignKey(
                        name: "fk_plantation_yield_predictions_models_model_id",
                        column: x => x.model_id,
                        principalTable: "models",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_plantation_yield_predictions_plantations_plantation_id",
                        column: x => x.plantation_id,
                        principalTable: "plantations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "trees",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    plantation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_trees", x => x.id);
                    table.ForeignKey(
                        name: "fk_trees_plantations_plantation_id",
                        column: x => x.plantation_id,
                        principalTable: "plantations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tree_metrics",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    tree_id = table.Column<Guid>(type: "uuid", nullable: false),
                    soil_moisture = table.Column<float>(type: "real", nullable: false),
                    soil_temperature = table.Column<float>(type: "real", nullable: false),
                    air_temperature = table.Column<float>(type: "real", nullable: false),
                    timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tree_metrics", x => x.id);
                    table.ForeignKey(
                        name: "fk_tree_metrics_trees_tree_id",
                        column: x => x.tree_id,
                        principalTable: "trees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_plantation_coordinates_plantation_id",
                table: "plantation_coordinates",
                column: "plantation_id");

            migrationBuilder.CreateIndex(
                name: "ix_plantation_harvests_plantation_id",
                table: "plantation_harvests",
                column: "plantation_id");

            migrationBuilder.CreateIndex(
                name: "ix_plantation_yield_predictions_model_id",
                table: "plantation_yield_predictions",
                column: "model_id");

            migrationBuilder.CreateIndex(
                name: "ix_plantation_yield_predictions_plantation_id",
                table: "plantation_yield_predictions",
                column: "plantation_id");

            migrationBuilder.CreateIndex(
                name: "ix_plantations_name",
                table: "plantations",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_tree_metrics_tree_id",
                table: "tree_metrics",
                column: "tree_id");

            migrationBuilder.CreateIndex(
                name: "ix_trees_plantation_id",
                table: "trees",
                column: "plantation_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "plantation_coordinates");

            migrationBuilder.DropTable(
                name: "plantation_harvests");

            migrationBuilder.DropTable(
                name: "plantation_yield_predictions");

            migrationBuilder.DropTable(
                name: "tree_metrics");

            migrationBuilder.DropTable(
                name: "models");

            migrationBuilder.DropTable(
                name: "trees");

            migrationBuilder.DropTable(
                name: "plantations");
        }
    }
}
