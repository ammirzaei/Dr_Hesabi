using Microsoft.EntityFrameworkCore.Migrations;

namespace Dr_Hesabi.DataLayers.Migrations
{
    public partial class MigEmailSupport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailSupport",
                table: "Setting",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Connections",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EmailorPhone",
                table: "Connections",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailSupport",
                table: "Setting");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Connections",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "EmailorPhone",
                table: "Connections",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 150);
        }
    }
}
