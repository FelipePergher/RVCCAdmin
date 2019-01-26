using Microsoft.EntityFrameworkCore.Migrations;

namespace LigaCancer.Migrations
{
    public partial class AddAge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "Patients",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Age",
                table: "FamilyMembers",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age",
                table: "Patients");

            migrationBuilder.AlterColumn<int>(
                name: "Age",
                table: "FamilyMembers",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
