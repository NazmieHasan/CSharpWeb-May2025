using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddOwnerAndIsForAnotherPersonInBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsForAnotherPerson",
                table: "Bookings",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Indicates whether the booking is for another person");

            migrationBuilder.AddColumn<string>(
                name: "Owner",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                comment: "Owner name of the booking");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Owner",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "IsForAnotherPerson",
                table: "Bookings");
        }
    }            
}
