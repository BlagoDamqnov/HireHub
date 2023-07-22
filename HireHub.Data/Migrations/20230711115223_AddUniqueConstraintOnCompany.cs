using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HireHub.Data.Migrations
{
    public partial class AddUniqueConstraintOnCompany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Companies_ContactEmail",
                table: "Companies",
                column: "ContactEmail",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Companies_ContactPhone",
                table: "Companies",
                column: "ContactPhone",
                unique: true,
                filter: "[ContactPhone] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_Name",
                table: "Companies",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Companies_ContactEmail",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Companies_ContactPhone",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Companies_Name",
                table: "Companies");
        }
    }
}