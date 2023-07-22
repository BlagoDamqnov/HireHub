using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HireHub.Data.Migrations
{
    public partial class AddAdditionalFieldInCompanyEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogoUrl",
                table: "Jobs");

            migrationBuilder.AddColumn<string>(
                name: "LogoUrl",
                table: "Companies",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4aa6831b-552e-473b-b40e-f71d5b8a5b44",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "cea6d5c6-b15b-4796-8032-ea3841bd1e4d", "AQAAAAEAACcQAAAAEFYDnxmcNzK9uvqillb6p+hJ+yzYCJu8CLAfTzawtVfvSxjC80SPuATKnp7BhzJIOw==", "d87e67dc-e9ab-49ae-aa11-79b91069dc08" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogoUrl",
                table: "Companies");

            migrationBuilder.AddColumn<string>(
                name: "LogoUrl",
                table: "Jobs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4aa6831b-552e-473b-b40e-f71d5b8a5b44",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "090ce677-2213-417b-b19e-e8ee158c49cf", "AQAAAAEAACcQAAAAELZHgcidskihmpP+sed0nyF+tA90EF/JuCljIddJTTyW8zP5MQ59VpFKt4Dlf3Y4Sw==", "236fcd4b-ca4c-45e3-a120-2dfb723113a7" });
        }
    }
}