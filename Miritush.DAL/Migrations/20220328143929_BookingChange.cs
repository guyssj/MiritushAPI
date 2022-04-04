using Microsoft.EntityFrameworkCore.Migrations;

namespace Miritush.DAL.Migrations
{
    public partial class BookingChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ArrivalToken",
                table: "Books",
                type: "VARCHAR(64)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "Due",
                table: "Books",
                type: "int(11)",
                nullable: false,
                defaultValueSql: "0");

            migrationBuilder.CreateIndex(
                name: "ArrivalToken_unique",
                table: "Books",
                column: "ArrivalToken",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ArrivalToken_unique",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "ArrivalToken",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Due",
                table: "Books");
        }
    }
}
