using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBookingEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterTable(
                name: "Categories",
                comment: "Category in the system");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Rooms",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Shows if room is deleted",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Shows if movie is deleted");

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Booking identifier"),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DateArrival = table.Column<DateOnly>(type: "date", nullable: false),
                    DateDeparture = table.Column<DateOnly>(type: "date", nullable: false),
                    AdultsCount = table.Column<int>(type: "int", nullable: false),
                    ChildCount = table.Column<int>(type: "int", nullable: false, comment: "Child age is between 4 and 17"),
                    BabyCount = table.Column<int>(type: "int", nullable: false, comment: "Baby age is between 0 and 3"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Shows if booking is deleted"),
                    RoomId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.CheckConstraint("CK_Booking_DateArrival_NotPast", "[DateArrival] >= CONVERT(date, GETUTCDATE())");
                    table.CheckConstraint("CK_Booking_DepartureAfterArrival", "[DateDeparture] > [DateArrival]");
                    table.ForeignKey(
                        name: "FK_Bookings_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Booking in the system");

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "Id", "AdultsCount", "BabyCount", "ChildCount", "CreatedOn", "DateArrival", "DateDeparture", "RoomId" },
                values: new object[,]
                {
                    { new Guid("2a523913-dd8e-44d1-a95e-d343ab4d4080"), 2, 0, 0, new DateTime(2025, 7, 26, 11, 39, 55, 606, DateTimeKind.Utc).AddTicks(132), new DateOnly(2025, 7, 28), new DateOnly(2025, 7, 31), new Guid("68fb84b9-ef2a-402f-b4fc-595006f5c275") },
                    { new Guid("7da78485-b70d-4770-84f8-152ed4d9ccee"), 2, 0, 0, new DateTime(2025, 7, 26, 11, 39, 55, 606, DateTimeKind.Utc).AddTicks(70), new DateOnly(2025, 7, 27), new DateOnly(2025, 7, 29), new Guid("ae50a5ab-9642-466f-b528-3cc61071bb4c") },
                    { new Guid("eb003919-0478-4b33-a168-170c78a8750b"), 1, 0, 0, new DateTime(2025, 7, 26, 11, 39, 55, 606, DateTimeKind.Utc).AddTicks(151), new DateOnly(2025, 7, 29), new DateOnly(2025, 7, 30), new Guid("777634e2-3bb6-4748-8e91-7a10b70c78ac") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_RoomId",
                table: "Bookings",
                column: "RoomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.AlterTable(
                name: "Categories",
                oldComment: "Category in the system");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Rooms",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Shows if movie is deleted",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Shows if room is deleted");
        }
    }
}
