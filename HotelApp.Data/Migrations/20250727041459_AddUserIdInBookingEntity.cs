using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdInBookingEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "df1c3a0f-1234-4cde-bb55-d5f15a6aabcd",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d8d7a523-ebd4-4f82-9ea3-85ca4a05567a", "AQAAAAIAAYagAAAAEC9xlt0T+/3W65k/HXH2IbHMTM0MuCl0D26+7+zy0S+cgacCT267BQtQ6RFaxrSCYQ==", "23839c53-7de3-46de-963a-fc2cb1416825" });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("2a523913-dd8e-44d1-a95e-d343ab4d4080"),
                columns: new[] { "CreatedOn", "DateArrival", "DateDeparture", "UserId" },
                values: new object[] { new DateTime(2025, 7, 27, 4, 14, 52, 516, DateTimeKind.Utc).AddTicks(3039), new DateOnly(2025, 7, 29), new DateOnly(2025, 8, 1), "df1c3a0f-1234-4cde-bb55-d5f15a6aabcd" });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("7da78485-b70d-4770-84f8-152ed4d9ccee"),
                columns: new[] { "CreatedOn", "DateArrival", "DateDeparture", "UserId" },
                values: new object[] { new DateTime(2025, 7, 27, 4, 14, 52, 516, DateTimeKind.Utc).AddTicks(2997), new DateOnly(2025, 7, 28), new DateOnly(2025, 7, 30), "df1c3a0f-1234-4cde-bb55-d5f15a6aabcd" });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("eb003919-0478-4b33-a168-170c78a8750b"),
                columns: new[] { "CreatedOn", "DateArrival", "DateDeparture", "UserId" },
                values: new object[] { new DateTime(2025, 7, 27, 4, 14, 52, 523, DateTimeKind.Utc).AddTicks(9280), new DateOnly(2025, 7, 30), new DateOnly(2025, 7, 31), "df1c3a0f-1234-4cde-bb55-d5f15a6aabcd" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Bookings");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "df1c3a0f-1234-4cde-bb55-d5f15a6aabcd",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d4bd4192-9b26-43e9-baea-985b56309585", "AQAAAAIAAYagAAAAEHTqhs/EqxWiyamquIXploJKLFD7XTX0sJIX1e+C3tyBm1emPu7qXyb1CcrpfZ7QUg==", "e56c128f-d07e-4c60-b841-5c4bceec5a2c" });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("2a523913-dd8e-44d1-a95e-d343ab4d4080"),
                columns: new[] { "CreatedOn", "DateArrival", "DateDeparture" },
                values: new object[] { new DateTime(2025, 7, 26, 19, 58, 29, 520, DateTimeKind.Utc).AddTicks(5198), new DateOnly(2025, 7, 28), new DateOnly(2025, 7, 31) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("7da78485-b70d-4770-84f8-152ed4d9ccee"),
                columns: new[] { "CreatedOn", "DateArrival", "DateDeparture" },
                values: new object[] { new DateTime(2025, 7, 26, 19, 58, 29, 520, DateTimeKind.Utc).AddTicks(5116), new DateOnly(2025, 7, 27), new DateOnly(2025, 7, 29) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("eb003919-0478-4b33-a168-170c78a8750b"),
                columns: new[] { "CreatedOn", "DateArrival", "DateDeparture" },
                values: new object[] { new DateTime(2025, 7, 26, 19, 58, 29, 520, DateTimeKind.Utc).AddTicks(5213), new DateOnly(2025, 7, 29), new DateOnly(2025, 7, 30) });
        }
    }
}
