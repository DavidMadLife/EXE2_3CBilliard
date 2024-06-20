using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EXE201_3CBilliard_Repository.Migrations
{
    public partial class add_likeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateAt",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "Descrpition",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "Descrpition",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "Evalution",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "Like",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Comment");

            migrationBuilder.RenameColumn(
                name: "Note",
                table: "Post",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "ModifineAt",
                table: "Post",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Post",
                newName: "Content");

            migrationBuilder.RenameColumn(
                name: "Note",
                table: "Comment",
                newName: "Content");

            migrationBuilder.RenameColumn(
                name: "CreateAt",
                table: "Comment",
                newName: "CreatedAt");

            migrationBuilder.CreateTable(
                name: "Like",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    PostId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Like", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Like_Post_PostId",
                        column: x => x.PostId,
                        principalTable: "Post",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Like_PostId",
                table: "Like",
                column: "PostId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Like");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Post",
                newName: "Note");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Post",
                newName: "ModifineAt");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Post",
                newName: "Image");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Comment",
                newName: "CreateAt");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Comment",
                newName: "Note");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateAt",
                table: "Post",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Descrpition",
                table: "Post",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Post",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Descrpition",
                table: "Comment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "Evalution",
                table: "Comment",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Comment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "Like",
                table: "Comment",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Comment",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
