using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Web_API.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "GETDATE()"),
                    email = table.Column<string>(maxLength: 60, nullable: true),
                    name = table.Column<string>(maxLength: 60, nullable: true),
                    password = table.Column<string>(maxLength: 88, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "auth_access_tokens",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "GETDATE()"),
                    expires_at = table.Column<DateTime>(nullable: false),
                    token = table.Column<string>(maxLength: 88, nullable: true),
                    user_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_auth_access_tokens", x => x.id);
                    table.ForeignKey(
                        name: "FK_auth_access_tokens_Users_user_id",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "auth_refresh_tokens",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    access_token_id = table.Column<int>(nullable: false),
                    expires_at = table.Column<DateTime>(nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_auth_refresh_tokens", x => x.id);
                    table.ForeignKey(
                        name: "FK_auth_refresh_tokens_auth_access_tokens_access_token_id",
                        column: x => x.access_token_id,
                        principalTable: "auth_access_tokens",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_auth_access_tokens_user_id",
                table: "auth_access_tokens",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_auth_refresh_tokens_access_token_id",
                table: "auth_refresh_tokens",
                column: "access_token_id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "auth_refresh_tokens");

            migrationBuilder.DropTable(
                name: "auth_access_tokens");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
