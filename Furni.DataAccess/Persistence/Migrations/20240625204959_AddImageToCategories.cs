using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Furni.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddImageToCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageThumbnailUrl",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedOn", "ImageThumbnailUrl", "ImageUrl" },
                values: new object[] { new DateTime(2024, 6, 25, 23, 49, 58, 144, DateTimeKind.Local).AddTicks(8981), "", "" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedOn", "ImageThumbnailUrl", "ImageUrl" },
                values: new object[] { new DateTime(2024, 6, 25, 23, 49, 58, 144, DateTimeKind.Local).AddTicks(9036), "", "" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedOn", "ImageThumbnailUrl", "ImageUrl" },
                values: new object[] { new DateTime(2024, 6, 25, 23, 49, 58, 144, DateTimeKind.Local).AddTicks(9040), "", "" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedOn", "ImageThumbnailUrl", "ImageUrl" },
                values: new object[] { new DateTime(2024, 6, 25, 23, 49, 58, 144, DateTimeKind.Local).AddTicks(9043), "", "" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedOn", "ImageThumbnailUrl", "ImageUrl" },
                values: new object[] { new DateTime(2024, 6, 25, 23, 49, 58, 144, DateTimeKind.Local).AddTicks(9046), "", "" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedOn", "ImageThumbnailUrl", "ImageUrl" },
                values: new object[] { new DateTime(2024, 6, 25, 23, 49, 58, 144, DateTimeKind.Local).AddTicks(9052), "", "" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedOn", "ImageThumbnailUrl", "ImageUrl" },
                values: new object[] { new DateTime(2024, 6, 25, 23, 49, 58, 144, DateTimeKind.Local).AddTicks(9054), "", "" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageThumbnailUrl",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Categories");

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
    }
}
