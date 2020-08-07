// <copyright file="20200807191540_UpdateSaleShirt2020.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.EntityFrameworkCore.Migrations;

namespace RVCC.Data.Migrations
{
    public partial class UpdateSaleShirt2020 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SizeXGBabyLookQuantity",
                table: "SalesShirt2020",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SizeXGNormalQuantity",
                table: "SalesShirt2020",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SizeXGBabyLookQuantity",
                table: "SalesShirt2020");

            migrationBuilder.DropColumn(
                name: "SizeXGNormalQuantity",
                table: "SalesShirt2020");
        }
    }
}
