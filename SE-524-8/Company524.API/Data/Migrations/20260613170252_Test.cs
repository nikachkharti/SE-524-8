using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Company524.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class Test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-0000-0000-0000-000000000001"),
                column: "LastLoginDate",
                value: new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-0000-0000-0000-000000000002"),
                column: "LastLoginDate",
                value: new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-0000-0000-0000-000000000003"),
                column: "LastLoginDate",
                value: new DateTime(2026, 6, 13, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-0000-0000-0000-000000000004"),
                column: "LastLoginDate",
                value: new DateTime(2026, 6, 3, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-0000-0000-0000-000000000005"),
                column: "LastLoginDate",
                value: new DateTime(2026, 6, 10, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: new Guid("55555555-0000-0000-0000-000000000001"),
                column: "OrderDate",
                value: new DateTime(2026, 6, 9, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: new Guid("55555555-0000-0000-0000-000000000002"),
                column: "OrderDate",
                value: new DateTime(2026, 6, 12, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: new Guid("55555555-0000-0000-0000-000000000003"),
                column: "OrderDate",
                value: new DateTime(2026, 6, 13, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: new Guid("55555555-0000-0000-0000-000000000004"),
                column: "OrderDate",
                value: new DateTime(2026, 6, 10, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: new Guid("55555555-0000-0000-0000-000000000005"),
                column: "OrderDate",
                value: new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: new Guid("55555555-0000-0000-0000-000000000006"),
                column: "OrderDate",
                value: new DateTime(2026, 6, 7, 0, 0, 0, 0, DateTimeKind.Local));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-0000-0000-0000-000000000001"),
                column: "LastLoginDate",
                value: new DateTime(2026, 6, 6, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-0000-0000-0000-000000000002"),
                column: "LastLoginDate",
                value: new DateTime(2026, 6, 9, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-0000-0000-0000-000000000003"),
                column: "LastLoginDate",
                value: new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-0000-0000-0000-000000000004"),
                column: "LastLoginDate",
                value: new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-0000-0000-0000-000000000005"),
                column: "LastLoginDate",
                value: new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: new Guid("55555555-0000-0000-0000-000000000001"),
                column: "OrderDate",
                value: new DateTime(2026, 6, 7, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: new Guid("55555555-0000-0000-0000-000000000002"),
                column: "OrderDate",
                value: new DateTime(2026, 6, 10, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: new Guid("55555555-0000-0000-0000-000000000003"),
                column: "OrderDate",
                value: new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: new Guid("55555555-0000-0000-0000-000000000004"),
                column: "OrderDate",
                value: new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: new Guid("55555555-0000-0000-0000-000000000005"),
                column: "OrderDate",
                value: new DateTime(2026, 6, 9, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: new Guid("55555555-0000-0000-0000-000000000006"),
                column: "OrderDate",
                value: new DateTime(2026, 6, 5, 0, 0, 0, 0, DateTimeKind.Local));
        }
    }
}
