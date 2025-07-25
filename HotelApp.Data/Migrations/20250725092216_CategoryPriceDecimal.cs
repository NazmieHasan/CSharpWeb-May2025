using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class CategoryPriceDecimal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Categories",
                type: "decimal(18,3)",
                nullable: false,
                comment: "Category price",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldComment: "Category price");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Categories",
                type: "decimal(18,2)",
                nullable: false,
                comment: "Category price",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,3)",
                oldComment: "Category price");
        }
    }
}
