﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EXE201_3CBilliard_Repository.Migrations
{
    public partial class bug_booking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Bill");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Bill",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
