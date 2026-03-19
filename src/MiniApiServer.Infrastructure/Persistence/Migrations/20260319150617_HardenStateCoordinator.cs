using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniApiServer.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class HardenStateCoordinator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_data_summs_data_in_id",
                table: "data_summs",
                column: "data_in_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_data_subtractions_data_in_id",
                table: "data_subtractions",
                column: "data_in_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_data_summs_data_in_id",
                table: "data_summs");

            migrationBuilder.DropIndex(
                name: "IX_data_subtractions_data_in_id",
                table: "data_subtractions");
        }
    }
}
