using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HireHub.Data.Migrations
{
    public partial class SeedUserAdminData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "4aa6831b-552e-473b-b40e-f71d5b8a5b44", 0, "59a2e2b7-c2e7-4e32-aa50-29c06b717ae2", "admin@abv.bg", true, false, null, null, "ADMIN@ABV.BG", "AQAAAAEAACcQAAAAEOpxfsFY/n27EsHUW+5XGTbldIWULQcLrDAV8dXxNLqoscSJ4w7E5qqnoBV84mISUA==", null, true, "d72a7eee-7646-496a-98a0-abd8afe4f00c", false, "admin@abv.bg" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4aa6831b-552e-473b-b40e-f71d5b8a5b44");
        }
    }
}