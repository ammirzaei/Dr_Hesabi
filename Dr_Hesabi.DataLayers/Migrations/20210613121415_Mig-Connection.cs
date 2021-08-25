using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dr_Hesabi.DataLayers.Migrations
{
    public partial class MigConnection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Connections_Connections_ParentID",
                table: "Connections");

            migrationBuilder.DropForeignKey(
                name: "FK_Connections_Users_UserID2",
                table: "Connections");

            migrationBuilder.DropIndex(
                name: "IX_Connections_ParentID",
                table: "Connections");

            migrationBuilder.DropIndex(
                name: "IX_Connections_UserID2",
                table: "Connections");

            migrationBuilder.DropColumn(
                name: "DateTime",
                table: "Connections");

            migrationBuilder.DropColumn(
                name: "IsReplayAdmin",
                table: "Connections");

            migrationBuilder.DropColumn(
                name: "ParentID",
                table: "Connections");

            migrationBuilder.DropColumn(
                name: "UserID2",
                table: "Connections");

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "Connections",
                maxLength: 800,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<string>(
                name: "CommentTitle",
                table: "Connections",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "Connections",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "EmailorPhone",
                table: "Connections",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Connections",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSeen",
                table: "Connections",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommentTitle",
                table: "Connections");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "Connections");

            migrationBuilder.DropColumn(
                name: "EmailorPhone",
                table: "Connections");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Connections");

            migrationBuilder.DropColumn(
                name: "IsSeen",
                table: "Connections");

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "Connections",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 800);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTime",
                table: "Connections",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsReplayAdmin",
                table: "Connections",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ParentID",
                table: "Connections",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserID2",
                table: "Connections",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Connections_ParentID",
                table: "Connections",
                column: "ParentID");

            migrationBuilder.CreateIndex(
                name: "IX_Connections_UserID2",
                table: "Connections",
                column: "UserID2");

            migrationBuilder.AddForeignKey(
                name: "FK_Connections_Connections_ParentID",
                table: "Connections",
                column: "ParentID",
                principalTable: "Connections",
                principalColumn: "ConnectionID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Connections_Users_UserID2",
                table: "Connections",
                column: "UserID2",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
