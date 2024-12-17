using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASD.Onboard.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class PropertyManuable3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EducationLevel",
                table: "ApplicantEducations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EducationLevel",
                table: "ApplicantEducations",
                type: "int",
                nullable: true);
        }
    }
}
