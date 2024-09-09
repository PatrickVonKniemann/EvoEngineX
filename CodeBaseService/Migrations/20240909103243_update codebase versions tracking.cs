using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeBaseService.Migrations
{
    /// <inheritdoc />
    public partial class updatecodebaseversionstracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SuccessfulRunCounter",
                table: "CodeBases",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SuccessfulRunCounter",
                table: "CodeBases");
        }
    }
}
