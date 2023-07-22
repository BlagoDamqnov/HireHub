using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HireHub.Data.Migrations
{
    public partial class AddUserToRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "ef4c3c3e-8d45-4f36-92a8-994ce2d811fe", "4aa6831b-552e-473b-b40e-f71d5b8a5b44" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "ef4c3c3e-8d45-4f36-92a8-994ce2d811fe", "4aa6831b-552e-473b-b40e-f71d5b8a5b44" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "e332f872-2826-4cd4-a64b-65f82014f1af");
        }
    }
}