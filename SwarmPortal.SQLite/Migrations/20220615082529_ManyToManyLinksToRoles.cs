using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SwarmPortal.SQLite.Migrations
{
    public partial class ManyToManyLinksToRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Links_LinkId",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Roles_LinkId",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "LinkId",
                table: "Roles");

            migrationBuilder.CreateTable(
                name: "LinkRole",
                columns: table => new
                {
                    LinksId = table.Column<ulong>(type: "INTEGER", nullable: false),
                    RolesId = table.Column<ulong>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinkRole", x => new { x.LinksId, x.RolesId });
                    table.ForeignKey(
                        name: "FK_LinkRole_Links_LinksId",
                        column: x => x.LinksId,
                        principalTable: "Links",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LinkRole_Roles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LinkRole_RolesId",
                table: "LinkRole",
                column: "RolesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LinkRole");

            migrationBuilder.AddColumn<ulong>(
                name: "LinkId",
                table: "Roles",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_LinkId",
                table: "Roles",
                column: "LinkId");

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Links_LinkId",
                table: "Roles",
                column: "LinkId",
                principalTable: "Links",
                principalColumn: "Id");
        }
    }
}
