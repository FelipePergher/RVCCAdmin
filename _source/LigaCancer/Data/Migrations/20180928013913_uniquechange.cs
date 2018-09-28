using Microsoft.EntityFrameworkCore.Migrations;

namespace LigaCancer.Migrations
{
    public partial class uniquechange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Doctors_Name",
                table: "Doctors");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Doctors",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CRM",
                table: "Doctors",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_CRM",
                table: "Doctors",
                column: "CRM",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Doctors_CRM",
                table: "Doctors");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Doctors",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CRM",
                table: "Doctors",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_Name",
                table: "Doctors",
                column: "Name",
                unique: true);
        }
    }
}
