using Microsoft.EntityFrameworkCore.Migrations;

namespace Miritush.DAL.Migrations
{
    public partial class Arrivals : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Due",
                table: "Books");

            migrationBuilder.AddColumn<int>(
                name: "ArrivalStatus",
                table: "Books",
                type: "int(11)",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArrivalStatus",
                table: "Books");

            migrationBuilder.AddColumn<int>(
                name: "Due",
                table: "Books",
                type: "int(11)",
                nullable: false,
                defaultValueSql: "0");
        }
    }
}
