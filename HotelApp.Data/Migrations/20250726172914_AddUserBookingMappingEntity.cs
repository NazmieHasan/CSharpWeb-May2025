using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserBookingMappingEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationUserBookings",
                columns: table => new
                {
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "Foreign key to the referenced AspNetUser. Part of the entity composite PK."),
                    BookingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Foreign key to the referenced Booking. Part of the entity composite PK."),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Shows if ApplicationUserBooking entry is deleted")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserBookings", x => new { x.ApplicationUserId, x.BookingId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserBookings_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplicationUserBookings_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "User Booking entry in the system.");

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("2a523913-dd8e-44d1-a95e-d343ab4d4080"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 26, 17, 29, 7, 100, DateTimeKind.Utc).AddTicks(2102));

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("7da78485-b70d-4770-84f8-152ed4d9ccee"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 26, 17, 29, 7, 100, DateTimeKind.Utc).AddTicks(2066));

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("eb003919-0478-4b33-a168-170c78a8750b"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 26, 17, 29, 7, 100, DateTimeKind.Utc).AddTicks(2118));

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserBookings_BookingId",
                table: "ApplicationUserBookings",
                column: "BookingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserBookings");

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("2a523913-dd8e-44d1-a95e-d343ab4d4080"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 26, 11, 39, 55, 606, DateTimeKind.Utc).AddTicks(132));

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("7da78485-b70d-4770-84f8-152ed4d9ccee"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 26, 11, 39, 55, 606, DateTimeKind.Utc).AddTicks(70));

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("eb003919-0478-4b33-a168-170c78a8750b"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 26, 11, 39, 55, 606, DateTimeKind.Utc).AddTicks(151));
        }
    }
}
