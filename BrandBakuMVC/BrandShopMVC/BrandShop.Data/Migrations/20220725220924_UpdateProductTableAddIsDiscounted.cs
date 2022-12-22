using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrandShop.Data.Migrations
{
    public partial class UpdateProductTableAddIsDiscounted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDiscounted",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDiscounted",
                table: "Products");
        }
    }
}
