using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PropertyBuildingDemo.Infrastructure.Migrations
{
    public partial class PropertyBuilding04 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "InternalCode",
                table: "Property",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "64421293-0407-4c22-9925-e3b186e1a552",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "8b2532a0-1742-49d6-acbe-14620a08e505");

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Property",
                type: "rowversion",
                rowVersion: true,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Property");

            migrationBuilder.AlterColumn<string>(
                name: "InternalCode",
                table: "Property",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "8b2532a0-1742-49d6-acbe-14620a08e505",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "64421293-0407-4c22-9925-e3b186e1a552");
        }
    }
}
