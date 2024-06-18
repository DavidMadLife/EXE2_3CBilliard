using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EXE201_3CBilliard_Repository.Migrations
{
    public partial class fixNoti : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "Notificate",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Notificate_UserId",
                table: "Notificate",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notificate_User_UserId",
                table: "Notificate",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notificate_User_UserId",
                table: "Notificate");

            migrationBuilder.DropIndex(
                name: "IX_Notificate_UserId",
                table: "Notificate");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Notificate");
        }
    }
}
