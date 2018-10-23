using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LigaCancer.Migrations
{
    public partial class FamilyChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Families_Residences_ResidenceId",
                table: "Families");

            migrationBuilder.DropTable(
                name: "ResidenceTypeResidence");

            migrationBuilder.DropTable(
                name: "Residences");

            migrationBuilder.DropTable(
                name: "ResidenceTypes");

            migrationBuilder.DropIndex(
                name: "IX_Families_ResidenceId",
                table: "Families");

            migrationBuilder.DropColumn(
                name: "MonthlyIncome",
                table: "Families");

            migrationBuilder.DropColumn(
                name: "ResidenceId",
                table: "Families");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "MonthlyIncome",
                table: "Families",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "ResidenceId",
                table: "Families",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Residences",
                columns: table => new
                {
                    ResidenceId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DeletedDate = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(nullable: false),
                    LastUserUpdateId = table.Column<string>(nullable: true),
                    RegisterDate = table.Column<DateTime>(nullable: false),
                    ResidenceObservation = table.Column<string>(nullable: true),
                    UserCreatedId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Residences", x => x.ResidenceId);
                    table.ForeignKey(
                        name: "FK_Residences_AspNetUsers_LastUserUpdateId",
                        column: x => x.LastUserUpdateId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Residences_AspNetUsers_UserCreatedId",
                        column: x => x.UserCreatedId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ResidenceTypes",
                columns: table => new
                {
                    ResidenceTypeId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DeletedDate = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(nullable: false),
                    LastUserUpdateId = table.Column<string>(nullable: true),
                    RegisterDate = table.Column<DateTime>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    UserCreatedId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResidenceTypes", x => x.ResidenceTypeId);
                    table.ForeignKey(
                        name: "FK_ResidenceTypes_AspNetUsers_LastUserUpdateId",
                        column: x => x.LastUserUpdateId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResidenceTypes_AspNetUsers_UserCreatedId",
                        column: x => x.UserCreatedId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ResidenceTypeResidence",
                columns: table => new
                {
                    ResidenceId = table.Column<int>(nullable: false),
                    ResidenceTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResidenceTypeResidence", x => new { x.ResidenceId, x.ResidenceTypeId });
                    table.ForeignKey(
                        name: "FK_ResidenceTypeResidence_Residences_ResidenceId",
                        column: x => x.ResidenceId,
                        principalTable: "Residences",
                        principalColumn: "ResidenceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResidenceTypeResidence_ResidenceTypes_ResidenceTypeId",
                        column: x => x.ResidenceTypeId,
                        principalTable: "ResidenceTypes",
                        principalColumn: "ResidenceTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Families_ResidenceId",
                table: "Families",
                column: "ResidenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Residences_LastUserUpdateId",
                table: "Residences",
                column: "LastUserUpdateId");

            migrationBuilder.CreateIndex(
                name: "IX_Residences_UserCreatedId",
                table: "Residences",
                column: "UserCreatedId");

            migrationBuilder.CreateIndex(
                name: "IX_ResidenceTypeResidence_ResidenceTypeId",
                table: "ResidenceTypeResidence",
                column: "ResidenceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ResidenceTypes_LastUserUpdateId",
                table: "ResidenceTypes",
                column: "LastUserUpdateId");

            migrationBuilder.CreateIndex(
                name: "IX_ResidenceTypes_Type",
                table: "ResidenceTypes",
                column: "Type",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ResidenceTypes_UserCreatedId",
                table: "ResidenceTypes",
                column: "UserCreatedId");

            migrationBuilder.AddForeignKey(
                name: "FK_Families_Residences_ResidenceId",
                table: "Families",
                column: "ResidenceId",
                principalTable: "Residences",
                principalColumn: "ResidenceId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
