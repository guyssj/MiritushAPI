using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Miritush.DAL.Migrations
{
    public partial class Transcations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    TransactionID = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CustomerID = table.Column<int>(type: "int(11)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Status = table.Column<sbyte>(type: "tinyint(4)", nullable: false, defaultValueSql: "'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.TransactionID);
                    table.ForeignKey(
                        name: "CustomerId_Product_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TransactionItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TranscationID = table.Column<int>(type: "int(11)", nullable: false),
                    ProductID = table.Column<int>(type: "int(11)", nullable: true),
                    ServiceTypeID = table.Column<int>(type: "int(11)", nullable: true),
                    Quantity = table.Column<int>(type: "int(11)", nullable: false, defaultValueSql: "'0'"),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true, defaultValueSql: "'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionItems", x => x.Id);
                    table.ForeignKey(
                        name: "ProductId_Product_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "ServiceTypeID_ServiceType_ServiceTypeID",
                        column: x => x.ServiceTypeID,
                        principalTable: "ServiceType",
                        principalColumn: "ServiceTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "TranscationId_Transcations_TranscationId",
                        column: x => x.TranscationID,
                        principalTable: "Transactions",
                        principalColumn: "TransactionID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "ProductId_Product_ProductID",
                table: "TransactionItems",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "ServiceTypeID_ServiceType_ServiceTypeID",
                table: "TransactionItems",
                column: "ServiceTypeID");

            migrationBuilder.CreateIndex(
                name: "TransactionItemID",
                table: "TransactionItems",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "TranscationId_Transcations_TranscationId",
                table: "TransactionItems",
                column: "TranscationID");

            migrationBuilder.CreateIndex(
                name: "CustomerId_Product_CustomerID",
                table: "Transactions",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "TransactionID",
                table: "Transactions",
                column: "TransactionID",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransactionItems");

            migrationBuilder.DropTable(
                name: "Transactions");
        }
    }
}
