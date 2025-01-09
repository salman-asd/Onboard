using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASD.Onboard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class JobPostAdded32 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.RenameTable(
                name: "PositionPosts",
                schema: "onboarding",
                newName: "PositionPosts",
                newSchema: "dbo");

            migrationBuilder.RenameColumn(
                name: "StatusId",
                table: "JobApplications",
                newName: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "onboarding");

            migrationBuilder.RenameTable(
                name: "PositionPosts",
                schema: "dbo",
                newName: "PositionPosts",
                newSchema: "onboarding");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "JobApplications",
                newName: "StatusId");
        }
    }
}
