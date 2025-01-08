using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wdpr_project.Migrations
{
    public partial class migrationchat8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResearchExpert_Experts_ExpertId",
                table: "ResearchExpert");

            migrationBuilder.DropForeignKey(
                name: "FK_ResearchExpert_Researches_ResearchId",
                table: "ResearchExpert");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ResearchExpert",
                table: "ResearchExpert");

            migrationBuilder.RenameTable(
                name: "ResearchExpert",
                newName: "ResearchExperts");

            migrationBuilder.RenameIndex(
                name: "IX_ResearchExpert_ResearchId",
                table: "ResearchExperts",
                newName: "IX_ResearchExperts_ResearchId");

            migrationBuilder.RenameIndex(
                name: "IX_ResearchExpert_ExpertId",
                table: "ResearchExperts",
                newName: "IX_ResearchExperts_ExpertId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ResearchExperts",
                table: "ResearchExperts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ResearchExperts_Experts_ExpertId",
                table: "ResearchExperts",
                column: "ExpertId",
                principalTable: "Experts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ResearchExperts_Researches_ResearchId",
                table: "ResearchExperts",
                column: "ResearchId",
                principalTable: "Researches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResearchExperts_Experts_ExpertId",
                table: "ResearchExperts");

            migrationBuilder.DropForeignKey(
                name: "FK_ResearchExperts_Researches_ResearchId",
                table: "ResearchExperts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ResearchExperts",
                table: "ResearchExperts");

            migrationBuilder.RenameTable(
                name: "ResearchExperts",
                newName: "ResearchExpert");

            migrationBuilder.RenameIndex(
                name: "IX_ResearchExperts_ResearchId",
                table: "ResearchExpert",
                newName: "IX_ResearchExpert_ResearchId");

            migrationBuilder.RenameIndex(
                name: "IX_ResearchExperts_ExpertId",
                table: "ResearchExpert",
                newName: "IX_ResearchExpert_ExpertId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ResearchExpert",
                table: "ResearchExpert",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ResearchExpert_Experts_ExpertId",
                table: "ResearchExpert",
                column: "ExpertId",
                principalTable: "Experts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ResearchExpert_Researches_ResearchId",
                table: "ResearchExpert",
                column: "ResearchId",
                principalTable: "Researches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
