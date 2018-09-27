using Microsoft.EntityFrameworkCore.Migrations;

namespace LigaCancer.Migrations
{
    public partial class RelationsManyToMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medicines_PatientInformation_PatientInformationId",
                table: "Medicines");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Naturalities_NaturalityId",
                table: "Patients");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Professions_ProfessionId",
                table: "Patients");

            migrationBuilder.DropForeignKey(
                name: "FK_Residences_ResidenceTypes_ResidenceTypeId",
                table: "Residences");

            migrationBuilder.DropForeignKey(
                name: "FK_TreatmentPlaces_PatientInformation_PatientInformationId",
                table: "TreatmentPlaces");

            migrationBuilder.DropIndex(
                name: "IX_TreatmentPlaces_PatientInformationId",
                table: "TreatmentPlaces");

            migrationBuilder.DropIndex(
                name: "IX_Residences_ResidenceTypeId",
                table: "Residences");

            migrationBuilder.DropIndex(
                name: "IX_Patients_NaturalityId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_ProfessionId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Medicines_PatientInformationId",
                table: "Medicines");

            migrationBuilder.DropColumn(
                name: "PatientInformationId",
                table: "TreatmentPlaces");

            migrationBuilder.DropColumn(
                name: "ResidenceTypeId",
                table: "Residences");

            migrationBuilder.DropColumn(
                name: "NaturalityId",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "ProfessionId",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "PatientInformationId",
                table: "Medicines");

            migrationBuilder.CreateTable(
                name: "PatientInformationMedicine",
                columns: table => new
                {
                    PatientInformationId = table.Column<int>(nullable: false),
                    MedicineId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientInformationMedicine", x => new { x.PatientInformationId, x.MedicineId });
                    table.ForeignKey(
                        name: "FK_PatientInformationMedicine_Medicines_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Medicines",
                        principalColumn: "MedicineId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientInformationMedicine_PatientInformation_PatientInforma~",
                        column: x => x.PatientInformationId,
                        principalTable: "PatientInformation",
                        principalColumn: "PatientInformationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientInformationTreatmentPlace",
                columns: table => new
                {
                    PatientInformationId = table.Column<int>(nullable: false),
                    TreatmentPlaceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientInformationTreatmentPlace", x => new { x.PatientInformationId, x.TreatmentPlaceId });
                    table.ForeignKey(
                        name: "FK_PatientInformationTreatmentPlace_PatientInformation_PatientI~",
                        column: x => x.PatientInformationId,
                        principalTable: "PatientInformation",
                        principalColumn: "PatientInformationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientInformationTreatmentPlace_TreatmentPlaces_TreatmentPl~",
                        column: x => x.TreatmentPlaceId,
                        principalTable: "TreatmentPlaces",
                        principalColumn: "TreatmentPlaceId",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "ResidenceTypeResidence",
                columns: table => new
                {
                    ResidenceTypeId = table.Column<int>(nullable: false),
                    ResidenceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResidenceTypeResidence", x => new { x.ResidenceId, x.ResidenceTypeId });
                    table.ForeignKey(
                        name: "FK_ResidenceTypeResidence_Residences_ResidenceId",
                        column: x => x.ResidenceId,
                        principalTable: "Residences",
                        principalColumn: "ResidenceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResidenceTypeResidence_ResidenceTypes_ResidenceTypeId",
                        column: x => x.ResidenceTypeId,
                        principalTable: "ResidenceTypes",
                        principalColumn: "ResidenceTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatientInformationMedicine_MedicineId",
                table: "PatientInformationMedicine",
                column: "MedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientInformationTreatmentPlace_TreatmentPlaceId",
                table: "PatientInformationTreatmentPlace",
                column: "TreatmentPlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientNaturality_NaturalityId",
                table: "PatientNaturality",
                column: "NaturalityId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientProfession_ProfessionId",
                table: "PatientProfession",
                column: "ProfessionId");

            migrationBuilder.CreateIndex(
                name: "IX_ResidenceTypeResidence_ResidenceTypeId",
                table: "ResidenceTypeResidence",
                column: "ResidenceTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatientInformationMedicine");

            migrationBuilder.DropTable(
                name: "PatientInformationTreatmentPlace");

            migrationBuilder.DropTable(
                name: "PatientNaturality");

            migrationBuilder.DropTable(
                name: "PatientProfession");

            migrationBuilder.DropTable(
                name: "ResidenceTypeResidence");

            migrationBuilder.AddColumn<int>(
                name: "PatientInformationId",
                table: "TreatmentPlaces",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ResidenceTypeId",
                table: "Residences",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NaturalityId",
                table: "Patients",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProfessionId",
                table: "Patients",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PatientInformationId",
                table: "Medicines",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TreatmentPlaces_PatientInformationId",
                table: "TreatmentPlaces",
                column: "PatientInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_Residences_ResidenceTypeId",
                table: "Residences",
                column: "ResidenceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_NaturalityId",
                table: "Patients",
                column: "NaturalityId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_ProfessionId",
                table: "Patients",
                column: "ProfessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Medicines_PatientInformationId",
                table: "Medicines",
                column: "PatientInformationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Medicines_PatientInformation_PatientInformationId",
                table: "Medicines",
                column: "PatientInformationId",
                principalTable: "PatientInformation",
                principalColumn: "PatientInformationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Naturalities_NaturalityId",
                table: "Patients",
                column: "NaturalityId",
                principalTable: "Naturalities",
                principalColumn: "NaturalityId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Professions_ProfessionId",
                table: "Patients",
                column: "ProfessionId",
                principalTable: "Professions",
                principalColumn: "ProfessionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Residences_ResidenceTypes_ResidenceTypeId",
                table: "Residences",
                column: "ResidenceTypeId",
                principalTable: "ResidenceTypes",
                principalColumn: "ResidenceTypeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TreatmentPlaces_PatientInformation_PatientInformationId",
                table: "TreatmentPlaces",
                column: "PatientInformationId",
                principalTable: "PatientInformation",
                principalColumn: "PatientInformationId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
