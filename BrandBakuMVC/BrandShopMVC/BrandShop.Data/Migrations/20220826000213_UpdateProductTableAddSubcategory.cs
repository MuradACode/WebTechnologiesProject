using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrandShop.Data.Migrations
{
    public partial class UpdateProductTableAddSubcategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Subcategories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubcategoryId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Subcategories_ProductId",
                table: "Subcategories",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subcategories_Products_ProductId",
                table: "Subcategories",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subcategories_Products_ProductId",
                table: "Subcategories");

            migrationBuilder.DropIndex(
                name: "IX_Subcategories_ProductId",
                table: "Subcategories");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Subcategories");

            migrationBuilder.DropColumn(
                name: "SubcategoryId",
                table: "Products");
        }
    }
}
