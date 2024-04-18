using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Furni.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryTableAndSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    LastUpdatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedById", "CreatedOn", "DisplayOrder", "IsDeleted", "LastUpdatedById", "LastUpdatedOn", "Name" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2024, 4, 18, 21, 1, 49, 255, DateTimeKind.Local).AddTicks(4358), 1, false, null, null, "Cheers" },
                    { 2, null, new DateTime(2024, 4, 18, 21, 1, 49, 255, DateTimeKind.Local).AddTicks(4410), 2, false, null, null, "Couches" },
                    { 3, null, new DateTime(2024, 4, 18, 21, 1, 49, 255, DateTimeKind.Local).AddTicks(4413), 3, false, null, null, "Beds" },
                    { 4, null, new DateTime(2024, 4, 18, 21, 1, 49, 255, DateTimeKind.Local).AddTicks(4416), 4, false, null, null, "Tables" },
                    { 5, null, new DateTime(2024, 4, 18, 21, 1, 49, 255, DateTimeKind.Local).AddTicks(4418), 5, false, null, null, "Bed Rooms" },
                    { 6, null, new DateTime(2024, 4, 18, 21, 1, 49, 255, DateTimeKind.Local).AddTicks(4423), 6, false, null, null, "Children's Bedrooms" },
                    { 7, null, new DateTime(2024, 4, 18, 21, 1, 49, 255, DateTimeKind.Local).AddTicks(4425), 7, false, null, null, "Wordrobe" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_DisplayOrder",
                table: "Categories",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
