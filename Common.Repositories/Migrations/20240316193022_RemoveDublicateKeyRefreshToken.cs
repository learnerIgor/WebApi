using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Common.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDublicateKeyRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_ApplicationUsers_ApplicationUserId1",
                table: "RefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_RefreshTokens_ApplicationUserId1",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId1",
                table: "RefreshTokens");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApplicationUserId1",
                table: "RefreshTokens",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_ApplicationUserId1",
                table: "RefreshTokens",
                column: "ApplicationUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_ApplicationUsers_ApplicationUserId1",
                table: "RefreshTokens",
                column: "ApplicationUserId1",
                principalTable: "ApplicationUsers",
                principalColumn: "Id");
        }
    }
}
