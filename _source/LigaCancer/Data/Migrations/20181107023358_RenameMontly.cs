using Microsoft.EntityFrameworkCore.Migrations;

namespace LigaCancer.Migrations
{
    public partial class RenameMontly : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MontlyIncome",
                table: "Families",
                newName: "MonthlyIncome");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MonthlyIncome",
                table: "Families",
                newName: "MontlyIncome");
        }
    }
}
