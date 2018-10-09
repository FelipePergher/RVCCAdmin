using Microsoft.EntityFrameworkCore.Migrations;

namespace LigaCancer.Migrations
{
    public partial class ChangedProfessionRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatientProfession");

            migrationBuilder.AddColumn<int>(
                name: "ProfessionsProfessionId",
                table: "Patients",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Patients_ProfessionsProfessionId",
                table: "Patients",
                column: "ProfessionsProfessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Professions_ProfessionsProfessionId",
                table: "Patients",
                column: "ProfessionsProfessionId",
                principalTable: "Professions",
                principalColumn: "ProfessionId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Professions_ProfessionsProfessionId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_ProfessionsProfessionId",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "ProfessionsProfessionId",
                table: "Patients");

            migrationBuilder.CreateTable(
                name: "PatientProfession",
                columns: table => new
                {
                    PatientId = table.Column<int>(nullable: false),
                    ProfessionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientProfession", x => new { x.PatientId, x.ProfessionId });
                    table.ForeignKey(
                        name: "FK_PatientProfession_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientProfession_Professions_ProfessionId",
                        column: x => x.ProfessionId,
                        principalTable: "Professions",
                        principalColumn: "ProfessionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatientProfession_ProfessionId",
                table: "PatientProfession",
                column: "ProfessionId");
        }
    }
}
