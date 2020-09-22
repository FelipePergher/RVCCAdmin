﻿// <copyright file="20200922014814_MinSalary.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RVCC.Data.Migrations
{
    public partial class MinSalary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "MonthlyIncomeMinSalary",
                table: "Patients",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MonthlyIncomeMinSalary",
                table: "FamilyMembers",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "AdminInfos",
                columns: table => new
                {
                    AdminInfoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegisterTime = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedTime = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    MinSalary = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminInfos", x => x.AdminInfoId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminInfos");

            migrationBuilder.DropColumn(
                name: "MonthlyIncomeMinSalary",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "MonthlyIncomeMinSalary",
                table: "FamilyMembers");
        }
    }
}
