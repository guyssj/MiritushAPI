using Microsoft.EntityFrameworkCore.Migrations;

namespace Miritush.DAL.Migrations
{
    public partial class BookNailAddDataSeeing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "SettingName", "SettingValue" },
                values: new object[,]
                {
                    { "SEND_SMS_APP", "1" },
                    { "MIN_AFTER_WORK", "60" },
                    { "SMS_TEMPLATE_APP", "שלום {FirstName} {LastName},\\nנקבעה לך פגישה לטיפול {ServiceType} אצל מיריתוש\\nבתאריך {Date} בשעה {Time}\\nיש להגיע עם מסכה\n" },
                    { "SMS_TEMPLATE_REMINDER", "שלום {FirstName} {LastName},\\nזאת תזכורת לטיפול {ServiceType} אצל מיריתוש\\nבתאריך {Date} בשעה {Time}\\nלא לשכוח מסכה\\n\\nלאישור הגעה יש להודיע בהודעת ווצאפ\nלמספר 0525533979" },
                    { "TIME_INTERVAL_CALENDAR", "60" },
                    { "SMS_TEMPLATE_UPAPP", "שלום {FirstName} {LastName},\\nהפגישה לטיפול {ServiceType} אצל מיריתוש\\n עודכנה לתאריך {Date} בשעה {Time}\\nיש להגיע עם מסכה" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "id", "Password", "RegId", "UserName" },
                values: new object[] { 1, "Hash", null, "admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "SettingName",
                keyValue: "MIN_AFTER_WORK");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "SettingName",
                keyValue: "SEND_SMS_APP");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "SettingName",
                keyValue: "SMS_TEMPLATE_APP");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "SettingName",
                keyValue: "SMS_TEMPLATE_REMINDER");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "SettingName",
                keyValue: "SMS_TEMPLATE_UPAPP");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "SettingName",
                keyValue: "TIME_INTERVAL_CALENDAR");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "id",
                keyValue: 1);
        }
    }
}
