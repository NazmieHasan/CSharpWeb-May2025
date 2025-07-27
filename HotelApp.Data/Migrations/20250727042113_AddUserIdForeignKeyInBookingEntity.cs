using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdForeignKeyInBookingEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Bookings",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "df1c3a0f-1234-4cde-bb55-d5f15a6aabcd",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "02cd9c0e-97fa-4935-8d5a-74219935cc8c", "AQAAAAIAAYagAAAAECQKs/vh/HdIe6dyPZ0yYbAiyiUJUpZUWCAbjXIdp7S7FzPvwwrkPtOjLzJ6qQrDEg==", "f03b39c2-658b-48b9-ae8c-cdaab28c7aa8" });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("2a523913-dd8e-44d1-a95e-d343ab4d4080"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 27, 4, 21, 5, 420, DateTimeKind.Utc).AddTicks(9612));

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("7da78485-b70d-4770-84f8-152ed4d9ccee"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 27, 4, 21, 5, 420, DateTimeKind.Utc).AddTicks(9560));

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("eb003919-0478-4b33-a168-170c78a8750b"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 27, 4, 21, 5, 420, DateTimeKind.Utc).AddTicks(9721));

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_UserId",
                table: "Bookings",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_AspNetUsers_UserId",
                table: "Bookings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_AspNetUsers_UserId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_UserId",
                table: "Bookings");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

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
                column: "CreatedOn",
                value: new DateTime(2025, 7, 27, 4, 14, 52, 516, DateTimeKind.Utc).AddTicks(3039));

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("7da78485-b70d-4770-84f8-152ed4d9ccee"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 27, 4, 14, 52, 516, DateTimeKind.Utc).AddTicks(2997));

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("eb003919-0478-4b33-a168-170c78a8750b"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 27, 4, 14, 52, 523, DateTimeKind.Utc).AddTicks(9280));
        }
    }
}
