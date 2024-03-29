﻿// <auto-generated/>
using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RVCC.Data.Migrations
{
    public partial class TreatmentType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditPatientTreatmentTypes",
                columns: table => new
                {
                    AuditTreatmentTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientTreatmentTypeId = table.Column<int>(type: "int", nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    TreatmentTypeId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuditDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuditAction = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditPatientTreatmentTypes", x => x.AuditTreatmentTypeId);
                });

            migrationBuilder.CreateTable(
                name: "AuditTreatmentTypes",
                columns: table => new
                {
                    AuditTreatmentTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TreatmentTypeId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuditDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuditAction = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditTreatmentTypes", x => x.AuditTreatmentTypeId);
                });

            migrationBuilder.CreateTable(
                name: "TreatmentTypes",
                columns: table => new
                {
                    TreatmentTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegisterTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreatmentTypes", x => x.TreatmentTypeId);
                });

            migrationBuilder.CreateTable(
                name: "PatientTreatmentTypes",
                columns: table => new
                {
                    PatientTreatmentTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    TreatmentTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientTreatmentTypes", x => x.PatientTreatmentTypeId);
                    table.ForeignKey(
                        name: "FK_PatientTreatmentTypes_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientTreatmentTypes_TreatmentTypes_TreatmentTypeId",
                        column: x => x.TreatmentTypeId,
                        principalTable: "TreatmentTypes",
                        principalColumn: "TreatmentTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatientTreatmentTypes_PatientId",
                table: "PatientTreatmentTypes",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientTreatmentTypes_TreatmentTypeId",
                table: "PatientTreatmentTypes",
                column: "TreatmentTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditPatientTreatmentTypes");

            migrationBuilder.DropTable(
                name: "AuditTreatmentTypes");

            migrationBuilder.DropTable(
                name: "PatientTreatmentTypes");

            migrationBuilder.DropTable(
                name: "TreatmentTypes");
        }
    }
}
