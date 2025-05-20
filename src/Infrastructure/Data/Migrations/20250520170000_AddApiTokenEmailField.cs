using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace dotnet_exemplar.Infrastructure.Data.Migrations
{
    public partial class AddApiTokenEmailField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "ApiTokens",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "ApiTokens");
        }
    }
}