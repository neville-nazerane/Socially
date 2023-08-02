using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Socially.Server.DataAccess.RealTimeMigrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PostConnections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    ConnectionId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostConnections", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostConnections_ConnectionId",
                table: "PostConnections",
                column: "ConnectionId");

            migrationBuilder.CreateIndex(
                name: "IX_PostConnections_PostId",
                table: "PostConnections",
                column: "PostId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostConnections");
        }
    }
}
