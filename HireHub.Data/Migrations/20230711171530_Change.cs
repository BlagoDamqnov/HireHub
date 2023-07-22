using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HireHub.Data.Migrations
{
    public partial class Change : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4aa6831b-552e-473b-b40e-f71d5b8a5b44",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "bcc5e635-4ccf-415b-940b-903d45339455", "AQAAAAEAACcQAAAAEGxjYgTmvrLF5k1M2dtONGchFz6vBjxTvzQ7LZmObQBK9vnBnAwYU9k2FFVKsRU0Rg==", "61290ee0-fbf0-49e3-8ff0-bd98f9d4dfb4" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4aa6831b-552e-473b-b40e-f71d5b8a5b44",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "31071bf1-be9b-4502-a082-5cdcb8a7a9c3", "AQAAAAEAACcQAAAAEMev1YTuNq8OPNj5exIN+sbAc3nd0bMtT+K9/as0ZXu5CGfirue3yMl2sH282KiClg==", "d27f356f-4c6b-4bfa-b3df-2ccef71d1974" });
        }
    }
}