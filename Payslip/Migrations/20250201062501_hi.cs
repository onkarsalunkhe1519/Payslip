using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Payslip.Migrations
{
    /// <inheritdoc />
    public partial class hi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "EmployeeEarnings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "EmployeeDeductions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "EmployeeEarnings");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "EmployeeDeductions");
        }
    }
}
