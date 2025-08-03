using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixMultipleQueryFilters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "df1c3a0f-1234-4cde-bb55-d5f15a6aabcd",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "cd51afc8-a13c-4caa-89b3-894e5ab7138f", "AQAAAAIAAYagAAAAEFUJw0HysDhFf/5nqyfv8oFw8xHw87DFAJ/CBxy++XmN79Hr874begSFhCgQ7L6vfA==", "a1aca26a-58c2-4daa-801b-0496ce23f1bf" });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("2a523913-dd8e-44d1-a95e-d343ab4d4080"),
                columns: new[] { "CreatedOn", "DateArrival", "DateDeparture" },
                values: new object[] { new DateTime(2025, 8, 3, 9, 55, 21, 717, DateTimeKind.Utc).AddTicks(2836), new DateOnly(2025, 8, 5), new DateOnly(2025, 8, 8) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("7da78485-b70d-4770-84f8-152ed4d9ccee"),
                columns: new[] { "CreatedOn", "DateArrival", "DateDeparture" },
                values: new object[] { new DateTime(2025, 8, 3, 9, 55, 21, 717, DateTimeKind.Utc).AddTicks(2685), new DateOnly(2025, 8, 4), new DateOnly(2025, 8, 6) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("eb003919-0478-4b33-a168-170c78a8750b"),
                columns: new[] { "CreatedOn", "DateArrival", "DateDeparture" },
                values: new object[] { new DateTime(2025, 8, 3, 9, 55, 21, 717, DateTimeKind.Utc).AddTicks(2860), new DateOnly(2025, 8, 6), new DateOnly(2025, 8, 7) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "df1c3a0f-1234-4cde-bb55-d5f15a6aabcd",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5b490252-db74-4320-9dea-07a533f7ed91", "AQAAAAIAAYagAAAAEIDP0BU5nYwwgrsFVNEIum1n4yJkZ8aLwYN1rQPVNQ53I+ISGeX3zHLAe6qRasZzBw==", "927413b6-a4ce-450f-801d-d16661a73140" });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("2a523913-dd8e-44d1-a95e-d343ab4d4080"),
                columns: new[] { "CreatedOn", "DateArrival", "DateDeparture" },
                values: new object[] { new DateTime(2025, 7, 28, 22, 58, 34, 74, DateTimeKind.Utc).AddTicks(7764), new DateOnly(2025, 7, 30), new DateOnly(2025, 8, 2) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("7da78485-b70d-4770-84f8-152ed4d9ccee"),
                columns: new[] { "CreatedOn", "DateArrival", "DateDeparture" },
                values: new object[] { new DateTime(2025, 7, 28, 22, 58, 34, 74, DateTimeKind.Utc).AddTicks(7714), new DateOnly(2025, 7, 29), new DateOnly(2025, 7, 31) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("eb003919-0478-4b33-a168-170c78a8750b"),
                columns: new[] { "CreatedOn", "DateArrival", "DateDeparture" },
                values: new object[] { new DateTime(2025, 7, 28, 22, 58, 34, 74, DateTimeKind.Utc).AddTicks(7776), new DateOnly(2025, 7, 31), new DateOnly(2025, 8, 1) });
        }
    }
}
