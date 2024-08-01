using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeBaseService.Migrations
{
    /// <inheritdoc />
    public partial class updatedefaultmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "CodeBases",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SupportedPlatform",
                table: "CodeBases",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Valid",
                table: "CodeBases",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "CodeBases");

            migrationBuilder.DropColumn(
                name: "SupportedPlatform",
                table: "CodeBases");

            migrationBuilder.DropColumn(
                name: "Valid",
                table: "CodeBases");
        }
    }
}
