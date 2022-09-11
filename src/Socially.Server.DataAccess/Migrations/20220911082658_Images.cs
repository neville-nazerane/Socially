using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Socially.Server.DataAccess.Migrations
{
    public partial class Images : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProfileImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfileImages_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserRefreshTokens_RefreshToken",
                table: "UserRefreshTokens",
                column: "RefreshToken");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileImages_FileName_UserId",
                table: "ProfileImages",
                columns: new[] { "FileName", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProfileImages_UserId",
                table: "ProfileImages",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProfileImages");

            migrationBuilder.DropIndex(
                name: "IX_UserRefreshTokens_RefreshToken",
                table: "UserRefreshTokens");
        }
    }
}
