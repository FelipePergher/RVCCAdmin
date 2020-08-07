// <copyright file="20200502193209_Stays.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RVCC.Data.Migrations
{
    public partial class Stays : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Stays",
                columns: table => new
                {
                    StayId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegisterTime = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedTime = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    PatientId = table.Column<int>(nullable: true),
                    PatientName = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    StayDateTime = table.Column<DateTime>(nullable: false),
                    Note = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stays", x => x.StayId);
                    table.ForeignKey(
                        name: "FK_Stays_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stays_PatientId",
                table: "Stays",
                column: "PatientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Stays");
        }
    }
}
