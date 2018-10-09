using Microsoft.EntityFrameworkCore.Migrations;

namespace LigaCancer.Migrations
{
    public partial class ChangeColumnName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Professions_ProfessionsProfessionId",
                table: "Patients");

            migrationBuilder.RenameColumn(
                name: "ProfessionsProfessionId",
                table: "Patients",
                newName: "ProfessionId");

            migrationBuilder.RenameIndex(
                name: "IX_Patients_ProfessionsProfessionId",
                table: "Patients",
                newName: "IX_Patients_ProfessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Professions_ProfessionId",
                table: "Patients",
                column: "ProfessionId",
                principalTable: "Professions",
                principalColumn: "ProfessionId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Professions_ProfessionId",
                table: "Patients");

            migrationBuilder.RenameColumn(
                name: "ProfessionId",
                table: "Patients",
                newName: "ProfessionsProfessionId");

            migrationBuilder.RenameIndex(
                name: "IX_Patients_ProfessionId",
                table: "Patients",
                newName: "IX_Patients_ProfessionsProfessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Professions_ProfessionsProfessionId",
                table: "Patients",
                column: "ProfessionsProfessionId",
                principalTable: "Professions",
                principalColumn: "ProfessionId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
