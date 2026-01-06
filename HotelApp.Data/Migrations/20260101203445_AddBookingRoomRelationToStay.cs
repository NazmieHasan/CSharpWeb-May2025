using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBookingRoomRelationToStay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BookingRoomId",
                table: "Stays",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Stays_BookingRoomId",
                table: "Stays",
                column: "BookingRoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stays_BookingRooms_BookingRoomId",
                table: "Stays",
                column: "BookingRoomId",
                principalTable: "BookingRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stays_BookingRooms_BookingRoomId",
                table: "Stays");

            migrationBuilder.DropIndex(
                name: "IX_Stays_BookingRoomId",
                table: "Stays");

            migrationBuilder.DropColumn(
                name: "BookingRoomId",
                table: "Stays");
        }
    }
}
