using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Miritush.DAL.Model
{
    public partial class booksDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public booksDbContext()
        {
        }

        public booksDbContext(DbContextOptions<booksDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Bookscancel> Bookscancels { get; set; }
        public virtual DbSet<Closeday> Closedays { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Holiday> Holidays { get; set; }
        public virtual DbSet<Lockhour> Lockhours { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<Servicetype> Servicetypes { get; set; }
        public virtual DbSet<Setting> Settings { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Workhour> Workhours { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var serverVersion = new MySqlServerVersion(new Version(5, 7, 34));

                // optionsBuilder.UseMySql("server=localhost;user=root;password=root;database=reptouch_bookNail;port=8889", serverVersion);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>(entity =>
            {
                entity.ToTable("books");

                entity.HasIndex(e => e.CustomerId, "CustomerID_idx");

                entity.HasIndex(e => e.ServiceId, "ServiceID_idx");

                entity.HasIndex(e => e.ServiceTypeId, "ServiceTypeID_idx");

                entity.HasIndex(e => new { e.BookId, e.StartDate }, "StartDate")
                    .IsUnique();

                entity.Property(e => e.BookId)
                    .HasColumnType("int(11)")
                    .HasColumnName("BookID");

                entity.Property(e => e.CustomerId)
                    .HasColumnType("int(11)")
                    .HasColumnName("CustomerID")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.Durtion)
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.Notes)
                    .HasMaxLength(500)
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.ServiceId)
                    .HasColumnType("int(11)")
                    .HasColumnName("ServiceID")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.ServiceTypeId)
                    .HasColumnType("int(11)")
                    .HasColumnName("ServiceTypeID")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.StartAt).HasColumnType("int(11)");

                entity.Property(e => e.StartDate)
                    .HasColumnType("date")
                    .HasDefaultValueSql("'NULL'");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("CustomerID");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("ServiceID");

                entity.HasOne(d => d.ServiceType)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.ServiceTypeId)
                    .HasConstraintName("ServiceTypeID");
            });

            modelBuilder.Entity<Bookscancel>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("bookscancel");

                entity.HasIndex(e => e.CustomerId, "CustomerIDX");

                entity.HasIndex(e => e.ServiceId, "ServiceIDX");

                entity.HasIndex(e => e.ServiceTypeId, "ServiceTypeIDX");

                entity.Property(e => e.BookId)
                    .HasColumnType("int(11)")
                    .HasColumnName("BookID");

                entity.Property(e => e.CustomerId)
                    .HasColumnType("int(11)")
                    .HasColumnName("CustomerID");

                entity.Property(e => e.Durtion).HasColumnType("int(11)");

                entity.Property(e => e.Notes)
                    .HasMaxLength(500)
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.ServiceId)
                    .HasColumnType("int(11)")
                    .HasColumnName("ServiceID")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.ServiceTypeId)
                    .HasColumnType("int(11)")
                    .HasColumnName("ServiceTypeID")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.StartAt).HasColumnType("int(11)");

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.Property(e => e.WhyCancel)
                    .HasMaxLength(500)
                    .HasColumnName("whyCancel")
                    .HasDefaultValueSql("'NULL'");

                entity.HasOne(d => d.Customer)
                    .WithMany()
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("CustomerIDX");

                entity.HasOne(d => d.Service)
                    .WithMany()
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("ServiceIDX");

                entity.HasOne(d => d.ServiceType)
                    .WithMany()
                    .HasForeignKey(d => d.ServiceTypeId)
                    .HasConstraintName("ServiceTypeIDX");
            });

            modelBuilder.Entity<Closeday>(entity =>
            {
                entity.HasKey(e => e.CloseDaysId)
                    .HasName("PRIMARY");

                entity.ToTable("closedays");

                entity.Property(e => e.CloseDaysId)
                    .HasColumnType("int(11)")
                    .HasColumnName("CloseDaysID");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Notes)
                    .HasMaxLength(500)
                    .HasDefaultValueSql("'NULL'");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("customers");

                entity.Property(e => e.CustomerId)
                    .HasColumnType("int(11)")
                    .HasColumnName("CustomerID");

                entity.Property(e => e.Active)
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.Color)
                    .HasMaxLength(45)
                    .HasDefaultValueSql("'''#c96d9f'''");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(250)
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.LastName)
                    .HasMaxLength(250)
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.Notes)
                    .HasMaxLength(500)
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.Otp)
                    .HasColumnType("int(11)")
                    .HasColumnName("OTP")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(250)
                    .HasDefaultValueSql("'NULL'");
            });

            modelBuilder.Entity<Holiday>(entity =>
            {
                entity.ToTable("holidays");

                entity.Property(e => e.HolidayId)
                    .HasColumnType("int(11)")
                    .HasColumnName("HolidayID");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Notes)
                    .HasMaxLength(500)
                    .HasCharSet("utf8")
                    .UseCollation("utf8_general_ci")
                    .HasDefaultValueSql("'NULL'");
            });

            modelBuilder.Entity<Lockhour>(entity =>
            {
                entity.HasKey(e => e.IdLockHours)
                    .HasName("PRIMARY");

                entity.ToTable("lockhours");

                entity.Property(e => e.IdLockHours)
                    .HasColumnType("int(11)")
                    .HasColumnName("idLockHours");

                entity.Property(e => e.EndAt)
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.Notes)
                    .HasMaxLength(500)
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.StartAt)
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.StartDate)
                    .HasColumnType("date")
                    .HasDefaultValueSql("'NULL'");
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.ToTable("services");

                entity.Property(e => e.ServiceId)
                    .HasColumnType("int(11)")
                    .HasColumnName("ServiceID");

                entity.Property(e => e.ServiceName)
                    .HasMaxLength(45)
                    .HasDefaultValueSql("'NULL'");
            });

            modelBuilder.Entity<Servicetype>(entity =>
            {
                entity.ToTable("servicetype");

                entity.HasIndex(e => e.ServiceId, "ServiceID_idx");

                entity.Property(e => e.ServiceTypeId)
                    .HasColumnType("int(11)")
                    .HasColumnName("ServiceTypeID");

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.Duration)
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(18,2)")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.ServiceId)
                    .HasColumnType("int(11)")
                    .HasColumnName("ServiceID");

                entity.Property(e => e.ServiceTypeName)
                    .HasMaxLength(255)
                    .HasDefaultValueSql("'NULL'");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.Servicetypes)
                    .HasForeignKey(d => d.ServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ServiceIDFK");
            });

            modelBuilder.Entity<Setting>(entity =>
            {
                entity.HasKey(e => e.SettingName)
                    .HasName("PRIMARY");

                entity.ToTable("settings");

                entity.HasIndex(e => e.SettingName, "SettingName")
                    .IsUnique();

                entity.Property(e => e.SettingName).HasMaxLength(500);

                entity.Property(e => e.SettingValue)
                    .HasColumnType("varchar(5000)")
                    .HasDefaultValueSql("'NULL'");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.RegId)
                    .HasMaxLength(1000)
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<Workhour>(entity =>
            {
                entity.HasKey(e => e.DayOfWeek)
                    .HasName("PRIMARY");

                entity.ToTable("workhours");

                entity.HasIndex(e => e.DayOfWeek, "DayOfWeek_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.DayOfWeek).HasColumnType("int(11)");

                entity.Property(e => e.CloseTime).HasColumnType("int(11)");

                entity.Property(e => e.OpenTime).HasColumnType("int(11)");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
