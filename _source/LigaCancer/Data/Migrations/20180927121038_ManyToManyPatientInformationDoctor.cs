using Microsoft.EntityFrameworkCore.Migrations;

namespace LigaCancer.Migrations
{
    public partial class ManyToManyPatientInformationDoctor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_PatientInformation_PatientInformationId",
                table: "Doctors");

            migrationBuilder.DropIndex(
                name: "IX_Doctors_PatientInformationId",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "PatientInformationId",
                table: "Doctors");

            migrationBuilder.CreateTable(
                name: "PatientInformationDoctor",
                columns: table => new
                {
                    PatientInformationId = table.Column<int>(nullable: false),
                    DoctorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientInformationDoctor", x => new { x.PatientInformationId, x.DoctorId });
                    table.ForeignKey(
                        name: "FK_PatientInformationDoctor_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "DoctorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientInformationDoctor_PatientInformation_PatientInformati~",
                        column: x => x.PatientInformationId,
                        principalTable: "PatientInformation",
                        principalColumn: "PatientInformationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatientInformationDoctor_DoctorId",
                table: "PatientInformationDoctor",
                column: "DoctorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatientInformationDoctor");

            migrationBuilder.AddColumn<int>(
                name: "PatientInformationId",
                table: "Doctors",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_PatientInformationId",
                table: "Doctors",
                column: "PatientInformationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_PatientInformation_PatientInformationId",
                table: "Doctors",
                column: "PatientInformationId",
                principalTable: "PatientInformation",
                principalColumn: "PatientInformationId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
