﻿// <auto-generated />
using System;
using ESMS.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ESMS.Migrations
{
    [DbContext(typeof(ESMSContext))]
    partial class ESMSContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ESMS.Data.Model.AspNetRoleClaims", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasMaxLength(450);

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("ESMS.Data.Model.AspNetRoles", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("ESMS.Data.Model.AspNetUserClaims", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasMaxLength(450);

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("ESMS.Data.Model.AspNetUserLogins", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasMaxLength(450);

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("ESMS.Data.Model.AspNetUserRoles", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("ESMS.Data.Model.AspNetUserTokens", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("ESMS.Data.Model.AspNetUsers", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Address2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("BirthDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("('0001-01-01T00:00:00.0000000')");

                    b.Property<int?>("City")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Country")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<int>("EmployeeStatus")
                        .HasColumnType("int");

                    b.Property<DateTime>("EmploymentDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("('0001-01-01T00:00:00.0000000')");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(100)")
                        .HasDefaultValueSql("(N'')")
                        .HasMaxLength(100);

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("IbanCode")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(32)")
                        .HasDefaultValueSql("(N'')")
                        .HasMaxLength(32);

                    b.Property<string>("JobTitle")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(128)")
                        .HasDefaultValueSql("(N'')")
                        .HasMaxLength(128);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(100)")
                        .HasDefaultValueSql("(N'')")
                        .HasMaxLength(100);

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PersonalNumber")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(12)")
                        .HasDefaultValueSql("(N'')")
                        .HasMaxLength(12);

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<int?>("PostCode")
                        .HasColumnType("int");

                    b.Property<float>("Salary")
                        .HasColumnName("salary")
                        .HasColumnType("real");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<byte[]>("UserProfile")
                        .HasColumnType("varbinary(max)");

                    b.HasKey("Id");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("ESMS.Data.Model.AspNetUsersHistory", b =>
                {
                    b.Property<int>("IdHistory")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Address2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("BirthDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("('0001-01-01T00:00:00.0000000')");

                    b.Property<int?>("City")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Country")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<int>("EmployeeStatus")
                        .HasColumnType("int");

                    b.Property<DateTime>("EmploymentDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("('0001-01-01T00:00:00.0000000')");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(100)")
                        .HasDefaultValueSql("(N'')")
                        .HasMaxLength(100);

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("IbanCode")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(32)")
                        .HasDefaultValueSql("(N'')")
                        .HasMaxLength(32);

                    b.Property<string>("Id")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasMaxLength(450);

                    b.Property<string>("JobTitle")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(128)")
                        .HasDefaultValueSql("(N'')")
                        .HasMaxLength(128);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(100)")
                        .HasDefaultValueSql("(N'')")
                        .HasMaxLength(100);

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PersonalNumber")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(12)")
                        .HasDefaultValueSql("(N'')")
                        .HasMaxLength(12);

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<int?>("PostCode")
                        .HasColumnType("int");

                    b.Property<float>("Salary")
                        .HasColumnName("salary")
                        .HasColumnType("real");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<byte[]>("UserProfile")
                        .HasColumnType("varbinary(max)");

                    b.HasKey("IdHistory");

                    b.ToTable("AspNetUsersHistory");
                });

            modelBuilder.Entity("ESMS.Data.Model.Cities", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ContryId")
                        .HasColumnName("contryId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnName("name")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("ESMS.Data.Model.Contries", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnName("ID")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("name")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<int>("PhoneCode")
                        .HasColumnName("phoneCode")
                        .HasColumnType("int");

                    b.Property<string>("SortName")
                        .IsRequired()
                        .HasColumnName("sortName")
                        .HasColumnType("nvarchar(3)")
                        .HasMaxLength(3);

                    b.HasKey("Id");

                    b.ToTable("Contries");
                });

            modelBuilder.Entity("ESMS.Data.Model.DocumentType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DtInserted")
                        .HasColumnName("dtInserted")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("DtModify")
                        .HasColumnName("dtModify")
                        .HasColumnType("datetime");

                    b.Property<string>("NInsertedId")
                        .IsRequired()
                        .HasColumnName("nInsertedID")
                        .HasColumnType("nvarchar(450)")
                        .HasMaxLength(450);

                    b.Property<string>("NModifyId")
                        .HasColumnName("nModifyID")
                        .HasColumnType("nvarchar(450)")
                        .HasMaxLength(450);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.HasKey("Id");

                    b.ToTable("DocumentType");
                });

            modelBuilder.Entity("ESMS.Data.Model.EmployeeDocuments", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DtInserted")
                        .HasColumnName("dtInserted")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("DtModify")
                        .HasColumnName("dtModify")
                        .HasColumnType("datetime");

                    b.Property<string>("Employee")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasMaxLength(450);

                    b.Property<string>("NInsertedId")
                        .IsRequired()
                        .HasColumnName("nInsertedID")
                        .HasColumnType("nvarchar(450)")
                        .HasMaxLength(450);

                    b.Property<string>("NModifyId")
                        .HasColumnName("nModifyID")
                        .HasColumnType("nvarchar(450)")
                        .HasMaxLength(450);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Employee");

                    b.HasIndex("Type");

                    b.ToTable("EmployeeDocuments");
                });

            modelBuilder.Entity("ESMS.Data.Model.Logs", b =>
                {
                    b.Property<long>("NLogId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("nLogID")
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("DtInserted")
                        .HasColumnName("dtInserted")
                        .HasColumnType("datetime");

                    b.Property<string>("Hostname")
                        .HasColumnName("hostname")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("IpAdress")
                        .HasColumnName("ipAdress")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Method")
                        .HasColumnName("method")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Page")
                        .HasColumnName("page")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<int?>("StatusCode")
                        .HasColumnType("int");

                    b.Property<string>("Url")
                        .HasColumnName("url")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnName("userId")
                        .HasColumnType("nvarchar(450)")
                        .HasMaxLength(450);

                    b.HasKey("NLogId");

                    b.ToTable("Logs");
                });

            modelBuilder.Entity("ESMS.Data.Model.Menu", b =>
                {
                    b.Property<int>("NMenuId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("nMenuID")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DtInserted")
                        .HasColumnName("dtInserted")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("DtModify")
                        .HasColumnName("dtModify")
                        .HasColumnType("datetime");

                    b.Property<string>("NInsertedId")
                        .IsRequired()
                        .HasColumnName("nInsertedID")
                        .HasColumnType("nvarchar(450)")
                        .HasMaxLength(450);

                    b.Property<string>("NModifyId")
                        .HasColumnName("nModifyID")
                        .HasColumnType("nvarchar(450)")
                        .HasMaxLength(450);

                    b.Property<string>("VcIcon")
                        .IsRequired()
                        .HasColumnName("vcIcon")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("VcMenNameSq")
                        .IsRequired()
                        .HasColumnName("vcMenName_SQ")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("VcMenuNameEn")
                        .IsRequired()
                        .HasColumnName("vcMenuName_EN")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.HasKey("NMenuId");

                    b.HasIndex("NInsertedId");

                    b.ToTable("Menu");
                });

            modelBuilder.Entity("ESMS.Data.Model.Notifications", b =>
                {
                    b.Property<int>("NNotificationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("nNotificationId")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DtInserted")
                        .HasColumnName("dtInserted")
                        .HasColumnType("datetime");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnName("title")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("VcIcon")
                        .IsRequired()
                        .HasColumnName("vcIcon")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("VcInsertedUser")
                        .IsRequired()
                        .HasColumnName("vcInsertedUser")
                        .HasColumnType("nvarchar(450)")
                        .HasMaxLength(450);

                    b.Property<string>("VcText")
                        .IsRequired()
                        .HasColumnName("vcText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VcUser")
                        .IsRequired()
                        .HasColumnName("vcUser")
                        .HasColumnType("nvarchar(450)")
                        .HasMaxLength(450);

                    b.HasKey("NNotificationId");

                    b.HasIndex("VcInsertedUser");

                    b.HasIndex("VcUser");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("ESMS.Data.Model.Policy", b =>
                {
                    b.Property<int>("NPolicyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("nPolicyID")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("BActive")
                        .HasColumnName("bActive")
                        .HasColumnType("bit");

                    b.Property<DateTime>("DtInserted")
                        .HasColumnName("dtInserted")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("DtModify")
                        .HasColumnName("dtModify")
                        .HasColumnType("datetime");

                    b.Property<string>("NInsertedId")
                        .IsRequired()
                        .HasColumnName("nInsertedID")
                        .HasColumnType("nvarchar(450)")
                        .HasMaxLength(450);

                    b.Property<string>("NModifyId")
                        .HasColumnName("nModifyID")
                        .HasColumnType("nvarchar(450)")
                        .HasMaxLength(450);

                    b.Property<string>("VcClaimType")
                        .IsRequired()
                        .HasColumnName("vcClaimType")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("VcClaimValue")
                        .HasColumnName("vcClaimValue")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("VcPolicyName")
                        .IsRequired()
                        .HasColumnName("vcPolicyName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("NPolicyId");

                    b.ToTable("Policy");
                });

            modelBuilder.Entity("ESMS.Data.Model.Position", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DtInserted")
                        .HasColumnName("dtInserted")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("DtModify")
                        .HasColumnName("dtModify")
                        .HasColumnType("datetime");

                    b.Property<string>("NInsertedId")
                        .IsRequired()
                        .HasColumnName("nInsertedID")
                        .HasColumnType("nvarchar(450)")
                        .HasMaxLength(450);

                    b.Property<string>("NModifyId")
                        .HasColumnName("nModifyID")
                        .HasColumnType("nvarchar(450)")
                        .HasMaxLength(450);

                    b.Property<string>("NameEn")
                        .HasColumnName("Name_EN")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("NameSq")
                        .HasColumnName("Name_SQ")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.HasKey("Id");

                    b.ToTable("Position");
                });

            modelBuilder.Entity("ESMS.Data.Model.States", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<int?>("ContryId")
                        .HasColumnName("contryID")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnName("name")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.ToTable("States");
                });

            modelBuilder.Entity("ESMS.Data.Model.SubMenu", b =>
                {
                    b.Property<int>("NSubMenuId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("nSubMenuID")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DtInserted")
                        .HasColumnName("dtInserted")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("DtModify")
                        .HasColumnName("dtModify")
                        .HasColumnType("datetime");

                    b.Property<string>("NInsertedId")
                        .IsRequired()
                        .HasColumnName("nInsertedID")
                        .HasColumnType("nvarchar(450)")
                        .HasMaxLength(450);

                    b.Property<int>("NMenuId")
                        .HasColumnName("nMenuID")
                        .HasColumnType("int");

                    b.Property<string>("VcClaim")
                        .HasColumnName("vcClaim")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("VcController")
                        .IsRequired()
                        .HasColumnName("vcController")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("VcPage")
                        .IsRequired()
                        .HasColumnName("vcPage")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("VcSubMenuEn")
                        .IsRequired()
                        .HasColumnName("vcSubMenu_EN")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("VcSubMenuSq")
                        .IsRequired()
                        .HasColumnName("vcSubMenu_SQ")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.HasKey("NSubMenuId")
                        .HasName("PK_SubMeny");

                    b.HasIndex("NInsertedId");

                    b.HasIndex("NMenuId");

                    b.ToTable("SubMenu");
                });

            modelBuilder.Entity("ESMS.Data.Model.AspNetRoleClaims", b =>
                {
                    b.HasOne("ESMS.Data.Model.AspNetRoles", "Role")
                        .WithMany("AspNetRoleClaims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ESMS.Data.Model.AspNetUserClaims", b =>
                {
                    b.HasOne("ESMS.Data.Model.AspNetUsers", "User")
                        .WithMany("AspNetUserClaims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ESMS.Data.Model.AspNetUserLogins", b =>
                {
                    b.HasOne("ESMS.Data.Model.AspNetUsers", "User")
                        .WithMany("AspNetUserLogins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ESMS.Data.Model.AspNetUserRoles", b =>
                {
                    b.HasOne("ESMS.Data.Model.AspNetRoles", "Role")
                        .WithMany("AspNetUserRoles")
                        .HasForeignKey("RoleId")
                        .HasConstraintName("FK_AspNetUserRoles_AspNetRoles")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ESMS.Data.Model.AspNetUsers", "User")
                        .WithMany("AspNetUserRoles")
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_AspNetUserRoles_AspNetUsers")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ESMS.Data.Model.AspNetUserTokens", b =>
                {
                    b.HasOne("ESMS.Data.Model.AspNetUsers", "User")
                        .WithMany("AspNetUserTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ESMS.Data.Model.EmployeeDocuments", b =>
                {
                    b.HasOne("ESMS.Data.Model.AspNetUsers", "EmployeeNavigation")
                        .WithMany("EmployeeDocuments")
                        .HasForeignKey("Employee")
                        .HasConstraintName("FK_EmployeeDocuments_AspNetUsers")
                        .IsRequired();

                    b.HasOne("ESMS.Data.Model.DocumentType", "TypeNavigation")
                        .WithMany("EmployeeDocuments")
                        .HasForeignKey("Type")
                        .HasConstraintName("FK_EmployeeDocuments_DocumentType")
                        .IsRequired();
                });

            modelBuilder.Entity("ESMS.Data.Model.Menu", b =>
                {
                    b.HasOne("ESMS.Data.Model.AspNetUsers", "NInserted")
                        .WithMany("Menu")
                        .HasForeignKey("NInsertedId")
                        .HasConstraintName("FK_Menu_AspNetUsers")
                        .IsRequired();
                });

            modelBuilder.Entity("ESMS.Data.Model.Notifications", b =>
                {
                    b.HasOne("ESMS.Data.Model.AspNetUsers", "VcInsertedUserNavigation")
                        .WithMany("NotificationsVcInsertedUserNavigation")
                        .HasForeignKey("VcInsertedUser")
                        .HasConstraintName("FK_Notifications_AspNetUsers1")
                        .IsRequired();

                    b.HasOne("ESMS.Data.Model.AspNetUsers", "VcUserNavigation")
                        .WithMany("NotificationsVcUserNavigation")
                        .HasForeignKey("VcUser")
                        .HasConstraintName("FK_Notifications_AspNetUsers")
                        .IsRequired();
                });

            modelBuilder.Entity("ESMS.Data.Model.SubMenu", b =>
                {
                    b.HasOne("ESMS.Data.Model.AspNetUsers", "NInserted")
                        .WithMany("SubMenu")
                        .HasForeignKey("NInsertedId")
                        .HasConstraintName("FK_SubMenu_AspNetUsers")
                        .IsRequired();

                    b.HasOne("ESMS.Data.Model.Menu", "NMenu")
                        .WithMany("SubMenu")
                        .HasForeignKey("NMenuId")
                        .HasConstraintName("FK_SubMeny_Menu")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}