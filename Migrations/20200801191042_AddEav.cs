using Microsoft.EntityFrameworkCore.Migrations;

namespace EVA_Model.Migrations
{
    public partial class AddEav : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeeEav",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeEav", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeAttributes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmployeeId = table.Column<int>(nullable: false),
                    AttributeName = table.Column<string>(nullable: true),
                    AttributeValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeAttributes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeAttributes_EmployeeEav_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "EmployeeEav",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAttributes_EmployeeId",
                table: "EmployeeAttributes",
                column: "EmployeeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeAttributes");

            migrationBuilder.DropTable(
                name: "EmployeeEav");
        }
    }
}
