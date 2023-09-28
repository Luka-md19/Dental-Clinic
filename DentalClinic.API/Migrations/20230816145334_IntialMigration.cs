using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DentalClinic.API.Migrations
{
    /// <inheritdoc />
    public partial class IntialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Specialization",
                table: "Dentists",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Dentists",
                keyColumn: "Id",
                keyValue: 1,
                column: "Specialization",
                value: null);

            migrationBuilder.UpdateData(
                table: "Dentists",
                keyColumn: "Id",
                keyValue: 2,
                column: "Specialization",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Specialization",
                table: "Dentists");
        }
    }
}
