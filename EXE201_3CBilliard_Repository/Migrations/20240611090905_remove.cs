using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EXE201_3CBilliard_Repository.Migrations
{
    public partial class remove : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OpeningHours",
                table: "BidaClub",
                newName: "GoogleMapLink");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GoogleMapLink",
                table: "BidaClub",
                newName: "OpeningHours");
        }
    }
}
