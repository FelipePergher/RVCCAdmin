using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LigaCancer.Migrations
{
    public partial class RemoveAgeField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "Age",
                table: "FamilyMembers");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "FamilyMembers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "FamilyMembers");

            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "Patients",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "FamilyMembers",
                nullable: true);
        }
    }
}
