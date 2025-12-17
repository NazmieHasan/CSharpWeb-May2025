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
                    { 1, 2, "Modern and stylish design", "/images/upload/categories/7fb46e46-d4d6-41b8-8b54-9c81096462f1.jpg", false, "Double Room", 300.00m },
                    { 2, 3, "Convenience for all the family", "/images/upload/categories/3ce40dc4-f2a7-4cf1-97e6-3d9e68f892fe.jpg", false, "Family Room", 500.00m },
                    { 3, 4, "Modern design, comfort and convenience", "/images/upload/categories/b77081e5-111e-4220-a122-597e46708efd.jpg", false, "Apartment", 1000.00m },
                    { 4, 2, "Luxury, elegance and comfort", "/images/upload/categories/e3d7e66a-aebd-45d7-9ad4-04f65c2704f6.jpg", false, "Double Room L", 400.00m },
                    { 5, 4, "Luxury, elegance and comfort", "/images/upload/categories/44e74722-5f1d-4736-bf28-bc7d914a9192.jpg", false, "Apartment L", 1500.00m },
                    { 6, 4, "Super luxury, elegance and comfort", "/images/upload/categories/7aa43c19-831a-4890-bd6e-dded11e88d3f.jpg", false, "Apartment Super L", 2000.00m }
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

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}
