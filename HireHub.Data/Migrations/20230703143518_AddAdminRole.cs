using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HireHub.Data.Migrations
{
    public partial class AddAdminRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ef4c3c3e-8d45-4f36-92a8-994ce2d811fe", "ef4c3c3e-8d45-4f36-92a8-994ce2d811fe", "Admin", "ADMIN" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ef4c3c3e-8d45-4f36-92a8-994ce2d811fe");
        }
    }
}
