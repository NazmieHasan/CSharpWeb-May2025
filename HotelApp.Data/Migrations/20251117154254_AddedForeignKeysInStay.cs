using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedForeignKeysInStay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "GuestId",
                table: "Stays",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "Stays",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<Guid>(
                name: "BookingId",
                table: "Stays",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckoutOn",
                table: "Stays",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Stays",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Shows if stay is deleted");

            migrationBuilder.CreateIndex(
                name: "IX_Stays_BookingId",
                table: "Stays",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_Stays_GuestId",
                table: "Stays",
                column: "GuestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stays_Bookings_BookingId",
                table: "Stays",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Stays_Guests_GuestId",
                table: "Stays",
                column: "GuestId",
                principalTable: "Guests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stays_Bookings_BookingId",
                table: "Stays");

            migrationBuilder.DropForeignKey(
                name: "FK_Stays_Guests_GuestId",
                table: "Stays");

            migrationBuilder.DropIndex(
                name: "IX_Stays_BookingId",
                table: "Stays");

            migrationBuilder.DropIndex(
                name: "IX_Stays_GuestId",
                table: "Stays");

            migrationBuilder.DropColumn(
                name: "CheckoutOn",
                table: "Stays");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Stays");

            migrationBuilder.AlterColumn<string>(
                name: "GuestId",
                table: "Stays",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "Stays",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<string>(
                name: "BookingId",
                table: "Stays",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");
        }
    }
}
