﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeRunService.Migrations
{
    /// <inheritdoc />
    public partial class updateCodeRun : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "CodeRuns",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "CodeRuns");
        }
    }
}
