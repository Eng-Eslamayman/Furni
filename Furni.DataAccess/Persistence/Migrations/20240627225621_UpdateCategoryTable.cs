using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Furni.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCategoryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Categories_DisplayOrder",
                table: "Categories");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedOn", "DisplayOrder", "ImageThumbnailUrl", "ImageUrl", "Name" },
                values: new object[] { new DateTime(2024, 6, 28, 1, 56, 18, 419, DateTimeKind.Local).AddTicks(4908), 3, "/images/categories/thumb/3b61ac7e-4157-4e65-8409-4fd0c5b0e8ae.jpg", "/images/categories/3b61ac7e-4157-4e65-8409-4fd0c5b0e8ae.jpg", "CHAIRS" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedOn", "DisplayOrder", "Name" },
                values: new object[] { new DateTime(2024, 6, 28, 1, 56, 18, 419, DateTimeKind.Local).AddTicks(5081), 7, "COUCHES" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedOn", "DisplayOrder", "ImageThumbnailUrl", "ImageUrl", "Name" },
                values: new object[] { new DateTime(2024, 6, 28, 1, 56, 18, 419, DateTimeKind.Local).AddTicks(5088), 6, "/images/categories/thumb/3adc43a5-d623-4d97-8693-8241bdc80e1d.jpg", "/images/categories/3adc43a5-d623-4d97-8693-8241bdc80e1d.jpg", "BEDS" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedOn", "DisplayOrder", "ImageThumbnailUrl", "ImageUrl", "Name" },
                values: new object[] { new DateTime(2024, 6, 28, 1, 56, 18, 419, DateTimeKind.Local).AddTicks(5093), 2, "/images/categories/thumb/f5bc5d01-6d9e-4b86-958a-4b5ec2bde6e9.jpg", "/images/categories/f5bc5d01-6d9e-4b86-958a-4b5ec2bde6e9.jpg", "TABLES" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedOn", "DisplayOrder" },
                values: new object[] { new DateTime(2024, 6, 28, 1, 56, 18, 419, DateTimeKind.Local).AddTicks(5097), 8 });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedOn", "DisplayOrder" },
                values: new object[] { new DateTime(2024, 6, 28, 1, 56, 18, 419, DateTimeKind.Local).AddTicks(5115), 9 });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedOn", "DisplayOrder", "ImageThumbnailUrl", "ImageUrl", "Name" },
                values: new object[] { new DateTime(2024, 6, 28, 1, 56, 18, 419, DateTimeKind.Local).AddTicks(5123), 5, "/images/categories/thumb/999eae65-2149-4fee-8687-b7356b67b06a.jpg", "/images/categories/999eae65-2149-4fee-8687-b7356b67b06a.jpg", "WARDROBES" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedById", "CreatedOn", "DisplayOrder", "ImageThumbnailUrl", "ImageUrl", "IsDeleted", "LastUpdatedById", "LastUpdatedOn", "Name" },
                values: new object[,]
                {
                    { 8, null, new DateTime(2024, 6, 28, 1, 56, 18, 419, DateTimeKind.Local).AddTicks(5127), 1, "/images/categories/thumb/4bb63bbc-4b87-42ea-b0c6-5752153b8491.jpg", "/images/categories/4bb63bbc-4b87-42ea-b0c6-5752153b8491.jpg", false, null, null, "SOFA" },
                    { 9, null, new DateTime(2024, 6, 28, 1, 56, 18, 419, DateTimeKind.Local).AddTicks(5130), 4, "/images/categories/thumb/104e9c53-b0ca-44e8-bcf3-540a3ceab9eb.jpg", "/images/categories/104e9c53-b0ca-44e8-bcf3-540a3ceab9eb.jpg", false, null, null, "LIGHTS" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_DisplayOrder",
                table: "Categories",
                column: "DisplayOrder",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Categories_DisplayOrder",
                table: "Categories");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedOn", "DisplayOrder", "ImageThumbnailUrl", "ImageUrl", "Name" },
                values: new object[] { new DateTime(2024, 6, 26, 6, 18, 1, 883, DateTimeKind.Local).AddTicks(8256), 1, "", "", "Cheers" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedOn", "DisplayOrder", "Name" },
                values: new object[] { new DateTime(2024, 6, 26, 6, 18, 1, 883, DateTimeKind.Local).AddTicks(8316), 2, "Couches" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedOn", "DisplayOrder", "ImageThumbnailUrl", "ImageUrl", "Name" },
                values: new object[] { new DateTime(2024, 6, 26, 6, 18, 1, 883, DateTimeKind.Local).AddTicks(8319), 3, "", "", "Beds" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedOn", "DisplayOrder", "ImageThumbnailUrl", "ImageUrl", "Name" },
                values: new object[] { new DateTime(2024, 6, 26, 6, 18, 1, 883, DateTimeKind.Local).AddTicks(8322), 4, "", "", "Tables" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedOn", "DisplayOrder" },
                values: new object[] { new DateTime(2024, 6, 26, 6, 18, 1, 883, DateTimeKind.Local).AddTicks(8325), 5 });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedOn", "DisplayOrder" },
                values: new object[] { new DateTime(2024, 6, 26, 6, 18, 1, 883, DateTimeKind.Local).AddTicks(8329), 6 });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedOn", "DisplayOrder", "ImageThumbnailUrl", "ImageUrl", "Name" },
                values: new object[] { new DateTime(2024, 6, 26, 6, 18, 1, 883, DateTimeKind.Local).AddTicks(8332), 7, "", "", "Wordrobe" });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_DisplayOrder",
                table: "Categories",
                column: "DisplayOrder");
        }
    }
}
