using Microsoft.EntityFrameworkCore.Migrations;

namespace LigaCancer.Migrations
{
    public partial class ChangePresence : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Presences_Patients_PatientId",
                table: "Presences");

            migrationBuilder.DropIndex(
                name: "IX_Presences_PatientId",
                table: "Presences");

            migrationBuilder.AlterColumn<int>(
                name: "PatientId",
                table: "Presences",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Presences",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Presences");

            migrationBuilder.AlterColumn<int>(
                name: "PatientId",
                table: "Presences",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_Presences_PatientId",
                table: "Presences",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Presences_Patients_PatientId",
                table: "Presences",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "PatientId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
