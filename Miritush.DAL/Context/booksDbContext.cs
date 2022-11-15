using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Miritush.DAL.Model
{
    public partial class booksDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public booksDbContext() : base()
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
        public virtual DbSet<Attachment> Attachments { get; set; }
        public virtual DbSet<ProductCategory> ProductCategorys { get; set; }
        public virtual DbSet<Product> Products { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var serverVersion = new MySqlServerVersion(new Version(5, 7, 34));

                optionsBuilder.UseMySql("server=localhost;user=root;password=root;database=BookNail;port=8889", serverVersion);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>(entity =>
            {
                entity.ToTable("Books");

                entity.HasIndex(e => e.CustomerId, "CustomerID_Books_CustomerID");

                entity.HasIndex(e => e.ServiceId, "ServiceID_Books_ServiceID");

                entity.HasIndex(e => e.ServiceTypeId, "ServiceTypeID_Books_ServiceTypeID");

                entity.HasIndex(e => new { e.BookId, e.StartDate }, "StartDate")
                    .IsUnique();

                entity.Property(e => e.BookId)
                    .HasColumnType("int(11)")
                    .HasColumnName("BookID");

                entity.Property(e => e.CustomerId)
                    .HasColumnType("int(11)")
                    .HasColumnName("CustomerID")
                    .HasDefaultValueSql(null);

                entity.Property(e => e.Durtion)
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql(null);

                entity.Property(e => e.Notes)
                    .HasMaxLength(500)
                    .HasDefaultValueSql(null);

                entity.Property(e => e.ServiceId)
                    .HasColumnType("int(11)")
                    .HasColumnName("ServiceID")
                    .HasDefaultValueSql(null);

                entity.Property(e => e.ArrivalStatus)
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql(null);

                entity.Property(e => e.ArrivalToken)
                    .HasColumnType("VARCHAR(64)")
                    .HasColumnName("ArrivalToken");

                entity.HasIndex(e => e.ArrivalToken, "ArrivalToken_unique")
                    .IsUnique();

                entity.Property(e => e.ServiceTypeId)
                    .HasColumnType("int(11)")
                    .HasColumnName("ServiceTypeID")
                    .HasDefaultValueSql(null);

                entity.Property(e => e.StartAt).HasColumnType("int(11)");

                entity.Property(e => e.StartDate)
                    .HasColumnType("date")
                    .HasDefaultValueSql(null);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("CustomerID_Books_CustomerID");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("ServiceID_Books_ServiceID");

                entity.HasOne(d => d.ServiceType)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.ServiceTypeId)
                    .HasConstraintName("ServiceTypeID_Books_ServiceTypeID");
            });

            modelBuilder.Entity<Bookscancel>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("BooksCancel");

                entity.HasIndex(e => e.CustomerId, "CustomerID_BookCancel_CustomerID");

                entity.HasIndex(e => e.ServiceId, "ServiceID_BookCancel_ServiceID");

                entity.HasIndex(e => e.ServiceTypeId, "ServiceTypeID_BookCacnel_ServiceTypeID");

                entity.Property(e => e.BookId)
                    .HasColumnType("int(11)")
                    .HasColumnName("BookID");

                entity.Property(e => e.CustomerId)
                    .HasColumnType("int(11)")
                    .HasColumnName("CustomerID");

                entity.Property(e => e.Durtion).HasColumnType("int(11)");

                entity.Property(e => e.Notes)
                    .HasMaxLength(500)
                    .HasDefaultValueSql(null);

                entity.Property(e => e.ServiceId)
                    .HasColumnType("int(11)")
                    .HasColumnName("ServiceID")
                    .HasDefaultValueSql(null);

                entity.Property(e => e.ServiceTypeId)
                    .HasColumnType("int(11)")
                    .HasColumnName("ServiceTypeID")
                    .HasDefaultValueSql(null);

                entity.Property(e => e.StartAt).HasColumnType("int(11)");

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.Property(e => e.WhyCancel)
                    .HasMaxLength(500)
                    .HasColumnName("whyCancel")
                    .HasDefaultValueSql(null);

                entity.HasOne(d => d.Customer)
                    .WithMany()
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("CustomerID_BookCancel_CustomerID");

                entity.HasOne(d => d.Service)
                    .WithMany()
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("ServiceID_BookCancel_ServiceID");

                entity.HasOne(d => d.ServiceType)
                    .WithMany()
                    .HasForeignKey(d => d.ServiceTypeId)
                    .HasConstraintName("ServiceTypeID_BookCacnel_ServiceTypeID");
            });

            modelBuilder.Entity<Closeday>(entity =>
            {
                entity.HasKey(e => e.CloseDaysId)
                    .HasName("PRIMARY");

                entity.ToTable("CloseDays");

                entity.Property(e => e.CloseDaysId)
                    .HasColumnType("int(11)")
                    .HasColumnName("CloseDaysID");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Notes)
                    .HasMaxLength(500)
                    .HasDefaultValueSql(null);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customers");

                entity.Property(e => e.CustomerId)
                    .HasColumnType("int(11)")
                    .HasColumnName("CustomerID");

                entity.Property(e => e.Active)
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.Color)
                    .HasMaxLength(45)
                    .HasDefaultValueSql("'#c96d9f'");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(250)
                    .HasDefaultValueSql(null);

                entity.Property(e => e.LastName)
                    .HasMaxLength(250)
                    .HasDefaultValueSql(null);

                entity.Property(e => e.Notes)
                    .HasMaxLength(500)
                    .HasDefaultValueSql(null);

                entity.Property(e => e.Otp)
                    .HasColumnType("int(11)")
                    .HasColumnName("OTP")
                    .HasDefaultValueSql(null);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(250)
                    .HasDefaultValueSql(null);
            });

            modelBuilder.Entity<Holiday>(entity =>
            {
                entity.ToTable("Holidays");

                entity.Property(e => e.HolidayId)
                    .HasColumnType("int(11)")
                    .HasColumnName("HolidayID");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Notes)
                    .HasMaxLength(500)
                    .HasCharSet("utf8")
                    .UseCollation("utf8_general_ci")
                    .HasDefaultValueSql(null);
            });

            modelBuilder.Entity<Lockhour>(entity =>
            {
                entity.HasKey(e => e.IdLockHours)
                    .HasName("PRIMARY");

                entity.ToTable("LockHours");

                entity.Property(e => e.IdLockHours)
                    .HasColumnType("int(11)")
                    .HasColumnName("idLockHours");

                entity.Property(e => e.EndAt)
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql(null);

                entity.Property(e => e.Notes)
                    .HasMaxLength(500)
                    .HasDefaultValueSql(null);

                entity.Property(e => e.StartAt)
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql(null);

                entity.Property(e => e.StartDate)
                    .HasColumnType("date")
                    .HasDefaultValueSql(null);
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.ToTable("Services");

                entity.Property(e => e.ServiceId)
                    .HasColumnType("int(11)")
                    .HasColumnName("ServiceID");

                entity.Property(e => e.ServiceName)
                    .HasMaxLength(45)
                    .HasDefaultValueSql(null);
            });

            modelBuilder.Entity<Servicetype>(entity =>
            {
                entity.ToTable("ServiceType");

                entity.HasIndex(e => e.ServiceId, "ServiceID_idx");

                entity.Property(e => e.ServiceTypeId)
                    .HasColumnType("int(11)")
                    .HasColumnName("ServiceTypeID");

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .HasDefaultValueSql(null);

                entity.Property(e => e.Duration)
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql(null);

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(18,2)")
                    .HasDefaultValueSql(null);

                entity.Property(e => e.ServiceId)
                    .HasColumnType("int(11)")
                    .HasColumnName("ServiceID");

                entity.Property(e => e.ServiceTypeName)
                    .HasMaxLength(255)
                    .HasDefaultValueSql(null);

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

                entity.ToTable("Settings");

                entity.HasIndex(e => e.SettingName, "SettingName")
                    .IsUnique();

                entity.Property(e => e.SettingName).HasMaxLength(500);

                entity.Property(e => e.SettingValue)
                    .HasColumnType("varchar(5000)")
                    .HasDefaultValueSql(null);

                entity.HasData(
                    new Setting { SettingName = "SEND_SMS_APP", SettingValue = "1" },
                    new Setting { SettingName = "MIN_AFTER_WORK", SettingValue = "60" },
                    new Setting { SettingName = "SMS_TEMPLATE_APP", SettingValue = "שלום {FirstName} {LastName},\\nנקבעה לך פגישה לטיפול {ServiceType} אצל מיריתוש\\nבתאריך {Date} בשעה {Time}\\nיש להגיע עם מסכה\n" },
                    new Setting { SettingName = "SMS_TEMPLATE_REMINDER", SettingValue = "שלום {FirstName} {LastName},\\nזאת תזכורת לטיפול {ServiceType} אצל מיריתוש\\nבתאריך {Date} בשעה {Time}\\nלא לשכוח מסכה\\n\\nלאישור הגעה יש להודיע בהודעת ווצאפ\nלמספר 0525533979" },
                    new Setting { SettingName = "TIME_INTERVAL_CALENDAR", SettingValue = "60" },
                    new Setting { SettingName = "SMS_TEMPLATE_UPAPP", SettingValue = "שלום {FirstName} {LastName},\\nהפגישה לטיפול {ServiceType} אצל מיריתוש\\n עודכנה לתאריך {Date} בשעה {Time}\\nיש להגיע עם מסכה" });
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.RegId)
                    .HasMaxLength(1000)
                    .HasDefaultValueSql(null);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.HasData(new User { Id = 1, UserName = "admin", Password = "Hash" });
            });

            modelBuilder.Entity<Workhour>(entity =>
            {
                entity.HasKey(e => e.DayOfWeek)
                    .HasName("PRIMARY");

                entity.ToTable("WorkHours");

                entity.HasIndex(e => e.DayOfWeek, "DayOfWeek_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.DayOfWeek).HasColumnType("int(11)");

                entity.Property(e => e.CloseTime).HasColumnType("int(11)");

                entity.Property(e => e.OpenTime).HasColumnType("int(11)");

                entity.HasData(new Workhour { DayOfWeek = 1, OpenTime = 540, CloseTime = 1095 },
                new Workhour { DayOfWeek = 2, OpenTime = 540, CloseTime = 1095 },
                new Workhour { DayOfWeek = 3, OpenTime = 540, CloseTime = 1095 },
                new Workhour { DayOfWeek = 4, OpenTime = 540, CloseTime = 1095 },
                new Workhour { DayOfWeek = 5, OpenTime = 540, CloseTime = 930 },
                new Workhour { DayOfWeek = 6, OpenTime = 540, CloseTime = 840 });
            });

            modelBuilder.Entity<Attachment>(entity =>
            {
                entity.ToTable("Attachments");

                entity.HasIndex(e => e.CustomerId, "CustomerID_Attachments");

                entity.HasIndex(e => new { e.AttachmentId }, "AttachmentID")
                    .IsUnique();

                entity.Property(e => e.AttachmentId)
                    .HasColumnType("int(11)")
                    .HasColumnName("AttachmentID");

                entity.Property(e => e.CustomerId)
                    .HasColumnType("int(11)")
                    .HasColumnName("CustomerID")
                    .HasDefaultValueSql(null);

                entity.Property(e => e.AttachmentName)
                    .HasColumnType("varchar(400)")
                    .HasColumnName("AttachmentName");

                entity.Property(e => e.MimeType)
                    .HasColumnType("varchar(500)")
                    .HasColumnName("MimeType");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Attachments)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("CustomerID_Attachments");
            });

            modelBuilder.Entity<ProductCategory>(entity =>
            {
                entity.ToTable("ProductCategorys");

                entity.HasIndex(e => new { e.Id }, "CategoryID")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("CategoryID");

                entity.Property(e => e.Name)
                    .HasColumnType("varchar(5000)")
                    .HasColumnName("CategoryName");

            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Products");

                entity.HasIndex(e => e.CategoryID, "CategoryID_Products");

                entity.HasIndex(e => new { e.Id }, "ProductID")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("ProductID");

                entity.Property(e => e.CategoryID)
                    .HasColumnType("int(11)")
                    .HasColumnName("CategoryID")
                    .HasDefaultValueSql(null);

                entity.Property(e => e.Name)
                    .HasColumnType("varchar(5000)")
                    .HasColumnName("ProductName");

                entity.Property(e => e.Description)
                    .HasColumnType("varchar(5000)")
                    .HasColumnName("ProductDescription")
                    .HasDefaultValueSql(null);

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(18,2)")
                    .HasDefaultValueSql(null);

                entity.Property(e => e.Active)
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'1'");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryID)
                    .HasConstraintName("CategoryID_Products");

            });
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
