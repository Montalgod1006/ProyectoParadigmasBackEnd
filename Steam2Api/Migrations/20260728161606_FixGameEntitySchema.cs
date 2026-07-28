using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Steam2Api.Migrations
{
    /// <inheritdoc />
    public partial class FixGameEntitySchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GameEntityId",
                table: "juegos",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_juegos_GameEntityId",
                table: "juegos",
                column: "GameEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_juegos_juegos_GameEntityId",
                table: "juegos",
                column: "GameEntityId",
                principalTable: "juegos",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_juegos_juegos_GameEntityId",
                table: "juegos");

            migrationBuilder.DropIndex(
                name: "IX_juegos_GameEntityId",
                table: "juegos");

            migrationBuilder.DropColumn(
                name: "GameEntityId",
                table: "juegos");
        }
    }
}
