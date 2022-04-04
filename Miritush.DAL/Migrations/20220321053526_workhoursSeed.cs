using Microsoft.EntityFrameworkCore.Migrations;

namespace Miritush.DAL.Migrations
{
    public partial class workhoursSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "Customers",
                type: "varchar(45)",
                maxLength: 45,
                nullable: true,
                defaultValueSql: "'#c96d9f'",
                oldClrType: typeof(string),
                oldType: "varchar(45)",
                oldMaxLength: 45,
                oldNullable: true,
                oldDefaultValueSql: "'''#c96d9f'''")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "WorkHours",
                columns: new[] { "DayOfWeek", "CloseTime", "OpenTime" },
                values: new object[,]
                {
                    { 1, 1095, 540 },
                    { 2, 1095, 540 },
                    { 3, 1095, 540 },
                    { 4, 1095, 540 },
                    { 5, 930, 540 },
                    { 6, 840, 540 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "WorkHours",
                keyColumn: "DayOfWeek",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "WorkHours",
                keyColumn: "DayOfWeek",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "WorkHours",
                keyColumn: "DayOfWeek",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "WorkHours",
                keyColumn: "DayOfWeek",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "WorkHours",
                keyColumn: "DayOfWeek",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "WorkHours",
                keyColumn: "DayOfWeek",
                keyValue: 6);

            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "Customers",
                type: "varchar(45)",
                maxLength: 45,
                nullable: true,
                defaultValueSql: "'''#c96d9f'''",
                oldClrType: typeof(string),
                oldType: "varchar(45)",
                oldMaxLength: 45,
                oldNullable: true,
                oldDefaultValueSql: "'#c96d9f'")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
