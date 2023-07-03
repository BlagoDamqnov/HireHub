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
                values: new object[] { "ef4c3c3e-8d45-4f36-92a8-994ce2d811fe", "ff7240f7-02d0-4d9f-aab9-43b759a328df" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "ef4c3c3e-8d45-4f36-92a8-994ce2d811fe", "ff7240f7-02d0-4d9f-aab9-43b759a328df" });
        }
    }
}
