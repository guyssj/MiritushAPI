using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Miritush.DAL.Migrations
{
    public partial class customerTimelines : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerTimelines",
                columns: table => new
                {
                    CustomerTimelineID = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CustomerID = table.Column<int>(type: "int(11)", nullable: false),
                    Type = table.Column<int>(type: "int(11)", nullable: false, defaultValueSql: "'0'"),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Description = table.Column<string>(type: "varchar(1000)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Notes = table.Column<string>(type: "varchar(1000)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerTimelines", x => x.CustomerTimelineID);
                    table.ForeignKey(
                        name: "CustomerId_CustomerTimelines_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "CustomerId_IDX_CustomerID",
                table: "CustomerTimelines",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "CustomerTimelineID",
                table: "CustomerTimelines",
                column: "CustomerTimelineID",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerTimelines");
        }
    }
}
