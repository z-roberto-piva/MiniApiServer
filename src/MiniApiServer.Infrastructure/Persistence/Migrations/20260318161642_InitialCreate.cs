using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniApiServer.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "data_in",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    data_a = table.Column<int>(type: "integer", nullable: false),
                    data_b = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_data_in", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "data_subtractions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    data_in_id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    result = table.Column<int>(type: "integer", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_data_subtractions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "data_summs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    data_in_id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    result = table.Column<int>(type: "integer", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_data_summs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "stats",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    n_of_operations = table.Column<int>(type: "integer", nullable: false),
                    total_of_sums = table.Column<int>(type: "integer", nullable: false),
                    total_of_subtractions = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stats", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "data_in");

            migrationBuilder.DropTable(
                name: "data_subtractions");

            migrationBuilder.DropTable(
                name: "data_summs");

            migrationBuilder.DropTable(
                name: "stats");
        }
    }
}
