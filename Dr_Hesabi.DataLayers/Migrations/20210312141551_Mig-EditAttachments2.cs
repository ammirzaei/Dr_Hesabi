using Microsoft.EntityFrameworkCore.Migrations;

namespace Dr_Hesabi.DataLayers.Migrations
{
    public partial class MigEditAttachments2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserID",
                table: "Attachments",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_UserID",
                table: "Attachments",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Users_UserID",
                table: "Attachments",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Users_UserID",
                table: "Attachments");

            migrationBuilder.DropIndex(
                name: "IX_Attachments_UserID",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Attachments");
        }
    }
}
