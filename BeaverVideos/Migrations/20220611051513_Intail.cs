using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeaverVideos.Migrations
{
    public partial class Intail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActionLogs",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    ModuleName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    ActionName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    ITCode = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    ActionUrl = table.Column<string>(type: "TEXT", maxLength: 250, nullable: true),
                    ActionTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Duration = table.Column<double>(type: "REAL", nullable: false),
                    Remark = table.Column<string>(type: "TEXT", nullable: true),
                    IP = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    LogType = table.Column<int>(type: "INTEGER", nullable: false),
                    TenantCode = table.Column<string>(type: "TEXT", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreateBy = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdateBy = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionLogs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "DataPrivileges",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserCode = table.Column<string>(type: "TEXT", nullable: true),
                    GroupCode = table.Column<string>(type: "TEXT", nullable: true),
                    TableName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    RelateId = table.Column<string>(type: "TEXT", nullable: true),
                    Domain = table.Column<string>(type: "TEXT", nullable: true),
                    TenantCode = table.Column<string>(type: "TEXT", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreateBy = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdateBy = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataPrivileges", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "FileAttachments",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    FileName = table.Column<string>(type: "TEXT", nullable: false),
                    FileExt = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    Path = table.Column<string>(type: "TEXT", nullable: true),
                    Length = table.Column<long>(type: "INTEGER", nullable: false),
                    UploadTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SaveMode = table.Column<string>(type: "TEXT", nullable: true),
                    FileData = table.Column<byte[]>(type: "BLOB", nullable: true),
                    ExtraInfo = table.Column<string>(type: "TEXT", nullable: true),
                    HandlerInfo = table.Column<string>(type: "TEXT", nullable: true),
                    TenantCode = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileAttachments", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "FrameworkGroups",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    GroupCode = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    GroupName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    GroupRemark = table.Column<string>(type: "TEXT", nullable: true),
                    Manager = table.Column<string>(type: "TEXT", nullable: true),
                    TenantCode = table.Column<string>(type: "TEXT", nullable: true),
                    ParentId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FrameworkGroups", x => x.ID);
                    table.ForeignKey(
                        name: "FK_FrameworkGroups_FrameworkGroups_ParentId",
                        column: x => x.ParentId,
                        principalTable: "FrameworkGroups",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "FrameworkMenus",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    PageName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    ActionName = table.Column<string>(type: "TEXT", nullable: true),
                    ModuleName = table.Column<string>(type: "TEXT", nullable: true),
                    FolderOnly = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsInherit = table.Column<bool>(type: "INTEGER", nullable: false),
                    ClassName = table.Column<string>(type: "TEXT", nullable: true),
                    MethodName = table.Column<string>(type: "TEXT", nullable: true),
                    Domain = table.Column<string>(type: "TEXT", nullable: true),
                    ShowOnMenu = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsPublic = table.Column<bool>(type: "INTEGER", nullable: false),
                    DisplayOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    IsInside = table.Column<bool>(type: "INTEGER", nullable: false),
                    TenantAllowed = table.Column<bool>(type: "INTEGER", nullable: true),
                    Url = table.Column<string>(type: "TEXT", nullable: true),
                    Icon = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    ParentId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FrameworkMenus", x => x.ID);
                    table.ForeignKey(
                        name: "FK_FrameworkMenus_FrameworkMenus_ParentId",
                        column: x => x.ParentId,
                        principalTable: "FrameworkMenus",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "FrameworkRoles",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    RoleCode = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    RoleName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    RoleRemark = table.Column<string>(type: "TEXT", nullable: true),
                    TenantCode = table.Column<string>(type: "TEXT", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreateBy = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdateBy = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FrameworkRoles", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "FrameworkTenants",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    TCode = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    TName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    TDb = table.Column<string>(type: "TEXT", nullable: true),
                    TDbType = table.Column<int>(type: "INTEGER", nullable: true),
                    DbContext = table.Column<string>(type: "TEXT", nullable: true),
                    TDomain = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    TenantCode = table.Column<string>(type: "TEXT", nullable: true),
                    EnableSub = table.Column<bool>(type: "INTEGER", nullable: false),
                    Enabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreateBy = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdateBy = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FrameworkTenants", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "FrameworkUserGroups",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserCode = table.Column<string>(type: "TEXT", nullable: false),
                    GroupCode = table.Column<string>(type: "TEXT", nullable: false),
                    TenantCode = table.Column<string>(type: "TEXT", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreateBy = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdateBy = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FrameworkUserGroups", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "FrameworkUserRoles",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserCode = table.Column<string>(type: "TEXT", nullable: false),
                    RoleCode = table.Column<string>(type: "TEXT", nullable: false),
                    TenantCode = table.Column<string>(type: "TEXT", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreateBy = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdateBy = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FrameworkUserRoles", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "FunctionPrivileges",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    RoleCode = table.Column<string>(type: "TEXT", nullable: true),
                    MenuItemId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Allowed = table.Column<bool>(type: "INTEGER", nullable: false),
                    TenantCode = table.Column<string>(type: "TEXT", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreateBy = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdateBy = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FunctionPrivileges", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "FrameworkUsers",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Gender = table.Column<int>(type: "INTEGER", nullable: true),
                    CellPhone = table.Column<string>(type: "TEXT", nullable: true),
                    HomePhone = table.Column<string>(type: "TEXT", maxLength: 30, nullable: true),
                    Address = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    ZipCode = table.Column<string>(type: "TEXT", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreateBy = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdateBy = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    ITCode = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    IsValid = table.Column<bool>(type: "INTEGER", nullable: false),
                    PhotoId = table.Column<Guid>(type: "TEXT", nullable: true),
                    TenantCode = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FrameworkUsers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_FrameworkUsers_FileAttachments_PhotoId",
                        column: x => x.PhotoId,
                        principalTable: "FileAttachments",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FrameworkGroups_ParentId",
                table: "FrameworkGroups",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_FrameworkMenus_ParentId",
                table: "FrameworkMenus",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_FrameworkUsers_PhotoId",
                table: "FrameworkUsers",
                column: "PhotoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActionLogs");

            migrationBuilder.DropTable(
                name: "DataPrivileges");

            migrationBuilder.DropTable(
                name: "FrameworkGroups");

            migrationBuilder.DropTable(
                name: "FrameworkMenus");

            migrationBuilder.DropTable(
                name: "FrameworkRoles");

            migrationBuilder.DropTable(
                name: "FrameworkTenants");

            migrationBuilder.DropTable(
                name: "FrameworkUserGroups");

            migrationBuilder.DropTable(
                name: "FrameworkUserRoles");

            migrationBuilder.DropTable(
                name: "FrameworkUsers");

            migrationBuilder.DropTable(
                name: "FunctionPrivileges");

            migrationBuilder.DropTable(
                name: "FileAttachments");
        }
    }
}
