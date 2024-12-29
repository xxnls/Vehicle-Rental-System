using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class EmployeeFinancesFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "EmployeeFinances_Employees_ApprovedByID",
                table: "EmployeeFinances");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeFinances_ApprovedByID",
                table: "EmployeeFinances");

            migrationBuilder.DropColumn(
                name: "Allowances",
                table: "EmployeeFinances");

            migrationBuilder.DropColumn(
                name: "ApprovedByID",
                table: "EmployeeFinances");

            migrationBuilder.DropColumn(
                name: "Bonuses",
                table: "EmployeeFinances");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "EmployeeFinances");

            migrationBuilder.DropColumn(
                name: "Deductions",
                table: "EmployeeFinances");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "EmployeeFinances");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "EmployeeFinances");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "EmployeeFinances");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "EmployeeFinances");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "EmployeeFinances",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeFinances_EmployeeId",
                table: "EmployeeFinances",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeFinances_Employees_EmployeeId",
                table: "EmployeeFinances",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeFinances_Employees_EmployeeId",
                table: "EmployeeFinances");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeFinances_EmployeeId",
                table: "EmployeeFinances");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "EmployeeFinances");

            migrationBuilder.AddColumn<decimal>(
                name: "Allowances",
                table: "EmployeeFinances",
                type: "money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "ApprovedByID",
                table: "EmployeeFinances",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Bonuses",
                table: "EmployeeFinances",
                type: "money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "EmployeeFinances",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "(getdate())");

            migrationBuilder.AddColumn<decimal>(
                name: "Deductions",
                table: "EmployeeFinances",
                type: "money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "EmployeeFinances",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "EmployeeFinances",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "EmployeeFinances",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "EmployeeFinances",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeFinances_ApprovedByID",
                table: "EmployeeFinances",
                column: "ApprovedByID");

            migrationBuilder.AddForeignKey(
                name: "EmployeeFinances_Employees_ApprovedByID",
                table: "EmployeeFinances",
                column: "ApprovedByID",
                principalTable: "Employees",
                principalColumn: "Id");
        }
    }
}
