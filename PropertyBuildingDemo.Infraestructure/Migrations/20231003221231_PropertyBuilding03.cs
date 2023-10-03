using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PropertyBuildingDemo.Infrastructure.Migrations
{
    public partial class PropertyBuilding03 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "PropertyTrace",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Property",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "InternalCode",
                table: "Property",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "8b2532a0-1742-49d6-acbe-14620a08e505",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "52976262-0420-4f4e-8e8c-813aaa59a3e6");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Owner",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyTrace_IdPropertyTrace",
                table: "PropertyTrace",
                column: "IdPropertyTrace");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyTrace_Name",
                table: "PropertyTrace",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyImage_IdPropertyImage",
                table: "PropertyImage",
                column: "IdPropertyImage");

            migrationBuilder.CreateIndex(
                name: "IX_Property_IdProperty",
                table: "Property",
                column: "IdProperty");

            migrationBuilder.CreateIndex(
                name: "IX_Property_Name",
                table: "Property",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Owner_IdOwner",
                table: "Owner",
                column: "IdOwner");

            migrationBuilder.CreateIndex(
                name: "IX_Owner_Name",
                table: "Owner",
                column: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PropertyTrace_IdPropertyTrace",
                table: "PropertyTrace");

            migrationBuilder.DropIndex(
                name: "IX_PropertyTrace_Name",
                table: "PropertyTrace");

            migrationBuilder.DropIndex(
                name: "IX_PropertyImage_IdPropertyImage",
                table: "PropertyImage");

            migrationBuilder.DropIndex(
                name: "IX_Property_IdProperty",
                table: "Property");

            migrationBuilder.DropIndex(
                name: "IX_Property_Name",
                table: "Property");

            migrationBuilder.DropIndex(
                name: "IX_Owner_IdOwner",
                table: "Owner");

            migrationBuilder.DropIndex(
                name: "IX_Owner_Name",
                table: "Owner");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "PropertyTrace",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Property",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "InternalCode",
                table: "Property",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "52976262-0420-4f4e-8e8c-813aaa59a3e6",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "8b2532a0-1742-49d6-acbe-14620a08e505");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Owner",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
