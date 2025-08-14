using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedGuestEntityAndSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Guests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FamilyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guests", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("2a523913-dd8e-44d1-a95e-d343ab4d4080"),
                columns: new[] { "CreatedOn", "DateArrival", "DateDeparture" },
                values: new object[] { new DateTime(2025, 8, 14, 1, 57, 24, 635, DateTimeKind.Utc).AddTicks(742), new DateOnly(2025, 8, 16), new DateOnly(2025, 8, 19) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("7da78485-b70d-4770-84f8-152ed4d9ccee"),
                columns: new[] { "CreatedOn", "DateArrival", "DateDeparture" },
                values: new object[] { new DateTime(2025, 8, 14, 1, 57, 24, 635, DateTimeKind.Utc).AddTicks(624), new DateOnly(2025, 8, 15), new DateOnly(2025, 8, 17) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("eb003919-0478-4b33-a168-170c78a8750b"),
                columns: new[] { "CreatedOn", "DateArrival", "DateDeparture" },
                values: new object[] { new DateTime(2025, 8, 14, 1, 57, 24, 635, DateTimeKind.Utc).AddTicks(848), new DateOnly(2025, 8, 17), new DateOnly(2025, 8, 18) });

            migrationBuilder.InsertData(
                table: "Guests",
                columns: new[] { "Id", "CreatedOn", "FamilyName", "FirstName", "PhoneNumber" },
                values: new object[,]
                {
                    { new Guid("5d041311-f1b6-44c9-b453-de3c3ad4a7c4"), new DateTime(2025, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Doe", "John", "+111122222" },
                    { new Guid("ad35b73a-9686-4df6-a2d6-210b757370ab"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Smith", "Jane", "+222233333" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "df1c3a0f-1234-4cde-bb55-d5f15a6aabcd",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "17a36405-4517-4c42-ab71-d592247b3949", "AQAAAAIAAYagAAAAENkupFPUUNDg9/YeHbJxKQaFCmAqhidEDyIzaBWirH67NCYNEQxyjucQrfm08uSVJw==", "0eb9f01f-6231-4853-b02c-1e1ed4237f38" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Guests");

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("2a523913-dd8e-44d1-a95e-d343ab4d4080"),
                columns: new[] { "CreatedOn", "DateArrival", "DateDeparture" },
                values: new object[] { new DateTime(2025, 8, 13, 18, 42, 44, 653, DateTimeKind.Utc).AddTicks(6573), new DateOnly(2025, 8, 15), new DateOnly(2025, 8, 18) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("7da78485-b70d-4770-84f8-152ed4d9ccee"),
                columns: new[] { "CreatedOn", "DateArrival", "DateDeparture" },
                values: new object[] { new DateTime(2025, 8, 13, 18, 42, 44, 653, DateTimeKind.Utc).AddTicks(6521), new DateOnly(2025, 8, 14), new DateOnly(2025, 8, 16) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("eb003919-0478-4b33-a168-170c78a8750b"),
                columns: new[] { "CreatedOn", "DateArrival", "DateDeparture" },
                values: new object[] { new DateTime(2025, 8, 13, 18, 42, 44, 653, DateTimeKind.Utc).AddTicks(6681), new DateOnly(2025, 8, 16), new DateOnly(2025, 8, 17) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "df1c3a0f-1234-4cde-bb55-d5f15a6aabcd",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "01f7e1a5-8f49-439b-963d-8657898308b3", "AQAAAAIAAYagAAAAEPeV9wasg+Y9fFyWJ0z6GP/58qe4hh0YRgz16mpIoBWFpeK3JjfsiJv76zXa2xKk5w==", "3b1482ec-c205-4a2e-8f2f-151a95c11b8d" });
        }
    }
}
