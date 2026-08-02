using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Steam2Api.Migrations
{
    /// <inheritdoc />
    public partial class AddUserGamesRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameEntityUserEntity",
                columns: table => new
                {
                    GamesId = table.Column<string>(type: "TEXT", nullable: false),
                    UsersId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameEntityUserEntity", x => new { x.GamesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_GameEntityUserEntity_juegos_GamesId",
                        column: x => x.GamesId,
                        principalTable: "juegos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameEntityUserEntity_usuarios_UsersId",
                        column: x => x.UsersId,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameEntityUserEntity_UsersId",
                table: "GameEntityUserEntity",
                column: "UsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameEntityUserEntity");
        }
    }
}
