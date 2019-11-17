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
        public virtual DbSet<GroupMenu> GroupMenu { get; set; }
        public virtual DbSet<Logs> Logs { get; set; }
        public virtual DbSet<Menu> Menu { get; set; }
        public virtual DbSet<Policy> Policy { get; set; }
        public virtual DbSet<SubMenu> SubMenu { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.\\sqlsrv;Database=ESMS;user id=superman; password = Esms2019.;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.RoleId).IsRequired();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
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
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.BirthDate).HasDefaultValueSql("('0001-01-01T00:00:00.0000000')");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasDefaultValueSql("(N'')");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasDefaultValueSql("(N'')");

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<GroupMenu>(entity =>
            {
                entity.HasKey(e => e.NGroupMenyId);

                entity.Property(e => e.NGroupMenyId).HasColumnName("nGroupMenyID");

                entity.Property(e => e.BAccess).HasColumnName("bAccess");

                entity.Property(e => e.DtInserted)
                    .HasColumnName("dtInserted")
                    .HasColumnType("datetime");

                entity.Property(e => e.DtModify)
                    .HasColumnName("dtModify")
                    .HasColumnType("datetime");

                entity.Property(e => e.NInsertedId).HasColumnName("nInsertedID");

                entity.Property(e => e.NModifyId).HasColumnName("nModifyID");

                entity.Property(e => e.NSubMenuId).HasColumnName("nSubMenuID");

                entity.Property(e => e.VcAspNetRolesId)
                    .IsRequired()
                    .HasColumnName("vcAspNetRolesID")
                    .HasMaxLength(450);

                entity.HasOne(d => d.NSubMenu)
                    .WithMany(p => p.GroupMenu)
                    .HasForeignKey(d => d.NSubMenuId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GroupMenu_SubMenu");

                entity.HasOne(d => d.VcAspNetRoles)
                    .WithMany(p => p.GroupMenu)
                    .HasForeignKey(d => d.VcAspNetRolesId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GroupMenu_AspNetRoles");
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

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
