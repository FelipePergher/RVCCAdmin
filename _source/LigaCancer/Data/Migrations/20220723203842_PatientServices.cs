﻿// <auto-generated/>
using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RVCC.Data.Migrations
{
    public partial class PatientServices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ExpenseTypes",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "AuditPatientInformationServiceTypes",
                columns: table => new
                {
                    AuditPatientInformationServiceTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientInformationId = table.Column<int>(type: "int", nullable: false),
                    ServiceTypeId = table.Column<int>(type: "int", nullable: false),
                    AuditDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuditAction = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditPatientInformationServiceTypes", x => x.AuditPatientInformationServiceTypeId);
                });

            migrationBuilder.CreateTable(
                name: "AuditServiceTypes",
                columns: table => new
                {
                    AuditServiceTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceTypeId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuditDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuditAction = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditServiceTypes", x => x.AuditServiceTypeId);
                });

            migrationBuilder.CreateTable(
                name: "ServiceTypes",
                columns: table => new
                {
                    ServiceTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RegisterTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceTypes", x => x.ServiceTypeId);
                });

            migrationBuilder.CreateTable(
                name: "PatientInformationServiceType",
                columns: table => new
                {
                    PatientInformationId = table.Column<int>(type: "int", nullable: false),
                    ServiceTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientInformationServiceType", x => new { x.PatientInformationId, x.ServiceTypeId });
                    table.ForeignKey(
                        name: "FK_PatientInformationServiceType_PatientInformation_PatientInformationId",
                        column: x => x.PatientInformationId,
                        principalTable: "PatientInformation",
                        principalColumn: "PatientInformationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientInformationServiceType_ServiceTypes_ServiceTypeId",
                        column: x => x.ServiceTypeId,
                        principalTable: "ServiceTypes",
                        principalColumn: "ServiceTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseTypes_Name",
                table: "ExpenseTypes",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PatientInformationServiceType_ServiceTypeId",
                table: "PatientInformationServiceType",
                column: "ServiceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTypes_Name",
                table: "ServiceTypes",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditPatientInformationServiceTypes");

            migrationBuilder.DropTable(
                name: "AuditServiceTypes");

            migrationBuilder.DropTable(
                name: "PatientInformationServiceType");

            migrationBuilder.DropTable(
                name: "ServiceTypes");

            migrationBuilder.DropIndex(
                name: "IX_ExpenseTypes_Name",
                table: "ExpenseTypes");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ExpenseTypes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
