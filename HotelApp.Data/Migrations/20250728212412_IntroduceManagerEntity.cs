using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class IntroduceManagerEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ManagerId",
                table: "Bookings",
                type: "uniqueidentifier",
                nullable: true,
                comment: "Booking's manager");

            migrationBuilder.CreateTable(
                name: "Manager",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Manager identifier"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "Manager's user entity")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manager", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Manager_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Manager in the system");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "df1c3a0f-1234-4cde-bb55-d5f15a6aabcd",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2ae82bb1-1c53-4fe8-9160-92e14658ba8f", "AQAAAAIAAYagAAAAEBIiaIKm3j+0pqj5w0+KwktDGzf580usEF5xWH3nQZknORSeGuoRmP7s18N6YmVE8A==", "a6c20c9f-78e4-4c90-907a-b776e82587d6" });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("2a523913-dd8e-44d1-a95e-d343ab4d4080"),
                columns: new[] { "CreatedOn", "DateArrival", "DateDeparture", "ManagerId" },
                values: new object[] { new DateTime(2025, 7, 28, 21, 24, 4, 561, DateTimeKind.Utc).AddTicks(2515), new DateOnly(2025, 7, 30), new DateOnly(2025, 8, 2), null });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("7da78485-b70d-4770-84f8-152ed4d9ccee"),
                columns: new[] { "CreatedOn", "DateArrival", "DateDeparture", "ManagerId" },
                values: new object[] { new DateTime(2025, 7, 28, 21, 24, 4, 561, DateTimeKind.Utc).AddTicks(2442), new DateOnly(2025, 7, 29), new DateOnly(2025, 7, 31), null });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("eb003919-0478-4b33-a168-170c78a8750b"),
                columns: new[] { "CreatedOn", "DateArrival", "DateDeparture", "ManagerId" },
                values: new object[] { new DateTime(2025, 7, 28, 21, 24, 4, 561, DateTimeKind.Utc).AddTicks(2536), new DateOnly(2025, 7, 31), new DateOnly(2025, 8, 1), null });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ManagerId",
                table: "Bookings",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Manager_UserId",
                table: "Manager",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Manager_ManagerId",
                table: "Bookings",
                column: "ManagerId",
                principalTable: "Manager",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Manager_ManagerId",
                table: "Bookings");

            migrationBuilder.DropTable(
                name: "Manager");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_ManagerId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "Bookings");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "df1c3a0f-1234-4cde-bb55-d5f15a6aabcd",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "02cd9c0e-97fa-4935-8d5a-74219935cc8c", "AQAAAAIAAYagAAAAECQKs/vh/HdIe6dyPZ0yYbAiyiUJUpZUWCAbjXIdp7S7FzPvwwrkPtOjLzJ6qQrDEg==", "f03b39c2-658b-48b9-ae8c-cdaab28c7aa8" });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("2a523913-dd8e-44d1-a95e-d343ab4d4080"),
                columns: new[] { "CreatedOn", "DateArrival", "DateDeparture" },
                values: new object[] { new DateTime(2025, 7, 27, 4, 21, 5, 420, DateTimeKind.Utc).AddTicks(9612), new DateOnly(2025, 7, 29), new DateOnly(2025, 8, 1) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("7da78485-b70d-4770-84f8-152ed4d9ccee"),
                columns: new[] { "CreatedOn", "DateArrival", "DateDeparture" },
                values: new object[] { new DateTime(2025, 7, 27, 4, 21, 5, 420, DateTimeKind.Utc).AddTicks(9560), new DateOnly(2025, 7, 28), new DateOnly(2025, 7, 30) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("eb003919-0478-4b33-a168-170c78a8750b"),
                columns: new[] { "CreatedOn", "DateArrival", "DateDeparture" },
                values: new object[] { new DateTime(2025, 7, 27, 4, 21, 5, 420, DateTimeKind.Utc).AddTicks(9721), new DateOnly(2025, 7, 30), new DateOnly(2025, 7, 31) });
        }
    }
}
