using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SwarmPortal.SQLite.Migrations
{
    public partial class AddEnabled : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Enabled",
                table: "Roles",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Enabled",
                table: "Links",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Enabled",
                table: "Groups",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Enabled",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Enabled",
                table: "Links");

            migrationBuilder.DropColumn(
                name: "Enabled",
                table: "Groups");
        }
    }
}
