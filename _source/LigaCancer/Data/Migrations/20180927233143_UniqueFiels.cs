using Microsoft.EntityFrameworkCore.Migrations;

namespace LigaCancer.Migrations
{
    public partial class UniqueFiels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatientNaturality");

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "TreatmentPlaces",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "ResidenceTypes",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Professions",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RG",
                table: "Patients",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CPF",
                table: "Patients",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NaturalityId",
                table: "Patients",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Medicines",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Doctors",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "CancerTypes",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TreatmentPlaces_City",
                table: "TreatmentPlaces",
                column: "City",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ResidenceTypes_Type",
                table: "ResidenceTypes",
                column: "Type",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Professions_Name",
                table: "Professions",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Patients_CPF",
                table: "Patients",
                column: "CPF",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Patients_NaturalityId",
                table: "Patients",
                column: "NaturalityId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_RG",
                table: "Patients",
                column: "RG",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Medicines_Name",
                table: "Medicines",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_Name",
                table: "Doctors",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CancerTypes_Name",
                table: "CancerTypes",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Naturalities_NaturalityId",
                table: "Patients",
                column: "NaturalityId",
                principalTable: "Naturalities",
                principalColumn: "NaturalityId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Naturalities_NaturalityId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_TreatmentPlaces_City",
                table: "TreatmentPlaces");

            migrationBuilder.DropIndex(
                name: "IX_ResidenceTypes_Type",
                table: "ResidenceTypes");

            migrationBuilder.DropIndex(
                name: "IX_Professions_Name",
                table: "Professions");

            migrationBuilder.DropIndex(
                name: "IX_Patients_CPF",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_NaturalityId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_RG",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Medicines_Name",
                table: "Medicines");

            migrationBuilder.DropIndex(
                name: "IX_Doctors_Name",
                table: "Doctors");

            migrationBuilder.DropIndex(
                name: "IX_CancerTypes_Name",
                table: "CancerTypes");

            migrationBuilder.DropColumn(
                name: "NaturalityId",
                table: "Patients");

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "TreatmentPlaces",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "ResidenceTypes",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Professions",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RG",
                table: "Patients",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CPF",
                table: "Patients",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Medicines",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Doctors",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "CancerTypes",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "PatientNaturality",
                columns: table => new
                {
                    PatientId = table.Column<int>(nullable: false),
                    NaturalityId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientNaturality", x => new { x.PatientId, x.NaturalityId });
                    table.ForeignKey(
                        name: "FK_PatientNaturality_Naturalities_NaturalityId",
                        column: x => x.NaturalityId,
                        principalTable: "Naturalities",
                        principalColumn: "NaturalityId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientNaturality_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatientNaturality_NaturalityId",
                table: "PatientNaturality",
                column: "NaturalityId");
        }
    }
}
