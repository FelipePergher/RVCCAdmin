using Microsoft.EntityFrameworkCore.Migrations;

namespace LigaCancer.Migrations
{
    public partial class MontlyPatient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "MontlyIncome",
                table: "Families",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MontlyIncome",
                table: "Families");
        }
    }
}
