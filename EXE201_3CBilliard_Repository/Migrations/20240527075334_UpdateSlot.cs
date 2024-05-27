using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EXE201_3CBilliard_Repository.Migrations
{
    public partial class UpdateSlot : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SlotTime",
                table: "Slot");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "EndTime",
                table: "Slot",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "StartTime",
                table: "Slot",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Slot");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "Slot");

            migrationBuilder.AddColumn<string>(
                name: "SlotTime",
                table: "Slot",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
