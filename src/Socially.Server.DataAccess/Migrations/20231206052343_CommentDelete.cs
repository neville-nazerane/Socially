using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Socially.Server.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class CommentDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Comments",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Comments");
        }
    }
}
