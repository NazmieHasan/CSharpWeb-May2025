using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveBookingCheckConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Booking_DateArrival_NotPast",
                table: "Bookings");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Booking_DepartureAfterArrival",
                table: "Bookings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "CK_Booking_DateArrival_NotPast",
                table: "Bookings",
                sql: "[DateArrival] >= CONVERT(date, GETUTCDATE())");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Booking_DepartureAfterArrival",
                table: "Bookings",
                sql: "[DateDeparture] > [DateArrival]");
        }
    }
}
