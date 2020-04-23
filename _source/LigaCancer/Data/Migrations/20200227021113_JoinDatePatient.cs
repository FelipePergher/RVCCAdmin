﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RVCC.Data.Migrations
{
    public partial class JoinDatePatient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TreatmentbeginDate",
                table: "PatientInformation",
                newName: "TreatmentBeginDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "JoinDate",
                table: "Patients",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JoinDate",
                table: "Patients");

            migrationBuilder.RenameColumn(
                name: "TreatmentBeginDate",
                table: "PatientInformation",
                newName: "TreatmentbeginDate");
        }
    }
}