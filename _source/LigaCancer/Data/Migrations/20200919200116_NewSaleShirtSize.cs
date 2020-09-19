// <copyright file="20200919200116_NewSaleShirtSize.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.EntityFrameworkCore.Migrations;

namespace RVCC.Data.Migrations
{
    public partial class NewSaleShirtSize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Size2NormalQuantity",
                table: "SalesShirt2020",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Size2NormalQuantity",
                table: "SalesShirt2020");
        }
    }
}
