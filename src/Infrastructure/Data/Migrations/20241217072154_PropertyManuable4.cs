using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASD.Onboard.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class PropertyManuable4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EducationLevel",
                table: "ApplicantEducations",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EducationLevel",
                table: "ApplicantEducations");
        }
    }
}
