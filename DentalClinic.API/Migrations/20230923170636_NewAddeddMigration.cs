using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DentalClinic.API.Migrations
{
    /// <inheritdoc />
    public partial class NewAddeddMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4892d551-4596-4ecb-88a7-58e4ebc7a0c3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bfa6017b-b572-4b2a-851b-ba7dbd31ccbd");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "16c85514-446e-47a2-905a-563a60b93a90", null, "User", "USER" },
                    { "ce4cb387-543d-4306-a5b0-48a6b0156c83", null, "Administrator", "ADMINISTRATOR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "16c85514-446e-47a2-905a-563a60b93a90");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ce4cb387-543d-4306-a5b0-48a6b0156c83");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4892d551-4596-4ecb-88a7-58e4ebc7a0c3", null, "Administrator", "ADMINISTRATOR" },
                    { "bfa6017b-b572-4b2a-851b-ba7dbd31ccbd", null, "User", "USER" }
                });
        }
    }
}
