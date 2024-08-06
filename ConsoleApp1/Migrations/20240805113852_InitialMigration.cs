using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConsoleApp1.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fullName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("users_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "message",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    message_text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    data_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Is_send = table.Column<bool>(type: "bit", nullable: false),
                    UserToId = table.Column<int>(type: "int", nullable: false),
                    UserFromId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("message_pk", x => x.id);
                    table.ForeignKey(
                        name: "message_from_user_fk",
                        column: x => x.UserFromId,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "message_to_user_fk",
                        column: x => x.UserToId,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_message_UserFromId",
                table: "message",
                column: "UserFromId");

            migrationBuilder.CreateIndex(
                name: "IX_message_UserToId",
                table: "message",
                column: "UserToId");

            migrationBuilder.CreateIndex(
                name: "IX_users_fullName",
                table: "users",
                column: "fullName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "message");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
