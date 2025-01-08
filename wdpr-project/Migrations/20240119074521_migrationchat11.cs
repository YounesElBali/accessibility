using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wdpr_project.Migrations
{
    public partial class migrationchat11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResearchCriteria_Addresses_AddressId",
                table: "ResearchCriteria");

            migrationBuilder.DropIndex(
                name: "IX_ResearchCriteria_AddressId",
                table: "ResearchCriteria");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "ResearchCriteria");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "ResearchCriteria",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "ResearchCriteria");

            migrationBuilder.AddColumn<int>(
                name: "AddressId",
                table: "ResearchCriteria",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ResearchCriteria_AddressId",
                table: "ResearchCriteria",
                column: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_ResearchCriteria_Addresses_AddressId",
                table: "ResearchCriteria",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
