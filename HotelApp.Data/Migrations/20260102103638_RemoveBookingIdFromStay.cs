using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveBookingIdFromStay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stays_Bookings_BookingId",
                table: "Stays");

            migrationBuilder.DropIndex(
                name: "IX_Stays_BookingId",
                table: "Stays");

            migrationBuilder.DropColumn(
                name: "BookingId",
                table: "Stays");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BookingId",
                table: "Stays",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Stays_BookingId",
                table: "Stays",
                column: "BookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stays_Bookings_BookingId",
                table: "Stays",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
