using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrandShop.Data.Migrations
{
    public partial class UpdateHeroTableAddButtons : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BtnLink",
                table: "Heroes",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BtnText",
                table: "Heroes",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BtnLink",
                table: "Heroes");

            migrationBuilder.DropColumn(
                name: "BtnText",
                table: "Heroes");
        }
    }
}
