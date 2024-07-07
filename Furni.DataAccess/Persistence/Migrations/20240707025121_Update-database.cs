using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Furni.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Updatedatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2024, 7, 7, 5, 51, 20, 276, DateTimeKind.Local).AddTicks(4281));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2024, 7, 7, 5, 51, 20, 276, DateTimeKind.Local).AddTicks(4356));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedOn",
                value: new DateTime(2024, 7, 7, 5, 51, 20, 276, DateTimeKind.Local).AddTicks(4361));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedOn",
                value: new DateTime(2024, 7, 7, 5, 51, 20, 276, DateTimeKind.Local).AddTicks(4365));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedOn",
                value: new DateTime(2024, 7, 7, 5, 51, 20, 276, DateTimeKind.Local).AddTicks(4370));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedOn",
                value: new DateTime(2024, 7, 7, 5, 51, 20, 276, DateTimeKind.Local).AddTicks(4378));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedOn",
                value: new DateTime(2024, 7, 7, 5, 51, 20, 276, DateTimeKind.Local).AddTicks(4382));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedOn",
                value: new DateTime(2024, 7, 7, 5, 51, 20, 276, DateTimeKind.Local).AddTicks(4386));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedOn",
                value: new DateTime(2024, 7, 7, 5, 51, 20, 276, DateTimeKind.Local).AddTicks(4390));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2024, 6, 28, 1, 56, 18, 419, DateTimeKind.Local).AddTicks(4908));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2024, 6, 28, 1, 56, 18, 419, DateTimeKind.Local).AddTicks(5081));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedOn",
                value: new DateTime(2024, 6, 28, 1, 56, 18, 419, DateTimeKind.Local).AddTicks(5088));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedOn",
                value: new DateTime(2024, 6, 28, 1, 56, 18, 419, DateTimeKind.Local).AddTicks(5093));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedOn",
                value: new DateTime(2024, 6, 28, 1, 56, 18, 419, DateTimeKind.Local).AddTicks(5097));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedOn",
                value: new DateTime(2024, 6, 28, 1, 56, 18, 419, DateTimeKind.Local).AddTicks(5115));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedOn",
                value: new DateTime(2024, 6, 28, 1, 56, 18, 419, DateTimeKind.Local).AddTicks(5123));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedOn",
                value: new DateTime(2024, 6, 28, 1, 56, 18, 419, DateTimeKind.Local).AddTicks(5127));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedOn",
                value: new DateTime(2024, 6, 28, 1, 56, 18, 419, DateTimeKind.Local).AddTicks(5130));
        }
    }
}
