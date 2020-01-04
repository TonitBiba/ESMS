using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ESMS.Data.Model
{
    public partial class ESMSContext : DbContext
    {
        public ESMSContext()
        {
        }

        public ESMSContext(DbContextOptions<ESMSContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUsersHistory> AspNetUsersHistory { get; set; }
        public virtual DbSet<Cities> Cities { get; set; }
        public virtual DbSet<Contries> Contries { get; set; }
        public virtual DbSet<DocumentType> DocumentType { get; set; }
        public virtual DbSet<EmployeeDocuments> EmployeeDocuments { get; set; }
        public virtual DbSet<Leaves> Leaves { get; set; }
        public virtual DbSet<LeavesDetails> LeavesDetails { get; set; }
        public virtual DbSet<Logs> Logs { get; set; }
        public virtual DbSet<Menu> Menu { get; set; }
        public virtual DbSet<Month> Month { get; set; }
        public virtual DbSet<Notifications> Notifications { get; set; }
        public virtual DbSet<Payments> Payments { get; set; }
        public virtual DbSet<Policy> Policy { get; set; }
        public virtual DbSet<Position> Position { get; set; }
        public virtual DbSet<States> States { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<SubMenu> SubMenu { get; set; }
        public virtual DbSet<TaxGroup> TaxGroup { get; set; }
        public virtual DbSet<TypeOfLeaves> TypeOfLeaves { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.\\sqlsrv2019;Database=ESMS;user id=superman; password = Esms2019.;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.Property(e => e.RoleId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_AspNetUserRoles_AspNetRoles");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_AspNetUserRoles_AspNetUsers");
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.Property(e => e.BirthDate).HasDefaultValueSql("('0001-01-01T00:00:00.0000000')");

                entity.Property(e => e.DtFrom).HasDefaultValueSql("('0001-01-01T00:00:00.0000000')");

                entity.Property(e => e.DtTo).HasDefaultValueSql("('0001-01-01T00:00:00.0000000')");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .HasDefaultValueSql("(N'')");

                entity.Property(e => e.IbanCode)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasDefaultValueSql("(N'')");

                entity.Property(e => e.JobTitle)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasDefaultValueSql("(N'')");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasDefaultValueSql("(N'')");

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.PersonalNumber)
                    .IsRequired()
                    .HasMaxLength(12)
                    .HasDefaultValueSql("(N'')");

                entity.Property(e => e.Salary).HasColumnName("salary");

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUsersHistory>(entity =>
            {
                entity.HasKey(e => e.IdHistory);

                entity.Property(e => e.BirthDate).HasDefaultValueSql("('0001-01-01T00:00:00.0000000')");

                entity.Property(e => e.DtFrom).HasDefaultValueSql("('0001-01-01T00:00:00.0000000')");

                entity.Property(e => e.DtTo).HasDefaultValueSql("('0001-01-01T00:00:00.0000000')");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .HasDefaultValueSql("(N'')");

                entity.Property(e => e.IbanCode)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasDefaultValueSql("(N'')");

                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.JobTitle)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasDefaultValueSql("(N'')");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasDefaultValueSql("(N'')");

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.PersonalNumber)
                    .IsRequired()
                    .HasMaxLength(12)
                    .HasDefaultValueSql("(N'')");

                entity.Property(e => e.Salary).HasColumnName("salary");

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<Cities>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ContryId).HasColumnName("contryId");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<Contries>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(256);

                entity.Property(e => e.PhoneCode).HasColumnName("phoneCode");

                entity.Property(e => e.SortName)
                    .IsRequired()
                    .HasColumnName("sortName")
                    .HasMaxLength(3);
            });

            modelBuilder.Entity<DocumentType>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DtInserted)
                    .HasColumnName("dtInserted")
                    .HasColumnType("datetime");

                entity.Property(e => e.DtModify)
                    .HasColumnName("dtModify")
                    .HasColumnType("datetime");

                entity.Property(e => e.NInsertedId)
                    .IsRequired()
                    .HasColumnName("nInsertedID")
                    .HasMaxLength(450);

                entity.Property(e => e.NModifyId)
                    .HasColumnName("nModifyID")
                    .HasMaxLength(450);

                entity.Property(e => e.Name).HasMaxLength(128);
            });

            modelBuilder.Entity<EmployeeDocuments>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DtInserted)
                    .HasColumnName("dtInserted")
                    .HasColumnType("datetime");

                entity.Property(e => e.DtModify)
                    .HasColumnName("dtModify")
                    .HasColumnType("datetime");

                entity.Property(e => e.Employee)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.NInsertedId)
                    .IsRequired()
                    .HasColumnName("nInsertedID")
                    .HasMaxLength(450);

                entity.Property(e => e.NModifyId)
                    .HasColumnName("nModifyID")
                    .HasMaxLength(450);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Path).IsRequired();

                entity.HasOne(d => d.EmployeeNavigation)
                    .WithMany(p => p.EmployeeDocuments)
                    .HasForeignKey(d => d.Employee)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EmployeeDocuments_AspNetUsers");

                entity.HasOne(d => d.TypeNavigation)
                    .WithMany(p => p.EmployeeDocuments)
                    .HasForeignKey(d => d.Type)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EmployeeDocuments_DocumentType");
            });

            modelBuilder.Entity<Leaves>(entity =>
            {
                entity.Property(e => e.DtInserted)
                    .HasColumnName("dtInserted")
                    .HasColumnType("datetime");

                entity.Property(e => e.EndDate)
                    .HasColumnName("endDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.NTypeOfLeaves).HasColumnName("nTypeOfLeaves");

                entity.Property(e => e.StartDate)
                    .HasColumnName("startDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.VcComment).HasColumnName("vcComment");

                entity.Property(e => e.VcDocumentPath)
                    .HasColumnName("vcDocumentPath")
                    .HasMaxLength(256);

                entity.Property(e => e.VcUser)
                    .IsRequired()
                    .HasColumnName("vcUser")
                    .HasMaxLength(450);

                entity.HasOne(d => d.NTypeOfLeavesNavigation)
                    .WithMany(p => p.Leaves)
                    .HasForeignKey(d => d.NTypeOfLeaves)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Leaves_TypeOfLeaves");

                entity.HasOne(d => d.VcUserNavigation)
                    .WithMany(p => p.Leaves)
                    .HasForeignKey(d => d.VcUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Leaves_AspNetUsers");
            });

            modelBuilder.Entity<LeavesDetails>(entity =>
            {
                entity.Property(e => e.BActive).HasColumnName("bActive");

                entity.Property(e => e.DtInserted)
                    .HasColumnName("dtInserted")
                    .HasColumnType("datetime");

                entity.Property(e => e.NLeaves).HasColumnName("nLeaves");

                entity.Property(e => e.NStatus).HasColumnName("nStatus");

                entity.Property(e => e.VcComment).HasColumnName("vcComment");

                entity.Property(e => e.VcUser)
                    .IsRequired()
                    .HasColumnName("vcUser")
                    .HasMaxLength(450);

                entity.HasOne(d => d.NLeavesNavigation)
                    .WithMany(p => p.LeavesDetails)
                    .HasForeignKey(d => d.NLeaves)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LeavesDetails_Leaves");

                entity.HasOne(d => d.NStatusNavigation)
                    .WithMany(p => p.LeavesDetails)
                    .HasForeignKey(d => d.NStatus)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LeavesDetails_Status");

                entity.HasOne(d => d.VcUserNavigation)
                    .WithMany(p => p.LeavesDetails)
                    .HasForeignKey(d => d.VcUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LeavesDetails_AspNetUsers");
            });

            modelBuilder.Entity<Logs>(entity =>
            {
                entity.HasKey(e => e.NLogId);

                entity.Property(e => e.NLogId).HasColumnName("nLogID");

                entity.Property(e => e.DtInserted)
                    .HasColumnName("dtInserted")
                    .HasColumnType("datetime");

                entity.Property(e => e.Hostname)
                    .HasColumnName("hostname")
                    .HasMaxLength(50);

                entity.Property(e => e.IpAdress)
                    .HasColumnName("ipAdress")
                    .HasMaxLength(50);

                entity.Property(e => e.Method)
                    .HasColumnName("method")
                    .HasMaxLength(128);

                entity.Property(e => e.Page)
                    .HasColumnName("page")
                    .HasMaxLength(128);

                entity.Property(e => e.Url).HasColumnName("url");

                entity.Property(e => e.UserId)
                    .HasColumnName("userId")
                    .HasMaxLength(450);
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.HasKey(e => e.NMenuId);

                entity.Property(e => e.NMenuId).HasColumnName("nMenuID");

                entity.Property(e => e.DtInserted)
                    .HasColumnName("dtInserted")
                    .HasColumnType("datetime");

                entity.Property(e => e.DtModify)
                    .HasColumnName("dtModify")
                    .HasColumnType("datetime");

                entity.Property(e => e.NInsertedId)
                    .IsRequired()
                    .HasColumnName("nInsertedID")
                    .HasMaxLength(450);

                entity.Property(e => e.NModifyId)
                    .HasColumnName("nModifyID")
                    .HasMaxLength(450);

                entity.Property(e => e.VcIcon)
                    .IsRequired()
                    .HasColumnName("vcIcon")
                    .HasMaxLength(50);

                entity.Property(e => e.VcMenNameSq)
                    .IsRequired()
                    .HasColumnName("vcMenName_SQ")
                    .HasMaxLength(128);

                entity.Property(e => e.VcMenuNameEn)
                    .IsRequired()
                    .HasColumnName("vcMenuName_EN")
                    .HasMaxLength(128);

                entity.HasOne(d => d.NInserted)
                    .WithMany(p => p.Menu)
                    .HasForeignKey(d => d.NInsertedId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Menu_AspNetUsers");
            });

            modelBuilder.Entity<Month>(entity =>
            {
                entity.Property(e => e.MonthEn)
                    .IsRequired()
                    .HasColumnName("month_EN")
                    .HasMaxLength(64);

                entity.Property(e => e.MonthSq)
                    .IsRequired()
                    .HasColumnName("month_SQ")
                    .HasMaxLength(64);
            });

            modelBuilder.Entity<Notifications>(entity =>
            {
                entity.HasKey(e => e.NNotificationId);

                entity.Property(e => e.NNotificationId).HasColumnName("nNotificationId");

                entity.Property(e => e.DtInserted)
                    .HasColumnName("dtInserted")
                    .HasColumnType("datetime");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasMaxLength(100);

                entity.Property(e => e.VcIcon)
                    .IsRequired()
                    .HasColumnName("vcIcon")
                    .HasMaxLength(100);

                entity.Property(e => e.VcInsertedUser)
                    .IsRequired()
                    .HasColumnName("vcInsertedUser")
                    .HasMaxLength(450);

                entity.Property(e => e.VcText)
                    .IsRequired()
                    .HasColumnName("vcText");

                entity.Property(e => e.VcUser)
                    .IsRequired()
                    .HasColumnName("vcUser")
                    .HasMaxLength(450);

                entity.HasOne(d => d.VcInsertedUserNavigation)
                    .WithMany(p => p.NotificationsVcInsertedUserNavigation)
                    .HasForeignKey(d => d.VcInsertedUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Notifications_AspNetUsers1");

                entity.HasOne(d => d.VcUserNavigation)
                    .WithMany(p => p.NotificationsVcUserNavigation)
                    .HasForeignKey(d => d.VcUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Notifications_AspNetUsers");
            });

            modelBuilder.Entity<Payments>(entity =>
            {
                entity.Property(e => e.Deduction).HasColumnType("smallmoney");

                entity.Property(e => e.DtInserted)
                    .HasColumnName("dtInserted")
                    .HasColumnType("datetime");

                entity.Property(e => e.EmployeePension).HasColumnType("smallmoney");

                entity.Property(e => e.EmployerPension).HasColumnType("smallmoney");

                entity.Property(e => e.Month).HasColumnName("month");

                entity.Property(e => e.NetWage).HasColumnType("smallmoney");

                entity.Property(e => e.Salary)
                    .HasColumnName("salary")
                    .HasColumnType("money");

                entity.Property(e => e.SalaryAfterDeduction).HasColumnType("smallmoney");

                entity.Property(e => e.TaxableIncome).HasColumnType("smallmoney");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("userId")
                    .HasMaxLength(450);

                entity.Property(e => e.VcInserted)
                    .IsRequired()
                    .HasColumnName("vcInserted")
                    .HasMaxLength(450);

                entity.Property(e => e.WithholdingTax).HasColumnType("smallmoney");

                entity.HasOne(d => d.MonthNavigation)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.Month)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Payments_Month");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PaymentsUser)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Payments_AspNetUsers");

                entity.HasOne(d => d.VcInsertedNavigation)
                    .WithMany(p => p.PaymentsVcInsertedNavigation)
                    .HasForeignKey(d => d.VcInserted)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Payments_AspNetUsers1");
            });

            modelBuilder.Entity<Policy>(entity =>
            {
                entity.HasKey(e => e.NPolicyId);

                entity.Property(e => e.NPolicyId).HasColumnName("nPolicyID");

                entity.Property(e => e.BActive).HasColumnName("bActive");

                entity.Property(e => e.DtInserted)
                    .HasColumnName("dtInserted")
                    .HasColumnType("datetime");

                entity.Property(e => e.DtModify)
                    .HasColumnName("dtModify")
                    .HasColumnType("datetime");

                entity.Property(e => e.NInsertedId)
                    .IsRequired()
                    .HasColumnName("nInsertedID")
                    .HasMaxLength(450);

                entity.Property(e => e.NModifyId)
                    .HasColumnName("nModifyID")
                    .HasMaxLength(450);

                entity.Property(e => e.VcClaimType)
                    .IsRequired()
                    .HasColumnName("vcClaimType")
                    .HasMaxLength(256);

                entity.Property(e => e.VcClaimValue)
                    .HasColumnName("vcClaimValue")
                    .HasMaxLength(256);

                entity.Property(e => e.VcPolicyName)
                    .IsRequired()
                    .HasColumnName("vcPolicyName")
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<Position>(entity =>
            {
                entity.Property(e => e.DtInserted)
                    .HasColumnName("dtInserted")
                    .HasColumnType("datetime");

                entity.Property(e => e.DtModify)
                    .HasColumnName("dtModify")
                    .HasColumnType("datetime");

                entity.Property(e => e.NInsertedId)
                    .IsRequired()
                    .HasColumnName("nInsertedID")
                    .HasMaxLength(450);

                entity.Property(e => e.NModifyId)
                    .HasColumnName("nModifyID")
                    .HasMaxLength(450);

                entity.Property(e => e.NameEn)
                    .HasColumnName("Name_EN")
                    .HasMaxLength(128);

                entity.Property(e => e.NameSq)
                    .HasColumnName("Name_SQ")
                    .HasMaxLength(128);
            });

            modelBuilder.Entity<States>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ContryId).HasColumnName("contryID");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.Property(e => e.NameEn)
                    .IsRequired()
                    .HasColumnName("Name_EN")
                    .HasMaxLength(64);

                entity.Property(e => e.NameSq)
                    .IsRequired()
                    .HasColumnName("Name_SQ")
                    .HasMaxLength(64);
            });

            modelBuilder.Entity<SubMenu>(entity =>
            {
                entity.HasKey(e => e.NSubMenuId)
                    .HasName("PK_SubMeny");

                entity.Property(e => e.NSubMenuId).HasColumnName("nSubMenuID");

                entity.Property(e => e.DtInserted)
                    .HasColumnName("dtInserted")
                    .HasColumnType("datetime");

                entity.Property(e => e.DtModify)
                    .HasColumnName("dtModify")
                    .HasColumnType("datetime");

                entity.Property(e => e.NInsertedId)
                    .IsRequired()
                    .HasColumnName("nInsertedID")
                    .HasMaxLength(450);

                entity.Property(e => e.NMenuId).HasColumnName("nMenuID");

                entity.Property(e => e.VcClaim)
                    .HasColumnName("vcClaim")
                    .HasMaxLength(256);

                entity.Property(e => e.VcController)
                    .IsRequired()
                    .HasColumnName("vcController")
                    .HasMaxLength(128);

                entity.Property(e => e.VcPage)
                    .IsRequired()
                    .HasColumnName("vcPage")
                    .HasMaxLength(128);

                entity.Property(e => e.VcSubMenuEn)
                    .IsRequired()
                    .HasColumnName("vcSubMenu_EN")
                    .HasMaxLength(128);

                entity.Property(e => e.VcSubMenuSq)
                    .IsRequired()
                    .HasColumnName("vcSubMenu_SQ")
                    .HasMaxLength(128);

                entity.HasOne(d => d.NInserted)
                    .WithMany(p => p.SubMenu)
                    .HasForeignKey(d => d.NInsertedId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SubMenu_AspNetUsers");

                entity.HasOne(d => d.NMenu)
                    .WithMany(p => p.SubMenu)
                    .HasForeignKey(d => d.NMenuId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SubMeny_Menu");
            });

            modelBuilder.Entity<TaxGroup>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.NameEng)
                    .HasMaxLength(50)
                    .IsFixedLength();
            });

            modelBuilder.Entity<TypeOfLeaves>(entity =>
            {
                entity.Property(e => e.NameEn)
                    .IsRequired()
                    .HasColumnName("Name_EN")
                    .HasMaxLength(64);

                entity.Property(e => e.NameSq)
                    .IsRequired()
                    .HasColumnName("Name_SQ")
                    .HasMaxLength(64);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
