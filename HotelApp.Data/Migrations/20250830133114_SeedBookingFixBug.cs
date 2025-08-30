using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelApp.Data.Migrations
{
    public partial class SeedBookingFixBug : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[]
                {
                    "Id", "CreatedOn", "DateArrival", "DateDeparture", "AdultsCount",
                    "ChildCount", "BabyCount", "IsDeleted", "UserId", "RoomId", "ManagerId"
                },
                values: new object[,]
                {
                    {
                        Guid.Parse("7da78485-b70d-4770-84f8-152ed4d9ccee"),
                        new DateTime(2025, 8, 30, 13, 13, 54, DateTimeKind.Utc),
                        new DateOnly(2025, 8, 30),
                        new DateOnly(2025, 8, 31),
                        2, 0, 0, false,
                        "1b00f3f5-43ed-41f6-bdf2-ad5266370038",
                        Guid.Parse("AE50A5AB-9642-466F-B528-3CC61071BB4C"),
                        null
                    },
                    {
                        Guid.Parse("2a523913-dd8e-44d1-a95e-d343ab4d4080"),
                        new DateTime(2025, 8, 30, 13, 14, 54, DateTimeKind.Utc),
                        new DateOnly(2025, 8, 30),
                        new DateOnly(2025, 8, 31),
                        2, 0, 0, false,
                        "1b00f3f5-43ed-41f6-bdf2-ad5266370038",
                        Guid.Parse("68FB84B9-EF2A-402F-B4FC-595006F5C275"),
                        null
                    },
                    {
                        Guid.Parse("eb003919-0478-4b33-a168-170c78a8750b"),
                        new DateTime(2025, 8, 30, 13, 15, 54, DateTimeKind.Utc),
                        new DateOnly(2025, 8, 30),
                        new DateOnly(2025, 8, 31),
                        1, 0, 0, false,
                        "9c74337f-64e2-40ad-8bde-09b2631d17cb",
                        Guid.Parse("777634E2-3BB6-4748-8E91-7A10B70C78AC"),
                        null
                    }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: Guid.Parse("7da78485-b70d-4770-84f8-152ed4d9ccee"));

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: Guid.Parse("2a523913-dd8e-44d1-a95e-d343ab4d4080"));

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: Guid.Parse("eb003919-0478-4b33-a168-170c78a8750b"));
        }
    }
}
