using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Payslip.Migrations
{
    /// <inheritdoc />
    public partial class mgr1151 : Migration
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

            migrationBuilder.DropColumn(
                name: "DeductionsId",
                table: "EmployeeSalaries");

            migrationBuilder.RenameColumn(
                name: "EarningsId1",
                table: "EmployeeSalaries",
                newName: "DeductionsDeductionId");

            migrationBuilder.RenameIndex(
                name: "IX_EmployeeSalaries_EarningsId1",
                table: "EmployeeSalaries",
                newName: "IX_EmployeeSalaries_DeductionsDeductionId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSalaries_EarningsId",
                table: "EmployeeSalaries",
                column: "EarningsId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeSalaries_Deduction_DeductionsDeductionId",
                table: "EmployeeSalaries",
                column: "DeductionsDeductionId",
                principalTable: "Deduction",
                principalColumn: "DeductionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeSalaries_Earning_EarningsId",
                table: "EmployeeSalaries",
                column: "EarningsId",
                principalTable: "Earning",
                principalColumn: "EarningsId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeSalaries_Deduction_DeductionsDeductionId",
                table: "EmployeeSalaries");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeSalaries_Earning_EarningsId",
                table: "EmployeeSalaries");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeSalaries_EarningsId",
                table: "EmployeeSalaries");

            migrationBuilder.RenameColumn(
                name: "DeductionsDeductionId",
                table: "EmployeeSalaries",
                newName: "EarningsId1");

            migrationBuilder.RenameIndex(
                name: "IX_EmployeeSalaries_DeductionsDeductionId",
                table: "EmployeeSalaries",
                newName: "IX_EmployeeSalaries_EarningsId1");

            migrationBuilder.AddColumn<int>(
                name: "DeductionsId",
                table: "EmployeeSalaries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSalaries_DeductionsId",
                table: "EmployeeSalaries",
                column: "DeductionsId");

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
