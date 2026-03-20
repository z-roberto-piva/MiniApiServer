using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniApiServer.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ExtendStatsForAllOperations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "total_of_divisions",
                schema: "mini_api_server",
                table: "stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "total_of_multiplications",
                schema: "mini_api_server",
                table: "stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "total_of_divisions",
                schema: "mini_api_server",
                table: "stats");

            migrationBuilder.DropColumn(
                name: "total_of_multiplications",
                schema: "mini_api_server",
                table: "stats");
        }
    }
}
