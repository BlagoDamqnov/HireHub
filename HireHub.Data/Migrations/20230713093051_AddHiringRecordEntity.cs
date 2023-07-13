using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HireHub.Data.Migrations
{
    public partial class AddHiringRecordEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HiringRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JobId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CandidateId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DateOfHiring = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsHired = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HiringRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HiringRecords_AspNetUsers_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HiringRecords_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4aa6831b-552e-473b-b40e-f71d5b8a5b44",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "090ce677-2213-417b-b19e-e8ee158c49cf", "AQAAAAEAACcQAAAAELZHgcidskihmpP+sed0nyF+tA90EF/JuCljIddJTTyW8zP5MQ59VpFKt4Dlf3Y4Sw==", "236fcd4b-ca4c-45e3-a120-2dfb723113a7" });

            migrationBuilder.CreateIndex(
                name: "IX_HiringRecords_CandidateId",
                table: "HiringRecords",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_HiringRecords_JobId",
                table: "HiringRecords",
                column: "JobId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HiringRecords");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4aa6831b-552e-473b-b40e-f71d5b8a5b44",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fa2c449e-9ff3-4dcb-877d-14900c6b944c", "AQAAAAEAACcQAAAAEKKE8QE64+Df0pjxfZtV39JPGcqz6Vd95OeyhsG32m24E2ytUEU1tci9K/EZNERZAA==", "ec103232-a0ca-422a-aefb-e7d7b784aff4" });
        }
    }
}
