using Microsoft.EntityFrameworkCore.Migrations;

namespace Miritush.DAL.Migrations
{
    public partial class bookIdToTrans2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "BookId_Product_BookID",
                table: "Transactions");

            migrationBuilder.AlterColumn<int>(
                name: "BookID",
                table: "Transactions",
                type: "int(11)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int(11)");

            migrationBuilder.AddForeignKey(
                name: "BookId_Product_BookID",
                table: "Transactions",
                column: "BookID",
                principalTable: "Books",
                principalColumn: "BookID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "BookId_Product_BookID",
                table: "Transactions");

            migrationBuilder.AlterColumn<int>(
                name: "BookID",
                table: "Transactions",
                type: "int(11)",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int(11)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "BookId_Product_BookID",
                table: "Transactions",
                column: "BookID",
                principalTable: "Books",
                principalColumn: "BookID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
