using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Furni.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddRangeToRatingInReviewTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Rating",
                table: "Reviews",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "int");

			migrationBuilder.Sql(@"ALTER TABLE dbo.Reviews
                      ADD CONSTRAINT CK_Rating_Range_New CHECK (Rating >= 1 AND Rating <= 5);");

			migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2024, 7, 14, 20, 15, 50, 571, DateTimeKind.Local).AddTicks(331));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2024, 7, 14, 20, 15, 50, 571, DateTimeKind.Local).AddTicks(381));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedOn",
                value: new DateTime(2024, 7, 14, 20, 15, 50, 571, DateTimeKind.Local).AddTicks(385));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedOn",
                value: new DateTime(2024, 7, 14, 20, 15, 50, 571, DateTimeKind.Local).AddTicks(388));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedOn",
                value: new DateTime(2024, 7, 14, 20, 15, 50, 571, DateTimeKind.Local).AddTicks(390));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedOn",
                value: new DateTime(2024, 7, 14, 20, 15, 50, 571, DateTimeKind.Local).AddTicks(395));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedOn",
                value: new DateTime(2024, 7, 14, 20, 15, 50, 571, DateTimeKind.Local).AddTicks(398));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedOn",
                value: new DateTime(2024, 7, 14, 20, 15, 50, 571, DateTimeKind.Local).AddTicks(401));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedOn",
                value: new DateTime(2024, 7, 14, 20, 15, 50, 571, DateTimeKind.Local).AddTicks(403));

            migrationBuilder.AddCheckConstraint(
                name: "CK_Rating_Range",
                table: "Reviews",
                sql: "[Rating] >= 1 AND [Rating] <= 5");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Rating_Range",
                table: "Reviews");

            migrationBuilder.AlterColumn<int>(
                name: "Rating",
                table: "Reviews",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 1);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2024, 7, 14, 19, 9, 21, 692, DateTimeKind.Local).AddTicks(5230));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2024, 7, 14, 19, 9, 21, 692, DateTimeKind.Local).AddTicks(5362));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedOn",
                value: new DateTime(2024, 7, 14, 19, 9, 21, 692, DateTimeKind.Local).AddTicks(5366));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedOn",
                value: new DateTime(2024, 7, 14, 19, 9, 21, 692, DateTimeKind.Local).AddTicks(5369));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedOn",
                value: new DateTime(2024, 7, 14, 19, 9, 21, 692, DateTimeKind.Local).AddTicks(5371));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedOn",
                value: new DateTime(2024, 7, 14, 19, 9, 21, 692, DateTimeKind.Local).AddTicks(5377));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedOn",
                value: new DateTime(2024, 7, 14, 19, 9, 21, 692, DateTimeKind.Local).AddTicks(5379));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedOn",
                value: new DateTime(2024, 7, 14, 19, 9, 21, 692, DateTimeKind.Local).AddTicks(5382));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedOn",
                value: new DateTime(2024, 7, 14, 19, 9, 21, 692, DateTimeKind.Local).AddTicks(5385));
        }
    }
}
