// <copyright file="20200812232631_AddedNewShirtSizes.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.EntityFrameworkCore.Migrations;

namespace RVCC.Data.Migrations
{
    public partial class AddedNewShirtSizes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Size10NormalQuantity",
                table: "SalesShirt2020",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Size6NormalQuantity",
                table: "SalesShirt2020",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Size10NormalQuantity",
                table: "SalesShirt2020");

            migrationBuilder.DropColumn(
                name: "Size6NormalQuantity",
                table: "SalesShirt2020");
        }
    }
}
