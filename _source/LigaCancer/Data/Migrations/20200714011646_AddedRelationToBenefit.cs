// <copyright file="20200714011646_AddedRelationToBenefit.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

using Microsoft.EntityFrameworkCore.Migrations;

namespace RVCC.Data.Migrations
{
    public partial class AddedRelationToBenefit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Presences_PatientId",
                table: "Presences",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Presences_Patients_PatientId",
                table: "Presences",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "PatientId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Presences_Patients_PatientId",
                table: "Presences");

            migrationBuilder.DropIndex(
                name: "IX_Presences_PatientId",
                table: "Presences");
        }
    }
}
