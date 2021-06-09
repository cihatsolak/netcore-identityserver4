using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityServer.AuthServer.Migrations
{
    public partial class InitialCreateDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomUsers", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "CustomUsers",
                columns: new[] { "Id", "City", "Email", "Password", "Username" },
                values: new object[] { 1, "İstanbul", "cihatsolak@test.com", "password", "cihat.solak" });

            migrationBuilder.InsertData(
                table: "CustomUsers",
                columns: new[] { "Id", "City", "Email", "Password", "Username" },
                values: new object[] { 2, "Ankara", "mesutsolak@test.com", "password", "mesut.solak" });

            migrationBuilder.InsertData(
                table: "CustomUsers",
                columns: new[] { "Id", "City", "Email", "Password", "Username" },
                values: new object[] { 3, "İzmir", "mehmetsolak@test.com", "password", "mehmet.solak" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomUsers");
        }
    }
}
