using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedCategoriesDev : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Beds", "Description", "ImageUrl", "Name", "Price" },
                values: new object[,]
                {
                    { 1, 2, "Modern and stylish design for you", "https://cdn.pixabay.com/photo/2015/11/06/11/45/interior-1026452_960_720.jpg", "Double Room", 500.00m },
                    { 2, 4, "Modern design, comfort and convenience", "https://cdn.pixabay.com/photo/2017/04/28/22/14/room-2269591_960_720.jpg", "Apartment", 800.00m },
                    { 3, 4, "Luxury, elegance and comfort", "https://cdn.pixabay.com/photo/2015/01/10/11/39/hotel-595121_960_720.jpg", "Apartment Lux", 1500.00m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
