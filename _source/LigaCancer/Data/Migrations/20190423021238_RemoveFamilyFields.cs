using Microsoft.EntityFrameworkCore.Migrations;

namespace LigaCancer.Migrations
{
    public partial class RemoveFamilyFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FamilyIncome",
                table: "Families");

            migrationBuilder.DropColumn(
                name: "PerCapitaIncome",
                table: "Families");

            migrationBuilder.AlterColumn<double>(
                name: "MonthlyIncome",
                table: "Families",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "MonthlyIncome",
                table: "Families",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AddColumn<double>(
                name: "FamilyIncome",
                table: "Families",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PerCapitaIncome",
                table: "Families",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
