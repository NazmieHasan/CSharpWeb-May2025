using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedRoomEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Room identifier"),
                    Name = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false, comment: "Room name(number)"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Shows if movie is deleted"),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rooms_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Room in the system");

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "CategoryId", "Name" },
                values: new object[,]
                {
                    { new Guid("ae50a5ab-9642-466f-b528-3cc61071bb4c"), 1, "201" },
                    { new Guid("777634e2-3bb6-4748-8e91-7a10b70c78ac"), 1, "202" },
                    { new Guid("68fb84b9-ef2a-402f-b4fc-595006f5c275"), 1, "203" },
                    { new Guid("c1bd2a4a-6f9b-4daf-a0bb-4f7cdccf6101"), 1, "204" },
                    { new Guid("bfb7b7af-d533-4b7e-a7a9-69d27e0e5d47"), 1, "205" },
                    { new Guid("ee45d08c-f4d7-4b87-9ceb-6157c703a7dc"), 2, "301" },
                    { new Guid("f1e8ce5d-8c16-4bf6-9ff6-70db57fcd118"), 2, "302" },
                    { new Guid("40c553a9-f28f-4d17-bd83-92fd2c63ff91"), 2, "303" },
                    { new Guid("fbb4b2e4-7319-45a7-ac07-fe7e7345d5cb"), 2, "304" },
                    { new Guid("c6d17679-1de6-4cbb-b625-408b7bff3dc4"), 2, "305" },
                    { new Guid("7f48c43a-8c88-486b-a59e-0e89bd4453ec"), 3, "401" },
                    { new Guid("e76e6f0c-e838-4f47-b836-882b7ccc6983"), 3, "402" },
                    { new Guid("a93c5830-33fb-4d67-b8d8-468ed20c5efd"), 3, "403" },
                    { new Guid("8ffd66e2-7938-4e62-a246-f63cd583f2de"), 3, "404" },
                    { new Guid("d5f93a83-98f4-46a9-85da-ff2f3282e6f5"), 3, "405" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_CategoryId",
                table: "Rooms",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rooms");
        }
    }
}
