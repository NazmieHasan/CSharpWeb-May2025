using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "df1c3a0f-1234-4cde-bb55-d5f15a6aabcd");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.CreateTable(
                name: "IdentityUser",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityUser", x => x.Id);
                });

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

            migrationBuilder.InsertData(
                table: "IdentityUser",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "df1c3a0f-1234-4cde-bb55-d5f15a6aabcd", 0, "01f7e1a5-8f49-439b-963d-8657898308b3", "admin@hotelsystem.com", true, false, null, "ADMIN@HOTELSYSTEM.COM", "ADMIN@HOTELSYSTEM.COM", "AQAAAAIAAYagAAAAEPeV9wasg+Y9fFyWJ0z6GP/58qe4hh0YRgz16mpIoBWFpeK3JjfsiJv76zXa2xKk5w==", null, false, "3b1482ec-c205-4a2e-8f2f-151a95c11b8d", false, "admin@hotelsystem.com" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IdentityUser");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "df1c3a0f-1234-4cde-bb55-d5f15a6aabcd", 0, "cd51afc8-a13c-4caa-89b3-894e5ab7138f", "admin@hotelsystem.com", true, false, null, "ADMIN@HOTELSYSTEM.COM", "ADMIN@HOTELSYSTEM.COM", "AQAAAAIAAYagAAAAEFUJw0HysDhFf/5nqyfv8oFw8xHw87DFAJ/CBxy++XmN79Hr874begSFhCgQ7L6vfA==", null, false, "a1aca26a-58c2-4daa-801b-0496ce23f1bf", false, "admin@hotelsystem.com" });

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
    }
}
