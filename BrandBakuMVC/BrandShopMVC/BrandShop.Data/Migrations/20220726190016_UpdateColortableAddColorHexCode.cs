using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrandShop.Data.Migrations
{
    public partial class UpdateColortableAddColorHexCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ColorHexCode",
                table: "Colors",
                type: "nvarchar(7)",
                maxLength: 7,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColorHexCode",
                table: "Colors");
        }
    }
}
