using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Miritush.DAL.Migrations
{
    public partial class booknail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CloseDays",
                columns: table => new
                {
                    CloseDaysID = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    Notes = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, defaultValueSql: "'NULL'")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.CloseDaysID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerID = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true, defaultValueSql: "'NULL'")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastName = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true, defaultValueSql: "'NULL'")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhoneNumber = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true, defaultValueSql: "'NULL'")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Color = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true, defaultValueSql: "'''#c96d9f'''")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Notes = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, defaultValueSql: "'NULL'")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OTP = table.Column<int>(type: "int(11)", nullable: true, defaultValueSql: "'NULL'"),
                    Active = table.Column<sbyte>(type: "tinyint(4)", nullable: true, defaultValueSql: "'1'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customers", x => x.CustomerID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Holidays",
                columns: table => new
                {
                    HolidayID = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    Notes = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, defaultValueSql: "'NULL'", collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_holidays", x => x.HolidayID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LockHours",
                columns: table => new
                {
                    idLockHours = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StartDate = table.Column<DateTime>(type: "date", nullable: false, defaultValueSql: "'NULL'"),
                    StartAt = table.Column<int>(type: "int(11)", nullable: false, defaultValueSql: "'NULL'"),
                    EndAt = table.Column<int>(type: "int(11)", nullable: false, defaultValueSql: "'NULL'"),
                    Notes = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, defaultValueSql: "'NULL'")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.idLockHours);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    ServiceID = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ServiceName = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true, defaultValueSql: "'NULL'")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_services", x => x.ServiceID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    SettingName = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SettingValue = table.Column<string>(type: "varchar(5000)", nullable: true, defaultValueSql: "'NULL'")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.SettingName);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Password = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserName = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RegId = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true, defaultValueSql: "'NULL'")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "WorkHours",
                columns: table => new
                {
                    DayOfWeek = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OpenTime = table.Column<int>(type: "int(11)", nullable: false),
                    CloseTime = table.Column<int>(type: "int(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.DayOfWeek);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ServiceType",
                columns: table => new
                {
                    ServiceTypeID = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ServiceTypeName = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, defaultValueSql: "'NULL'")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ServiceID = table.Column<int>(type: "int(11)", nullable: false),
                    Duration = table.Column<int>(type: "int(11)", nullable: true, defaultValueSql: "'NULL'"),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true, defaultValueSql: "'NULL'"),
                    Description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, defaultValueSql: "'NULL'")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_servicetype", x => x.ServiceTypeID);
                    table.ForeignKey(
                        name: "ServiceIDFK",
                        column: x => x.ServiceID,
                        principalTable: "services",
                        principalColumn: "ServiceID",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "books",
                columns: table => new
                {
                    BookID = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StartDate = table.Column<DateTime>(type: "date", nullable: false, defaultValueSql: "'NULL'"),
                    StartAt = table.Column<int>(type: "int(11)", nullable: false),
                    CustomerID = table.Column<int>(type: "int(11)", nullable: true, defaultValueSql: "'NULL'"),
                    ServiceID = table.Column<int>(type: "int(11)", nullable: true, defaultValueSql: "'NULL'"),
                    Durtion = table.Column<int>(type: "int(11)", nullable: true, defaultValueSql: "'NULL'"),
                    ServiceTypeID = table.Column<int>(type: "int(11)", nullable: true, defaultValueSql: "'NULL'"),
                    Notes = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, defaultValueSql: "'NULL'")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_books", x => x.BookID);
                    table.ForeignKey(
                        name: "CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "customers",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "ServiceID",
                        column: x => x.ServiceID,
                        principalTable: "services",
                        principalColumn: "ServiceID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "ServiceTypeID",
                        column: x => x.ServiceTypeID,
                        principalTable: "servicetype",
                        principalColumn: "ServiceTypeID",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "bookscancel",
                columns: table => new
                {
                    BookID = table.Column<int>(type: "int(11)", nullable: false),
                    CustomerID = table.Column<int>(type: "int(11)", nullable: false),
                    Durtion = table.Column<int>(type: "int(11)", nullable: false),
                    Notes = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, defaultValueSql: "'NULL'")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ServiceID = table.Column<int>(type: "int(11)", nullable: true, defaultValueSql: "'NULL'"),
                    ServiceTypeID = table.Column<int>(type: "int(11)", nullable: true, defaultValueSql: "'NULL'"),
                    StartAt = table.Column<int>(type: "int(11)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "date", nullable: false),
                    whyCancel = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, defaultValueSql: "'NULL'")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "CustomerIDX",
                        column: x => x.CustomerID,
                        principalTable: "customers",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "ServiceIDX",
                        column: x => x.ServiceID,
                        principalTable: "services",
                        principalColumn: "ServiceID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "ServiceTypeIDX",
                        column: x => x.ServiceTypeID,
                        principalTable: "servicetype",
                        principalColumn: "ServiceTypeID",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "CustomerID_idx",
                table: "books",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "ServiceID_idx",
                table: "books",
                column: "ServiceID");

            migrationBuilder.CreateIndex(
                name: "ServiceTypeID_idx",
                table: "books",
                column: "ServiceTypeID");

            migrationBuilder.CreateIndex(
                name: "StartDate",
                table: "books",
                columns: new[] { "BookID", "StartDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "CustomerIDX",
                table: "bookscancel",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "ServiceIDX",
                table: "bookscancel",
                column: "ServiceID");

            migrationBuilder.CreateIndex(
                name: "ServiceTypeIDX",
                table: "bookscancel",
                column: "ServiceTypeID");

            migrationBuilder.CreateIndex(
                name: "ServiceID_idx1",
                table: "servicetype",
                column: "ServiceID");

            migrationBuilder.CreateIndex(
                name: "SettingName",
                table: "settings",
                column: "SettingName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "DayOfWeek_UNIQUE",
                table: "workhours",
                column: "DayOfWeek",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "books");

            migrationBuilder.DropTable(
                name: "bookscancel");

            migrationBuilder.DropTable(
                name: "closedays");

            migrationBuilder.DropTable(
                name: "holidays");

            migrationBuilder.DropTable(
                name: "lockhours");

            migrationBuilder.DropTable(
                name: "settings");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "workhours");

            migrationBuilder.DropTable(
                name: "customers");

            migrationBuilder.DropTable(
                name: "servicetype");

            migrationBuilder.DropTable(
                name: "services");
        }
    }
}
