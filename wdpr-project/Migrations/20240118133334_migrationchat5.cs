using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wdpr_project.Migrations
{
    public partial class migrationchat5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ResearchCriteria_ResearchId",
                table: "ResearchCriteria");

            migrationBuilder.CreateIndex(
                name: "IX_ResearchCriteria_ResearchId",
                table: "ResearchCriteria",
                column: "ResearchId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ResearchCriteria_ResearchId",
                table: "ResearchCriteria");

            migrationBuilder.CreateIndex(
                name: "IX_ResearchCriteria_ResearchId",
                table: "ResearchCriteria",
                column: "ResearchId",
                unique: true);
        }
    }
}
