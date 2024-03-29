﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Miritush.DAL.Model;

#nullable disable

namespace Miritush.DAL.Migrations
{
    [DbContext(typeof(booksDbContext))]
    [Migration("20230324125101_customerTimelines")]
    partial class customerTimelines
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Miritush.DAL.Model.Attachment", b =>
                {
                    b.Property<int>("AttachmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)")
                        .HasColumnName("AttachmentID");

                    b.Property<string>("AttachmentName")
                        .HasColumnType("varchar(400)")
                        .HasColumnName("AttachmentName");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int(11)")
                        .HasColumnName("CustomerID");

                    b.Property<string>("MimeType")
                        .HasColumnType("varchar(500)")
                        .HasColumnName("MimeType");

                    b.HasKey("AttachmentId");

                    b.HasIndex(new[] { "AttachmentId" }, "AttachmentID")
                        .IsUnique();

                    b.HasIndex(new[] { "CustomerId" }, "CustomerID_Attachments");

                    b.ToTable("Attachments", (string)null);
                });

            modelBuilder.Entity("Miritush.DAL.Model.Book", b =>
                {
                    b.Property<int>("BookId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)")
                        .HasColumnName("BookID");

                    b.Property<int>("ArrivalStatus")
                        .HasColumnType("int(11)");

                    b.Property<string>("ArrivalToken")
                        .HasColumnType("VARCHAR(64)")
                        .HasColumnName("ArrivalToken");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int(11)")
                        .HasColumnName("CustomerID");

                    b.Property<int>("Durtion")
                        .HasColumnType("int(11)");

                    b.Property<string>("Notes")
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<int>("ServiceId")
                        .HasColumnType("int(11)")
                        .HasColumnName("ServiceID");

                    b.Property<int>("ServiceTypeId")
                        .HasColumnType("int(11)")
                        .HasColumnName("ServiceTypeID");

                    b.Property<int>("StartAt")
                        .HasColumnType("int(11)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("date");

                    b.HasKey("BookId");

                    b.HasIndex(new[] { "ArrivalToken" }, "ArrivalToken_unique")
                        .IsUnique();

                    b.HasIndex(new[] { "CustomerId" }, "CustomerID_Books_CustomerID");

                    b.HasIndex(new[] { "ServiceId" }, "ServiceID_Books_ServiceID");

                    b.HasIndex(new[] { "ServiceTypeId" }, "ServiceTypeID_Books_ServiceTypeID");

                    b.HasIndex(new[] { "BookId", "StartDate" }, "StartDate")
                        .IsUnique();

                    b.ToTable("Books", (string)null);
                });

            modelBuilder.Entity("Miritush.DAL.Model.Bookscancel", b =>
                {
                    b.Property<int>("BookId")
                        .HasColumnType("int(11)")
                        .HasColumnName("BookID");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int(11)")
                        .HasColumnName("CustomerID");

                    b.Property<int>("Durtion")
                        .HasColumnType("int(11)");

                    b.Property<string>("Notes")
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<int?>("ServiceId")
                        .HasColumnType("int(11)")
                        .HasColumnName("ServiceID");

                    b.Property<int?>("ServiceTypeId")
                        .HasColumnType("int(11)")
                        .HasColumnName("ServiceTypeID");

                    b.Property<int>("StartAt")
                        .HasColumnType("int(11)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("date");

                    b.Property<string>("WhyCancel")
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)")
                        .HasColumnName("whyCancel");

                    b.HasIndex(new[] { "CustomerId" }, "CustomerID_BookCancel_CustomerID");

                    b.HasIndex(new[] { "ServiceId" }, "ServiceID_BookCancel_ServiceID");

                    b.HasIndex(new[] { "ServiceTypeId" }, "ServiceTypeID_BookCacnel_ServiceTypeID");

                    b.ToTable("BooksCancel", (string)null);
                });

            modelBuilder.Entity("Miritush.DAL.Model.Closeday", b =>
                {
                    b.Property<int>("CloseDaysId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)")
                        .HasColumnName("CloseDaysID");

                    b.Property<DateTime>("Date")
                        .HasColumnType("date");

                    b.Property<string>("Notes")
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.HasKey("CloseDaysId")
                        .HasName("PRIMARY");

                    b.ToTable("CloseDays", (string)null);
                });

            modelBuilder.Entity("Miritush.DAL.Model.Customer", b =>
                {
                    b.Property<int>("CustomerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)")
                        .HasColumnName("CustomerID");

                    b.Property<sbyte?>("Active")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(4)")
                        .HasDefaultValueSql("'1'");

                    b.Property<string>("Color")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)")
                        .HasDefaultValueSql("'#c96d9f'");

                    b.Property<string>("FirstName")
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)");

                    b.Property<string>("LastName")
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)");

                    b.Property<string>("Notes")
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<int?>("Otp")
                        .HasColumnType("int(11)")
                        .HasColumnName("OTP");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)");

                    b.HasKey("CustomerId");

                    b.ToTable("Customers", (string)null);
                });

            modelBuilder.Entity("Miritush.DAL.Model.CustomerTimeline", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)")
                        .HasColumnName("CustomerTimelineID");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int(11)")
                        .HasColumnName("CustomerID");

                    b.Property<string>("Description")
                        .HasColumnType("varchar(1000)")
                        .HasColumnName("Description");

                    b.Property<string>("Notes")
                        .HasColumnType("varchar(1000)")
                        .HasColumnName("Notes");

                    b.Property<int>("Type")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)")
                        .HasColumnName("Type")
                        .HasDefaultValueSql("'0'");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "CustomerId" }, "CustomerId_IDX_CustomerID");

                    b.HasIndex(new[] { "Id" }, "CustomerTimelineID")
                        .IsUnique();

                    b.ToTable("CustomerTimelines", (string)null);
                });

            modelBuilder.Entity("Miritush.DAL.Model.Holiday", b =>
                {
                    b.Property<int>("HolidayId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)")
                        .HasColumnName("HolidayID");

                    b.Property<DateTime>("Date")
                        .HasColumnType("date");

                    b.Property<string>("Notes")
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)")
                        .UseCollation("utf8_general_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("Notes"), "utf8");

                    b.HasKey("HolidayId");

                    b.ToTable("Holidays", (string)null);
                });

            modelBuilder.Entity("Miritush.DAL.Model.Lockhour", b =>
                {
                    b.Property<int>("IdLockHours")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)")
                        .HasColumnName("idLockHours");

                    b.Property<int>("EndAt")
                        .HasColumnType("int(11)");

                    b.Property<string>("Notes")
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<int>("StartAt")
                        .HasColumnType("int(11)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("date");

                    b.HasKey("IdLockHours")
                        .HasName("PRIMARY");

                    b.ToTable("LockHours", (string)null);
                });

            modelBuilder.Entity("Miritush.DAL.Model.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)")
                        .HasColumnName("ProductID");

                    b.Property<sbyte>("Active")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(4)")
                        .HasDefaultValueSql("'1'");

                    b.Property<int>("CategoryID")
                        .HasColumnType("int(11)")
                        .HasColumnName("CategoryID");

                    b.Property<string>("Description")
                        .HasColumnType("varchar(5000)")
                        .HasColumnName("ProductDescription");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(5000)")
                        .HasColumnName("ProductName");

                    b.Property<decimal?>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "CategoryID" }, "CategoryID_Products");

                    b.HasIndex(new[] { "Id" }, "ProductID")
                        .IsUnique();

                    b.ToTable("Products", (string)null);
                });

            modelBuilder.Entity("Miritush.DAL.Model.ProductCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)")
                        .HasColumnName("CategoryID");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(5000)")
                        .HasColumnName("CategoryName");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Id" }, "CategoryID")
                        .IsUnique();

                    b.ToTable("ProductCategorys", (string)null);
                });

            modelBuilder.Entity("Miritush.DAL.Model.Service", b =>
                {
                    b.Property<int>("ServiceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)")
                        .HasColumnName("ServiceID");

                    b.Property<string>("ServiceName")
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)");

                    b.HasKey("ServiceId");

                    b.ToTable("Services", (string)null);
                });

            modelBuilder.Entity("Miritush.DAL.Model.Servicetype", b =>
                {
                    b.Property<int>("ServiceTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)")
                        .HasColumnName("ServiceTypeID");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<int?>("Duration")
                        .HasColumnType("int(11)");

                    b.Property<decimal?>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ServiceId")
                        .HasColumnType("int(11)")
                        .HasColumnName("ServiceID");

                    b.Property<string>("ServiceTypeName")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("ServiceTypeId");

                    b.HasIndex(new[] { "ServiceId" }, "ServiceID_idx");

                    b.ToTable("ServiceType", (string)null);
                });

            modelBuilder.Entity("Miritush.DAL.Model.Setting", b =>
                {
                    b.Property<string>("SettingName")
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<string>("SettingValue")
                        .HasColumnType("varchar(5000)");

                    b.HasKey("SettingName")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "SettingName" }, "SettingName")
                        .IsUnique();

                    b.ToTable("Settings", (string)null);

                    b.HasData(
                        new
                        {
                            SettingName = "SEND_SMS_APP",
                            SettingValue = "1"
                        },
                        new
                        {
                            SettingName = "MIN_AFTER_WORK",
                            SettingValue = "60"
                        },
                        new
                        {
                            SettingName = "SMS_TEMPLATE_APP",
                            SettingValue = "שלום {FirstName} {LastName},\\nנקבעה לך פגישה לטיפול {ServiceType} אצל מיריתוש\\nבתאריך {Date} בשעה {Time}\\nיש להגיע עם מסכה\n"
                        },
                        new
                        {
                            SettingName = "SMS_TEMPLATE_REMINDER",
                            SettingValue = "שלום {FirstName} {LastName},\\nזאת תזכורת לטיפול {ServiceType} אצל מיריתוש\\nבתאריך {Date} בשעה {Time}\\nלא לשכוח מסכה\\n\\nלאישור הגעה יש להודיע בהודעת ווצאפ\nלמספר 0525533979"
                        },
                        new
                        {
                            SettingName = "TIME_INTERVAL_CALENDAR",
                            SettingValue = "60"
                        },
                        new
                        {
                            SettingName = "SMS_TEMPLATE_UPAPP",
                            SettingValue = "שלום {FirstName} {LastName},\\nהפגישה לטיפול {ServiceType} אצל מיריתוש\\n עודכנה לתאריך {Date} בשעה {Time}\\nיש להגיע עם מסכה"
                        });
                });

            modelBuilder.Entity("Miritush.DAL.Model.Transaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)")
                        .HasColumnName("TransactionID");

                    b.Property<int?>("BookId")
                        .HasColumnType("int(11)")
                        .HasColumnName("BookID");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int(11)")
                        .HasColumnName("CustomerID");

                    b.Property<sbyte>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(4)")
                        .HasDefaultValueSql("'0'");

                    b.HasKey("Id");

                    b.HasIndex("BookId");

                    b.HasIndex(new[] { "CustomerId" }, "CustomerId_Product_CustomerID");

                    b.HasIndex(new[] { "Id" }, "TransactionID")
                        .IsUnique();

                    b.ToTable("Transactions", (string)null);
                });

            modelBuilder.Entity("Miritush.DAL.Model.TransactionItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<decimal?>("Price")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(18,2)")
                        .HasDefaultValueSql("'0'");

                    b.Property<int?>("ProductId")
                        .HasColumnType("int(11)")
                        .HasColumnName("ProductID");

                    b.Property<int>("Quantity")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)")
                        .HasDefaultValueSql("'0'");

                    b.Property<int?>("ServiceTypeId")
                        .HasColumnType("int(11)")
                        .HasColumnName("ServiceTypeID");

                    b.Property<int>("TranscationId")
                        .HasColumnType("int(11)")
                        .HasColumnName("TranscationID");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "ProductId" }, "ProductId_Product_ProductID");

                    b.HasIndex(new[] { "ServiceTypeId" }, "ServiceTypeID_ServiceType_ServiceTypeID");

                    b.HasIndex(new[] { "Id" }, "TransactionItemID")
                        .IsUnique();

                    b.HasIndex(new[] { "TranscationId" }, "TranscationId_Transcations_TranscationId");

                    b.ToTable("TransactionItems", (string)null);
                });

            modelBuilder.Entity("Miritush.DAL.Model.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)")
                        .HasColumnName("id");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<string>("RegId")
                        .HasMaxLength(1000)
                        .HasColumnType("varchar(1000)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.HasKey("Id");

                    b.ToTable("Users", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Password = "Hash",
                            UserName = "admin"
                        });
                });

            modelBuilder.Entity("Miritush.DAL.Model.Workhour", b =>
                {
                    b.Property<int>("DayOfWeek")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)");

                    b.Property<int>("CloseTime")
                        .HasColumnType("int(11)");

                    b.Property<int>("OpenTime")
                        .HasColumnType("int(11)");

                    b.HasKey("DayOfWeek")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "DayOfWeek" }, "DayOfWeek_UNIQUE")
                        .IsUnique();

                    b.ToTable("WorkHours", (string)null);

                    b.HasData(
                        new
                        {
                            DayOfWeek = 1,
                            CloseTime = 1095,
                            OpenTime = 540
                        },
                        new
                        {
                            DayOfWeek = 2,
                            CloseTime = 1095,
                            OpenTime = 540
                        },
                        new
                        {
                            DayOfWeek = 3,
                            CloseTime = 1095,
                            OpenTime = 540
                        },
                        new
                        {
                            DayOfWeek = 4,
                            CloseTime = 1095,
                            OpenTime = 540
                        },
                        new
                        {
                            DayOfWeek = 5,
                            CloseTime = 930,
                            OpenTime = 540
                        },
                        new
                        {
                            DayOfWeek = 6,
                            CloseTime = 840,
                            OpenTime = 540
                        });
                });

            modelBuilder.Entity("Miritush.DAL.Model.Attachment", b =>
                {
                    b.HasOne("Miritush.DAL.Model.Customer", "Customer")
                        .WithMany("Attachments")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("CustomerID_Attachments");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("Miritush.DAL.Model.Book", b =>
                {
                    b.HasOne("Miritush.DAL.Model.Customer", "Customer")
                        .WithMany("Books")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("CustomerID_Books_CustomerID");

                    b.HasOne("Miritush.DAL.Model.Service", "Service")
                        .WithMany("Books")
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("ServiceID_Books_ServiceID");

                    b.HasOne("Miritush.DAL.Model.Servicetype", "ServiceType")
                        .WithMany("Books")
                        .HasForeignKey("ServiceTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("ServiceTypeID_Books_ServiceTypeID");

                    b.Navigation("Customer");

                    b.Navigation("Service");

                    b.Navigation("ServiceType");
                });

            modelBuilder.Entity("Miritush.DAL.Model.Bookscancel", b =>
                {
                    b.HasOne("Miritush.DAL.Model.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .IsRequired()
                        .HasConstraintName("CustomerID_BookCancel_CustomerID");

                    b.HasOne("Miritush.DAL.Model.Service", "Service")
                        .WithMany()
                        .HasForeignKey("ServiceId")
                        .HasConstraintName("ServiceID_BookCancel_ServiceID");

                    b.HasOne("Miritush.DAL.Model.Servicetype", "ServiceType")
                        .WithMany()
                        .HasForeignKey("ServiceTypeId")
                        .HasConstraintName("ServiceTypeID_BookCacnel_ServiceTypeID");

                    b.Navigation("Customer");

                    b.Navigation("Service");

                    b.Navigation("ServiceType");
                });

            modelBuilder.Entity("Miritush.DAL.Model.CustomerTimeline", b =>
                {
                    b.HasOne("Miritush.DAL.Model.Customer", "Customer")
                        .WithMany("CustomerTimelines")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("CustomerId_CustomerTimelines_CustomerID");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("Miritush.DAL.Model.Product", b =>
                {
                    b.HasOne("Miritush.DAL.Model.ProductCategory", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("CategoryID_Products");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Miritush.DAL.Model.Servicetype", b =>
                {
                    b.HasOne("Miritush.DAL.Model.Service", "Service")
                        .WithMany("Servicetypes")
                        .HasForeignKey("ServiceId")
                        .IsRequired()
                        .HasConstraintName("ServiceIDFK");

                    b.Navigation("Service");
                });

            modelBuilder.Entity("Miritush.DAL.Model.Transaction", b =>
                {
                    b.HasOne("Miritush.DAL.Model.Book", "Book")
                        .WithMany("Transactions")
                        .HasForeignKey("BookId")
                        .HasConstraintName("BookId_Product_BookID");

                    b.HasOne("Miritush.DAL.Model.Customer", "Customer")
                        .WithMany("Transactions")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("CustomerId_Product_CustomerID");

                    b.Navigation("Book");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("Miritush.DAL.Model.TransactionItem", b =>
                {
                    b.HasOne("Miritush.DAL.Model.Product", "Product")
                        .WithMany("TransactionItems")
                        .HasForeignKey("ProductId")
                        .HasConstraintName("ProductId_Product_ProductID");

                    b.HasOne("Miritush.DAL.Model.Servicetype", "ServiceType")
                        .WithMany("TransactionItems")
                        .HasForeignKey("ServiceTypeId")
                        .HasConstraintName("ServiceTypeID_ServiceType_ServiceTypeID");

                    b.HasOne("Miritush.DAL.Model.Transaction", "Transaction")
                        .WithMany("TransactionItems")
                        .HasForeignKey("TranscationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("TranscationId_Transcations_TranscationId");

                    b.Navigation("Product");

                    b.Navigation("ServiceType");

                    b.Navigation("Transaction");
                });

            modelBuilder.Entity("Miritush.DAL.Model.Book", b =>
                {
                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("Miritush.DAL.Model.Customer", b =>
                {
                    b.Navigation("Attachments");

                    b.Navigation("Books");

                    b.Navigation("CustomerTimelines");

                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("Miritush.DAL.Model.Product", b =>
                {
                    b.Navigation("TransactionItems");
                });

            modelBuilder.Entity("Miritush.DAL.Model.ProductCategory", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("Miritush.DAL.Model.Service", b =>
                {
                    b.Navigation("Books");

                    b.Navigation("Servicetypes");
                });

            modelBuilder.Entity("Miritush.DAL.Model.Servicetype", b =>
                {
                    b.Navigation("Books");

                    b.Navigation("TransactionItems");
                });

            modelBuilder.Entity("Miritush.DAL.Model.Transaction", b =>
                {
                    b.Navigation("TransactionItems");
                });
#pragma warning restore 612, 618
        }
    }
}
