using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Common.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class RefreshTokenUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_ApplicationUsers_UserId",
                table: "RefreshTokens");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "RefreshTokens",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                newName: "IX_RefreshTokens_ApplicationUserId");

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
                name: "FK_RefreshTokens_ApplicationUsers_ApplicationUserId",
                table: "RefreshTokens",
                column: "ApplicationUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_ApplicationUsers_ApplicationUserId1",
                table: "RefreshTokens",
                column: "ApplicationUserId1",
                principalTable: "ApplicationUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_ApplicationUsers_ApplicationUserId",
                table: "RefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_ApplicationUsers_ApplicationUserId1",
                table: "RefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_RefreshTokens_ApplicationUserId1",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId1",
                table: "RefreshTokens");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "RefreshTokens",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_RefreshTokens_ApplicationUserId",
                table: "RefreshTokens",
                newName: "IX_RefreshTokens_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_ApplicationUsers_UserId",
                table: "RefreshTokens",
                column: "UserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
