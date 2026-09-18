using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KaSHop.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Brandss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "categories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "brands",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "categories");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "brands");
        }
    }
}
