// <copyright file="20200227021113_JoinDatePatient.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.EntityFrameworkCore.Migrations;
using System;

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
