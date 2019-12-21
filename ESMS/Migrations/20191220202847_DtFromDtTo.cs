using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ESMS.Migrations
{
    public partial class DtFromDtTo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    BirthDate = table.Column<DateTime>(nullable: false, defaultValueSql: "('0001-01-01T00:00:00.0000000')"),
                    FirstName = table.Column<string>(maxLength: 100, nullable: false, defaultValueSql: "(N'')"),
                    LastName = table.Column<string>(maxLength: 100, nullable: false, defaultValueSql: "(N'')"),
                    Address = table.Column<string>(nullable: true),
                    Address2 = table.Column<string>(nullable: true),
                    City = table.Column<int>(nullable: true),
                    Country = table.Column<int>(nullable: true),
                    EmployeeStatus = table.Column<int>(nullable: false),
                    EmploymentDate = table.Column<DateTime>(nullable: false, defaultValueSql: "('0001-01-01T00:00:00.0000000')"),
                    Gender = table.Column<int>(nullable: false),
                    JobTitle = table.Column<string>(maxLength: 128, nullable: false, defaultValueSql: "(N'')"),
                    PostCode = table.Column<int>(nullable: true),
                    IbanCode = table.Column<string>(maxLength: 32, nullable: false, defaultValueSql: "(N'')"),
                    PersonalNumber = table.Column<string>(maxLength: 12, nullable: false, defaultValueSql: "(N'')"),
                    salary = table.Column<float>(nullable: false),
                    UserProfile = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsersHistory",
                columns: table => new
                {
                    IdHistory = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    BirthDate = table.Column<DateTime>(nullable: false, defaultValueSql: "('0001-01-01T00:00:00.0000000')"),
                    FirstName = table.Column<string>(maxLength: 100, nullable: false, defaultValueSql: "(N'')"),
                    LastName = table.Column<string>(maxLength: 100, nullable: false, defaultValueSql: "(N'')"),
                    Address = table.Column<string>(nullable: true),
                    Address2 = table.Column<string>(nullable: true),
                    City = table.Column<int>(nullable: true),
                    Country = table.Column<int>(nullable: true),
                    EmployeeStatus = table.Column<int>(nullable: false),
                    EmploymentDate = table.Column<DateTime>(nullable: false, defaultValueSql: "('0001-01-01T00:00:00.0000000')"),
                    Gender = table.Column<int>(nullable: false),
                    JobTitle = table.Column<string>(maxLength: 128, nullable: false, defaultValueSql: "(N'')"),
                    PostCode = table.Column<int>(nullable: true),
                    IbanCode = table.Column<string>(maxLength: 32, nullable: false, defaultValueSql: "(N'')"),
                    PersonalNumber = table.Column<string>(maxLength: 12, nullable: false, defaultValueSql: "(N'')"),
                    salary = table.Column<float>(nullable: false),
                    UserProfile = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsersHistory", x => x.IdHistory);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(maxLength: 256, nullable: true),
                    contryId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Contries",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false),
                    sortName = table.Column<string>(maxLength: 3, nullable: false),
                    name = table.Column<string>(maxLength: 256, nullable: false),
                    phoneCode = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contries", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "DocumentType",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 128, nullable: true),
                    nInsertedID = table.Column<string>(maxLength: 450, nullable: false),
                    dtInserted = table.Column<DateTime>(type: "datetime", nullable: false),
                    nModifyID = table.Column<string>(maxLength: 450, nullable: true),
                    dtModify = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentType", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    nLogID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ipAdress = table.Column<string>(maxLength: 50, nullable: true),
                    hostname = table.Column<string>(maxLength: 50, nullable: true),
                    userId = table.Column<string>(maxLength: 450, nullable: true),
                    dtInserted = table.Column<DateTime>(type: "datetime", nullable: true),
                    page = table.Column<string>(maxLength: 128, nullable: true),
                    method = table.Column<string>(maxLength: 128, nullable: true),
                    url = table.Column<string>(nullable: true),
                    StatusCode = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.nLogID);
                });

            migrationBuilder.CreateTable(
                name: "Policy",
                columns: table => new
                {
                    nPolicyID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    vcPolicyName = table.Column<string>(maxLength: 256, nullable: false),
                    vcClaimType = table.Column<string>(maxLength: 256, nullable: false),
                    vcClaimValue = table.Column<string>(maxLength: 256, nullable: true),
                    bActive = table.Column<bool>(nullable: false),
                    nInsertedID = table.Column<string>(maxLength: 450, nullable: false),
                    dtInserted = table.Column<DateTime>(type: "datetime", nullable: false),
                    nModifyID = table.Column<string>(maxLength: 450, nullable: true),
                    dtModify = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Policy", x => x.nPolicyID);
                });

            migrationBuilder.CreateTable(
                name: "Position",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name_SQ = table.Column<string>(maxLength: 128, nullable: true),
                    Name_EN = table.Column<string>(maxLength: 128, nullable: true),
                    nInsertedID = table.Column<string>(maxLength: 450, nullable: false),
                    dtInserted = table.Column<DateTime>(type: "datetime", nullable: false),
                    nModifyID = table.Column<string>(maxLength: 450, nullable: true),
                    dtModify = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Position", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "States",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    name = table.Column<string>(maxLength: 256, nullable: true),
                    contryID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_States", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(maxLength: 450, nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(maxLength: 450, nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(maxLength: 450, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Menu",
                columns: table => new
                {
                    nMenuID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    vcMenName_SQ = table.Column<string>(maxLength: 128, nullable: false),
                    vcMenuName_EN = table.Column<string>(maxLength: 128, nullable: false),
                    vcIcon = table.Column<string>(maxLength: 50, nullable: false),
                    nInsertedID = table.Column<string>(maxLength: 450, nullable: false),
                    dtInserted = table.Column<DateTime>(type: "datetime", nullable: false),
                    nModifyID = table.Column<string>(maxLength: 450, nullable: true),
                    dtModify = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menu", x => x.nMenuID);
                    table.ForeignKey(
                        name: "FK_Menu_AspNetUsers",
                        column: x => x.nInsertedID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    nNotificationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(maxLength: 100, nullable: false),
                    vcText = table.Column<string>(nullable: false),
                    vcIcon = table.Column<string>(maxLength: 100, nullable: false),
                    vcUser = table.Column<string>(maxLength: 450, nullable: false),
                    dtInserted = table.Column<DateTime>(type: "datetime", nullable: false),
                    vcInsertedUser = table.Column<string>(maxLength: 450, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.nNotificationId);
                    table.ForeignKey(
                        name: "FK_Notifications_AspNetUsers1",
                        column: x => x.vcInsertedUser,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notifications_AspNetUsers",
                        column: x => x.vcUser,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeDocuments",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Employee = table.Column<string>(maxLength: 450, nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Path = table.Column<string>(nullable: false),
                    nInsertedID = table.Column<string>(maxLength: 450, nullable: false),
                    dtInserted = table.Column<DateTime>(type: "datetime", nullable: false),
                    nModifyID = table.Column<string>(maxLength: 450, nullable: true),
                    dtModify = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeDocuments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_EmployeeDocuments_AspNetUsers",
                        column: x => x.Employee,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeDocuments_DocumentType",
                        column: x => x.Type,
                        principalTable: "DocumentType",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubMenu",
                columns: table => new
                {
                    nSubMenuID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nMenuID = table.Column<int>(nullable: false),
                    vcSubMenu_SQ = table.Column<string>(maxLength: 128, nullable: false),
                    vcSubMenu_EN = table.Column<string>(maxLength: 128, nullable: false),
                    vcController = table.Column<string>(maxLength: 128, nullable: false),
                    vcPage = table.Column<string>(maxLength: 128, nullable: false),
                    vcClaim = table.Column<string>(maxLength: 256, nullable: true),
                    nInsertedID = table.Column<string>(maxLength: 450, nullable: false),
                    dtInserted = table.Column<DateTime>(type: "datetime", nullable: false),
                    dtModify = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubMeny", x => x.nSubMenuID);
                    table.ForeignKey(
                        name: "FK_SubMenu_AspNetUsers",
                        column: x => x.nInsertedID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubMeny_Menu",
                        column: x => x.nMenuID,
                        principalTable: "Menu",
                        principalColumn: "nMenuID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDocuments_Employee",
                table: "EmployeeDocuments",
                column: "Employee");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDocuments_Type",
                table: "EmployeeDocuments",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_Menu_nInsertedID",
                table: "Menu",
                column: "nInsertedID");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_vcInsertedUser",
                table: "Notifications",
                column: "vcInsertedUser");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_vcUser",
                table: "Notifications",
                column: "vcUser");

            migrationBuilder.CreateIndex(
                name: "IX_SubMenu_nInsertedID",
                table: "SubMenu",
                column: "nInsertedID");

            migrationBuilder.CreateIndex(
                name: "IX_SubMenu_nMenuID",
                table: "SubMenu",
                column: "nMenuID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsersHistory");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "Contries");

            migrationBuilder.DropTable(
                name: "EmployeeDocuments");

            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Policy");

            migrationBuilder.DropTable(
                name: "Position");

            migrationBuilder.DropTable(
                name: "States");

            migrationBuilder.DropTable(
                name: "SubMenu");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "DocumentType");

            migrationBuilder.DropTable(
                name: "Menu");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
