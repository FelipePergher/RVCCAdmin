﻿// <auto-generated/>
using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RVCC.Data.Migrations
{
    public partial class Shirt2020 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SalesShirt2020",
                columns: table => new
                {
                    ShirtSaleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegisterTime = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedTime = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    ShirtQuantityTotal = table.Column<int>(nullable: false),
                    PriceTotal = table.Column<double>(nullable: false),
                    BuyerName = table.Column<string>(nullable: true),
                    BuyerPhone = table.Column<string>(nullable: true),
                    DateOrdered = table.Column<DateTime>(nullable: false),
                    DatePayment = table.Column<DateTime>(nullable: false),
                    DateConfection = table.Column<DateTime>(nullable: false),
                    DateProduced = table.Column<DateTime>(nullable: false),
                    DateCollected = table.Column<DateTime>(nullable: false),
                    DateCanceled = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    MaskQuantity = table.Column<int>(nullable: false),
                    Size4NormalQuantity = table.Column<int>(nullable: false),
                    Size8NormalQuantity = table.Column<int>(nullable: false),
                    Size12NormalQuantity = table.Column<int>(nullable: false),
                    Size14NormalQuantity = table.Column<int>(nullable: false),
                    Size16NormalQuantity = table.Column<int>(nullable: false),
                    SizePNormalQuantity = table.Column<int>(nullable: false),
                    SizeMNormalQuantity = table.Column<int>(nullable: false),
                    SizeGNormalQuantity = table.Column<int>(nullable: false),
                    SizeGGNormalQuantity = table.Column<int>(nullable: false),
                    SizePBabyLookQuantity = table.Column<int>(nullable: false),
                    SizeMBabyLookQuantity = table.Column<int>(nullable: false),
                    SizeGBabyLookQuantity = table.Column<int>(nullable: false),
                    SizeGGBabyLookQuantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesShirt2020", x => x.ShirtSaleId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SalesShirt2020");
        }
    }
}
