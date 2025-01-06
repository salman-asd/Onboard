﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASD.Onboard.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddOutboxEmailProcess2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HtmlBody",
                table: "EmailOutboxes");

            migrationBuilder.AlterColumn<string>(
                name: "Body",
                table: "EmailOutboxes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Body",
                table: "EmailOutboxes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HtmlBody",
                table: "EmailOutboxes",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
