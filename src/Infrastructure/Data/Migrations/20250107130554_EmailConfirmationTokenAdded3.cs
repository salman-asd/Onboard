using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASD.Onboard.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class EmailConfirmationTokenAdded3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EmailConfirmationTokens_UserId",
                table: "EmailConfirmationTokens");

            migrationBuilder.DropColumn(
                name: "Token",
                table: "EmailConfirmationTokens");

            migrationBuilder.CreateIndex(
                name: "IX_EmailConfirmationTokens_UserId_IsUsed_ExpiryTime",
                table: "EmailConfirmationTokens",
                columns: new[] { "UserId", "IsUsed", "ExpiryTime" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EmailConfirmationTokens_UserId_IsUsed_ExpiryTime",
                table: "EmailConfirmationTokens");

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "EmailConfirmationTokens",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_EmailConfirmationTokens_UserId",
                table: "EmailConfirmationTokens",
                column: "UserId");
        }
    }
}
