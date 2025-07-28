using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameManagerEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Manager_ManagerId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Manager_AspNetUsers_UserId",
                table: "Manager");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Manager",
                table: "Manager");

            migrationBuilder.DropIndex(
                name: "IX_Manager_UserId",
                table: "Manager");

            migrationBuilder.RenameTable(
                name: "Manager",
                newName: "Managers");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Managers",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Managers",
                table: "Managers",
                column: "Id");

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
                column: "CreatedOn",
                value: new DateTime(2025, 7, 28, 22, 58, 34, 74, DateTimeKind.Utc).AddTicks(7764));

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("7da78485-b70d-4770-84f8-152ed4d9ccee"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 28, 22, 58, 34, 74, DateTimeKind.Utc).AddTicks(7714));

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("eb003919-0478-4b33-a168-170c78a8750b"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 28, 22, 58, 34, 74, DateTimeKind.Utc).AddTicks(7776));

            migrationBuilder.CreateIndex(
                name: "IX_Managers_UserId",
                table: "Managers",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Managers_ManagerId",
                table: "Bookings",
                column: "ManagerId",
                principalTable: "Managers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Managers_AspNetUsers_UserId",
                table: "Managers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Managers_ManagerId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Managers_AspNetUsers_UserId",
                table: "Managers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Managers",
                table: "Managers");

            migrationBuilder.DropIndex(
                name: "IX_Managers_UserId",
                table: "Managers");

            migrationBuilder.RenameTable(
                name: "Managers",
                newName: "Manager");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Manager",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Manager",
                table: "Manager",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "df1c3a0f-1234-4cde-bb55-d5f15a6aabcd",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2ae82bb1-1c53-4fe8-9160-92e14658ba8f", "AQAAAAIAAYagAAAAEBIiaIKm3j+0pqj5w0+KwktDGzf580usEF5xWH3nQZknORSeGuoRmP7s18N6YmVE8A==", "a6c20c9f-78e4-4c90-907a-b776e82587d6" });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("2a523913-dd8e-44d1-a95e-d343ab4d4080"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 28, 21, 24, 4, 561, DateTimeKind.Utc).AddTicks(2515));

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("7da78485-b70d-4770-84f8-152ed4d9ccee"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 28, 21, 24, 4, 561, DateTimeKind.Utc).AddTicks(2442));

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("eb003919-0478-4b33-a168-170c78a8750b"),
                column: "CreatedOn",
                value: new DateTime(2025, 7, 28, 21, 24, 4, 561, DateTimeKind.Utc).AddTicks(2536));

            migrationBuilder.CreateIndex(
                name: "IX_Manager_UserId",
                table: "Manager",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Manager_ManagerId",
                table: "Bookings",
                column: "ManagerId",
                principalTable: "Manager",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Manager_AspNetUsers_UserId",
                table: "Manager",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
