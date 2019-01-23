using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LigaCancer.Migrations
{
    public partial class ChangesProfession : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Professions_ProfessionId",
                table: "Patients");

            migrationBuilder.DropTable(
                name: "Professions");

            migrationBuilder.DropIndex(
                name: "IX_Patients_ProfessionId",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "ProfessionId",
                table: "Patients");

            migrationBuilder.AddColumn<string>(
                name: "Profession",
                table: "Patients",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Profession",
                table: "Patients");

            migrationBuilder.AddColumn<int>(
                name: "ProfessionId",
                table: "Patients",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Professions",
                columns: table => new
                {
                    ProfessionId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DeletedDate = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(nullable: false),
                    LastUserUpdateId = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    RegisterDate = table.Column<DateTime>(nullable: false),
                    UserCreatedId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Professions", x => x.ProfessionId);
                    table.ForeignKey(
                        name: "FK_Professions_AspNetUsers_LastUserUpdateId",
                        column: x => x.LastUserUpdateId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Professions_AspNetUsers_UserCreatedId",
                        column: x => x.UserCreatedId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Patients_ProfessionId",
                table: "Patients",
                column: "ProfessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Professions_LastUserUpdateId",
                table: "Professions",
                column: "LastUserUpdateId");

            migrationBuilder.CreateIndex(
                name: "IX_Professions_Name",
                table: "Professions",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Professions_UserCreatedId",
                table: "Professions",
                column: "UserCreatedId");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Professions_ProfessionId",
                table: "Patients",
                column: "ProfessionId",
                principalTable: "Professions",
                principalColumn: "ProfessionId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
