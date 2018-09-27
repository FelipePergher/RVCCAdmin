using Microsoft.EntityFrameworkCore.Migrations;

namespace LigaCancer.Migrations
{
    public partial class missingData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CancerTypes_PatientInformation_PatientInformationId",
                table: "CancerTypes");

            migrationBuilder.DropIndex(
                name: "IX_CancerTypes_PatientInformationId",
                table: "CancerTypes");

            migrationBuilder.DropColumn(
                name: "PatientInformationId",
                table: "CancerTypes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PatientInformationId",
                table: "CancerTypes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CancerTypes_PatientInformationId",
                table: "CancerTypes",
                column: "PatientInformationId");

            migrationBuilder.AddForeignKey(
                name: "FK_CancerTypes_PatientInformation_PatientInformationId",
                table: "CancerTypes",
                column: "PatientInformationId",
                principalTable: "PatientInformation",
                principalColumn: "PatientInformationId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
