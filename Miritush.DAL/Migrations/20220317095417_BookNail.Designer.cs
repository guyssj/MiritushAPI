﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Miritush.DAL.Model;

namespace Miritush.DAL.Migrations
{
    [DbContext(typeof(booksDbContext))]
    [Migration("20220317095417_BookNail")]
    partial class BookNail
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.10");

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
                        .HasColumnType("varchar(100)")
                        .HasColumnName("MimeType");

                    b.HasKey("AttachmentId");

                    b.HasIndex(new[] { "AttachmentId" }, "AttachmentID")
                        .IsUnique();

                    b.HasIndex(new[] { "CustomerId" }, "CustomerID_Attachments");

                    b.ToTable("Attachments");
                });

            modelBuilder.Entity("Miritush.DAL.Model.Book", b =>
                {
                    b.Property<int>("BookId")
                        .ValueGeneratedOnAdd()
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

                    b.HasIndex(new[] { "CustomerId" }, "CustomerID_Books_CustomerID");

                    b.HasIndex(new[] { "ServiceId" }, "ServiceID_Books_ServiceID");

                    b.HasIndex(new[] { "ServiceTypeId" }, "ServiceTypeID_Books_ServiceTypeID");

                    b.HasIndex(new[] { "BookId", "StartDate" }, "StartDate")
                        .IsUnique();

                    b.ToTable("Books");
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

                    b.ToTable("BooksCancel");
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

                    b.ToTable("CloseDays");
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
                        .HasDefaultValueSql("'''#c96d9f'''");

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

                    b.ToTable("Customers");
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
                        .UseCollation("utf8_general_ci")
                        .HasCharSet("utf8");

                    b.HasKey("HolidayId");

                    b.ToTable("Holidays");
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

                    b.ToTable("LockHours");
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

                    b.ToTable("Services");
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

                    b.ToTable("ServiceType");
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

                    b.ToTable("Settings");
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

                    b.ToTable("Users");
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

                    b.ToTable("WorkHours");
                });

            modelBuilder.Entity("Miritush.DAL.Model.Attachment", b =>
                {
                    b.HasOne("Miritush.DAL.Model.Customer", "Customer")
                        .WithMany("Attachments")
                        .HasForeignKey("CustomerId")
                        .HasConstraintName("CustomerID_Attachments")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("Miritush.DAL.Model.Book", b =>
                {
                    b.HasOne("Miritush.DAL.Model.Customer", "Customer")
                        .WithMany("Books")
                        .HasForeignKey("CustomerId")
                        .HasConstraintName("CustomerID_Books_CustomerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Miritush.DAL.Model.Service", "Service")
                        .WithMany("Books")
                        .HasForeignKey("ServiceId")
                        .HasConstraintName("ServiceID_Books_ServiceID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Miritush.DAL.Model.Servicetype", "ServiceType")
                        .WithMany("Books")
                        .HasForeignKey("ServiceTypeId")
                        .HasConstraintName("ServiceTypeID_Books_ServiceTypeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Service");

                    b.Navigation("ServiceType");
                });

            modelBuilder.Entity("Miritush.DAL.Model.Bookscancel", b =>
                {
                    b.HasOne("Miritush.DAL.Model.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .HasConstraintName("CustomerID_BookCancel_CustomerID")
                        .IsRequired();

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

            modelBuilder.Entity("Miritush.DAL.Model.Servicetype", b =>
                {
                    b.HasOne("Miritush.DAL.Model.Service", "Service")
                        .WithMany("Servicetypes")
                        .HasForeignKey("ServiceId")
                        .HasConstraintName("ServiceIDFK")
                        .IsRequired();

                    b.Navigation("Service");
                });

            modelBuilder.Entity("Miritush.DAL.Model.Customer", b =>
                {
                    b.Navigation("Attachments");

                    b.Navigation("Books");
                });

            modelBuilder.Entity("Miritush.DAL.Model.Service", b =>
                {
                    b.Navigation("Books");

                    b.Navigation("Servicetypes");
                });

            modelBuilder.Entity("Miritush.DAL.Model.Servicetype", b =>
                {
                    b.Navigation("Books");
                });
#pragma warning restore 612, 618
        }
    }
}
