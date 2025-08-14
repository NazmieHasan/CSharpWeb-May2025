using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedStayEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Stay",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GuestId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookingId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stay", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("2a523913-dd8e-44d1-a95e-d343ab4d4080"),
                column: "CreatedOn",
                value: new DateTime(2025, 8, 14, 3, 43, 44, 916, DateTimeKind.Utc).AddTicks(1691));

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("7da78485-b70d-4770-84f8-152ed4d9ccee"),
                column: "CreatedOn",
                value: new DateTime(2025, 8, 14, 3, 43, 44, 916, DateTimeKind.Utc).AddTicks(1624));

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("eb003919-0478-4b33-a168-170c78a8750b"),
                column: "CreatedOn",
                value: new DateTime(2025, 8, 14, 3, 43, 44, 916, DateTimeKind.Utc).AddTicks(1715));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "df1c3a0f-1234-4cde-bb55-d5f15a6aabcd",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "aa4396e3-7c07-4a1e-ab77-b3e28e53cc6b", "AQAAAAIAAYagAAAAEN2VjsYKuo3JPVLZEIIxiNkHH7zD+DI8ZRw3rzYqG3dGAy+bnB86lozQmudkaWdKTA==", "3aed23f4-3f51-4482-9883-20d422f1da53" });

            migrationBuilder.InsertData(
                table: "Stay",
                columns: new[] { "Id", "BookingId", "CreatedOn", "GuestId" },
                values: new object[,]
                {
                    { new Guid("7b6bdc1f-e561-4fd6-bd03-d9756b60978e"), "033b12ed-6bcc-428e-897e-32db2974bd92", new DateTime(2025, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "396a0c6d-1448-4f1f-8047-a0076620ff09" },
                    { new Guid("d1940e15-594b-42a0-bc52-0ae788fdc91e"), "b89a76cb-0680-4923-b99a-40c9ce018352", new DateTime(2025, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "ef570e9a-b8da-4cbe-af17-60e15cea5ee3" },
                    { new Guid("dc801e92-aacc-4151-9448-b3d4200332b1"), "033b12ed-6bcc-428e-897e-32db2974bd92", new DateTime(2025, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "1ad1d737-c60d-4d83-995a-3c8e42b2f236" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Stay");

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("2a523913-dd8e-44d1-a95e-d343ab4d4080"),
                column: "CreatedOn",
                value: new DateTime(2025, 8, 14, 3, 19, 37, 635, DateTimeKind.Utc).AddTicks(9069));

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("7da78485-b70d-4770-84f8-152ed4d9ccee"),
                column: "CreatedOn",
                value: new DateTime(2025, 8, 14, 3, 19, 37, 635, DateTimeKind.Utc).AddTicks(9035));

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("eb003919-0478-4b33-a168-170c78a8750b"),
                column: "CreatedOn",
                value: new DateTime(2025, 8, 14, 3, 19, 37, 635, DateTimeKind.Utc).AddTicks(9084));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "df1c3a0f-1234-4cde-bb55-d5f15a6aabcd",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4d1a763e-5c09-43bd-8b85-48e47bd65709", "AQAAAAIAAYagAAAAEDIELJsbf8Fkd6dW5lsARqtBbendPcpAo83nlZfWWvcF6GyCpw5rMgAfkqOD5pLSCw==", "c302d37c-30a6-4a50-bf95-be1b481c6180" });
        }
    }
}
