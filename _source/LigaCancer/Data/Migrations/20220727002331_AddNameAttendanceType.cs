using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RVCC.Data.Migrations
{
    public partial class AddNameAttendanceType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AttendanceTypes",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "AttendanceTypes");
        }
    }
}
