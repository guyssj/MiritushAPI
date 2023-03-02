using Microsoft.EntityFrameworkCore.Migrations;

namespace Miritush.DAL.Migrations
{
    public partial class bookIdToTrans : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BookID",
                table: "Transactions",
                type: "int(11)",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_BookID",
                table: "Transactions",
                column: "BookID");

            migrationBuilder.AddForeignKey(
                name: "BookId_Product_BookID",
                table: "Transactions",
                column: "BookID",
                principalTable: "Books",
                principalColumn: "BookID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "BookId_Product_BookID",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_BookID",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "BookID",
                table: "Transactions");
        }
    }
}
