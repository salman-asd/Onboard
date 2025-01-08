using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASD.Onboard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AppliacantMOdified2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Applicants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PreferredName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DOB = table.Column<DateOnly>(type: "date", nullable: false),
                    BloodGroupId = table.Column<int>(type: "int", nullable: false),
                    ReligionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaritalStatusId = table.Column<int>(type: "int", nullable: false),
                    GenderId = table.Column<int>(type: "int", nullable: false),
                    PrimaryMobileNo = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    SecondaryMobileNo = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    PrimaryEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SecondaryEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Nationality = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IdentificationType = table.Column<int>(type: "int", nullable: true),
                    IdentificationNo = table.Column<long>(type: "bigint", nullable: true),
                    PermAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PermDistrictId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PermZipCode = table.Column<int>(type: "int", nullable: true),
                    PresAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PresDistrictId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PresZipCode = table.Column<int>(type: "int", nullable: true),
                    ContactAddress = table.Column<int>(type: "int", nullable: true, defaultValue: 1),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applicants", x => x.Id);
                });

        }
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Applicants");
        }
    }
}
