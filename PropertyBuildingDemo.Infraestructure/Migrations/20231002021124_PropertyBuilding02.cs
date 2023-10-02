using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PropertyBuildingDemo.Infrastructure.Migrations
{
    public partial class PropertyBuilding02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "InternalCode",
                table: "Property",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "52976262-0420-4f4e-8e8c-813aaa59a3e6",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "300bf100-a7dd-4764-a578-41c4a3f15820");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "InternalCode",
                table: "Property",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "300bf100-a7dd-4764-a578-41c4a3f15820",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "52976262-0420-4f4e-8e8c-813aaa59a3e6");
        }
    }
}
