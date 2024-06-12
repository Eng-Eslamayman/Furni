using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Furni.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateShoppingCartTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "ShoppingCarts",
                newName: "Count");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2024, 6, 12, 23, 23, 43, 685, DateTimeKind.Local).AddTicks(4637));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2024, 6, 12, 23, 23, 43, 685, DateTimeKind.Local).AddTicks(4688));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedOn",
                value: new DateTime(2024, 6, 12, 23, 23, 43, 685, DateTimeKind.Local).AddTicks(4691));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedOn",
                value: new DateTime(2024, 6, 12, 23, 23, 43, 685, DateTimeKind.Local).AddTicks(4694));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedOn",
                value: new DateTime(2024, 6, 12, 23, 23, 43, 685, DateTimeKind.Local).AddTicks(4697));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedOn",
                value: new DateTime(2024, 6, 12, 23, 23, 43, 685, DateTimeKind.Local).AddTicks(4701));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedOn",
                value: new DateTime(2024, 6, 12, 23, 23, 43, 685, DateTimeKind.Local).AddTicks(4753));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Count",
                table: "ShoppingCarts",
                newName: "Quantity");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2024, 6, 11, 21, 42, 31, 835, DateTimeKind.Local).AddTicks(5482));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2024, 6, 11, 21, 42, 31, 835, DateTimeKind.Local).AddTicks(5669));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedOn",
                value: new DateTime(2024, 6, 11, 21, 42, 31, 835, DateTimeKind.Local).AddTicks(5673));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedOn",
                value: new DateTime(2024, 6, 11, 21, 42, 31, 835, DateTimeKind.Local).AddTicks(5676));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedOn",
                value: new DateTime(2024, 6, 11, 21, 42, 31, 835, DateTimeKind.Local).AddTicks(5678));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedOn",
                value: new DateTime(2024, 6, 11, 21, 42, 31, 835, DateTimeKind.Local).AddTicks(5683));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedOn",
                value: new DateTime(2024, 6, 11, 21, 42, 31, 835, DateTimeKind.Local).AddTicks(5685));
        }
    }
}
