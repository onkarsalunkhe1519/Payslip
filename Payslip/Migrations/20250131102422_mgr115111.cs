using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Payslip.Migrations
{
    /// <inheritdoc />
    public partial class mgr115111 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeSalaries_Deduction_DeductionsId",
                table: "EmployeeSalaries");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeSalaries_Earning_EarningsId1",
                table: "EmployeeSalaries");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeSalaries_DeductionsId",
                table: "EmployeeSalaries");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeSalaries_EarningsId1",
                table: "EmployeeSalaries");

            migrationBuilder.DropColumn(
                name: "DeductionsId",
                table: "EmployeeSalaries");

            migrationBuilder.DropColumn(
                name: "EarningsId",
                table: "EmployeeSalaries");

            migrationBuilder.DropColumn(
                name: "EarningsId1",
                table: "EmployeeSalaries");

            migrationBuilder.CreateTable(
                name: "EmployeeDeductions",
                columns: table => new
                {
                    EmployeeDeductionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SalaryId = table.Column<int>(type: "int", nullable: false),
                    DeductionId = table.Column<int>(type: "int", nullable: false),
                    DeductionAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeDeductions", x => x.EmployeeDeductionId);
                    table.ForeignKey(
                        name: "FK_EmployeeDeductions_Deduction_DeductionId",
                        column: x => x.DeductionId,
                        principalTable: "Deduction",
                        principalColumn: "DeductionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeDeductions_EmployeeSalaries_SalaryId",
                        column: x => x.SalaryId,
                        principalTable: "EmployeeSalaries",
                        principalColumn: "SalaryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeEarnings",
                columns: table => new
                {
                    EmployeeEarningId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SalaryId = table.Column<int>(type: "int", nullable: false),
                    EarningId = table.Column<int>(type: "int", nullable: false),
                    EarningAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeEarnings", x => x.EmployeeEarningId);
                    table.ForeignKey(
                        name: "FK_EmployeeEarnings_Earning_EarningId",
                        column: x => x.EarningId,
                        principalTable: "Earning",
                        principalColumn: "EarningsId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeEarnings_EmployeeSalaries_SalaryId",
                        column: x => x.SalaryId,
                        principalTable: "EmployeeSalaries",
                        principalColumn: "SalaryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDeductions_DeductionId",
                table: "EmployeeDeductions",
                column: "DeductionId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDeductions_SalaryId",
                table: "EmployeeDeductions",
                column: "SalaryId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEarnings_EarningId",
                table: "EmployeeEarnings",
                column: "EarningId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEarnings_SalaryId",
                table: "EmployeeEarnings",
                column: "SalaryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeDeductions");

            migrationBuilder.DropTable(
                name: "EmployeeEarnings");

            migrationBuilder.AddColumn<int>(
                name: "DeductionsId",
                table: "EmployeeSalaries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EarningsId",
                table: "EmployeeSalaries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EarningsId1",
                table: "EmployeeSalaries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSalaries_DeductionsId",
                table: "EmployeeSalaries",
                column: "DeductionsId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSalaries_EarningsId1",
                table: "EmployeeSalaries",
                column: "EarningsId1");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeSalaries_Deduction_DeductionsId",
                table: "EmployeeSalaries",
                column: "DeductionsId",
                principalTable: "Deduction",
                principalColumn: "DeductionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeSalaries_Earning_EarningsId1",
                table: "EmployeeSalaries",
                column: "EarningsId1",
                principalTable: "Earning",
                principalColumn: "EarningsId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
