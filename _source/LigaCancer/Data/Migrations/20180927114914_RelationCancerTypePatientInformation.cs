using Microsoft.EntityFrameworkCore.Migrations;

namespace LigaCancer.Migrations
{
    public partial class RelationCancerTypePatientInformation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PatientInformationCancerType",
                columns: table => new
                {
                    PatientInformationId = table.Column<int>(nullable: false),
                    CancerTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientInformationCancerType", x => new { x.PatientInformationId, x.CancerTypeId });
                    table.ForeignKey(
                        name: "FK_PatientInformationCancerType_CancerTypes_CancerTypeId",
                        column: x => x.CancerTypeId,
                        principalTable: "CancerTypes",
                        principalColumn: "CancerTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientInformationCancerType_PatientInformation_PatientInfor~",
                        column: x => x.PatientInformationId,
                        principalTable: "PatientInformation",
                        principalColumn: "PatientInformationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatientInformationCancerType_CancerTypeId",
                table: "PatientInformationCancerType",
                column: "CancerTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatientInformationCancerType");
        }
    }
}
