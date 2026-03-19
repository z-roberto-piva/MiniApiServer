using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniApiServer.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMultiplicationAndDivision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "data_divisions",
                schema: "mini_api_server",
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
                    table.PrimaryKey("PK_data_divisions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "data_multiplications",
                schema: "mini_api_server",
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
                    table.PrimaryKey("PK_data_multiplications", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_data_divisions_data_in_id",
                schema: "mini_api_server",
                table: "data_divisions",
                column: "data_in_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_data_multiplications_data_in_id",
                schema: "mini_api_server",
                table: "data_multiplications",
                column: "data_in_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "data_divisions",
                schema: "mini_api_server");

            migrationBuilder.DropTable(
                name: "data_multiplications",
                schema: "mini_api_server");
        }
    }
}
