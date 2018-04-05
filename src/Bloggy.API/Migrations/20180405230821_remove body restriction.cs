using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Bloggy.API.Migrations
{
    public partial class removebodyrestriction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Body",
                table: "Posts",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 140);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Body",
                table: "Posts",
                maxLength: 140,
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
