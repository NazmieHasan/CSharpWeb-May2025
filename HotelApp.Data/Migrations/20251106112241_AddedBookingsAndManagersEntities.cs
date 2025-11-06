using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedBookingsAndManagersEntities : Migration
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
                name: "Managers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Manager identifier"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "Manager's user entity")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Managers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Managers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Manager in the system");

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
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ManagerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Booking's manager"),
                    RoomId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.CheckConstraint("CK_Booking_DateArrival_NotPast", "[DateArrival] >= CONVERT(date, GETUTCDATE())");
                    table.CheckConstraint("CK_Booking_DepartureAfterArrival", "[DateDeparture] > [DateArrival]");
                    table.ForeignKey(
                        name: "FK_Bookings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bookings_Managers_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Managers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Bookings_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Booking in the system");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ManagerId",
                table: "Bookings",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_RoomId",
                table: "Bookings",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_UserId",
                table: "Bookings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Managers_UserId",
                table: "Managers",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Managers");

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
