using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Payslip.Migrations
{
    /// <inheritdoc />
    public partial class hi11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BasicSalary",
                table: "EmployeeSalaries",
                newName: "TotalSalary");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalSalary",
                table: "EmployeeSalaries",
                newName: "BasicSalary");
        }
    }
}
