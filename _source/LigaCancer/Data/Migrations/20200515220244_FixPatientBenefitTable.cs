// <copyright file="20200515220244_FixPatientBenefitTable.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

using Microsoft.EntityFrameworkCore.Migrations;

namespace RVCC.Data.Migrations
{
    public partial class FixPatientBenefitTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PatientBenefits",
                table: "PatientBenefits");

            migrationBuilder.RenameColumn(
                name: "MonthlyAmmountResidence",
                "Addresses",
                "MonthlyAmountResidence");

            migrationBuilder.AddColumn<int>(
                name: "PatientBenefitId",
                table: "PatientBenefits",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PatientBenefits",
                table: "PatientBenefits",
                column: "PatientBenefitId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientBenefits_PatientId",
                table: "PatientBenefits",
                column: "PatientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PatientBenefits",
                table: "PatientBenefits");

            migrationBuilder.DropIndex(
                name: "IX_PatientBenefits_PatientId",
                table: "PatientBenefits");

            migrationBuilder.DropColumn(
                name: "PatientBenefitId",
                table: "PatientBenefits");

            migrationBuilder.RenameColumn(
                name: "MonthlyAmountResidence",
                "Addresses",
                "MonthlyAmmountResidence");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PatientBenefits",
                table: "PatientBenefits",
                columns: new[] { "PatientId", "BenefitId" });
        }
    }
}
