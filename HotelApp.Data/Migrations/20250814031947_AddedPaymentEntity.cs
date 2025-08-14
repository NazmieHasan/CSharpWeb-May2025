using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedPaymentEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookingId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentUserFullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentUserPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                });

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

            migrationBuilder.InsertData(
                table: "Payments",
                columns: new[] { "Id", "Amount", "BookingId", "CreatedOn", "PaymentUserFullName", "PaymentUserPhoneNumber" },
                values: new object[] { new Guid("7b6bdc1f-e561-4fd6-bd03-d9756b60978e"), "500", "2a42e8c7-ba40-46e9-a6c0-7dca4751f087", new DateTime(2025, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Alex Doe", "Doe" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("2a523913-dd8e-44d1-a95e-d343ab4d4080"),
                column: "CreatedOn",
                value: new DateTime(2025, 8, 14, 1, 57, 24, 635, DateTimeKind.Utc).AddTicks(742));

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("7da78485-b70d-4770-84f8-152ed4d9ccee"),
                column: "CreatedOn",
                value: new DateTime(2025, 8, 14, 1, 57, 24, 635, DateTimeKind.Utc).AddTicks(624));

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("eb003919-0478-4b33-a168-170c78a8750b"),
                column: "CreatedOn",
                value: new DateTime(2025, 8, 14, 1, 57, 24, 635, DateTimeKind.Utc).AddTicks(848));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "df1c3a0f-1234-4cde-bb55-d5f15a6aabcd",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "17a36405-4517-4c42-ab71-d592247b3949", "AQAAAAIAAYagAAAAENkupFPUUNDg9/YeHbJxKQaFCmAqhidEDyIzaBWirH67NCYNEQxyjucQrfm08uSVJw==", "0eb9f01f-6231-4853-b02c-1e1ed4237f38" });
        }
    }
}
