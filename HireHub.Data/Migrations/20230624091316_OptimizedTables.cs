using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HireHub.Data.Migrations
{
    public partial class OptimizedTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationResumes");

            migrationBuilder.AddColumn<string>(
                name: "Requirements",
                table: "Jobs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ResumeId",
                table: "Applications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Applications_ResumeId",
                table: "Applications",
                column: "ResumeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Resumes_ResumeId",
                table: "Applications",
                column: "ResumeId",
                principalTable: "Resumes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Resumes_ResumeId",
                table: "Applications");

            migrationBuilder.DropIndex(
                name: "IX_Applications_ResumeId",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "Requirements",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "ResumeId",
                table: "Applications");

            migrationBuilder.CreateTable(
                name: "ApplicationResumes",
                columns: table => new
                {
                    ApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ResumeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationResumes", x => new { x.ApplicationId, x.ResumeId });
                    table.ForeignKey(
                        name: "FK_ApplicationResumes_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ApplicationResumes_Resumes_ResumeId",
                        column: x => x.ResumeId,
                        principalTable: "Resumes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationResumes_ResumeId",
                table: "ApplicationResumes",
                column: "ResumeId");
        }
    }
}