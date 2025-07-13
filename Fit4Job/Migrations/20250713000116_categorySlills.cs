using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fit4Job.Migrations
{
    /// <inheritdoc />
    public partial class categorySlills : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Skills",
                table: "track_categories",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Skills",
                table: "track_categories");
        }
    }
}
