using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class RentalRequestsUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RequestStatus",
                table: "RentalRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Pending",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "RentalRequests",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "(getdate())");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "RentalRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "RentalRequests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "RentalRequests",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "RentalRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "RentalRequests",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentStatus",
                table: "RentalRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Pending");

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "RentalRequests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "TotalCost",
                table: "RentalRequests",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "RentalRequests");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "RentalRequests");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "RentalRequests");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "RentalRequests");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "RentalRequests");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "RentalRequests");

            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "RentalRequests");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "RentalRequests");

            migrationBuilder.DropColumn(
                name: "TotalCost",
                table: "RentalRequests");

            migrationBuilder.AlterColumn<string>(
                name: "RequestStatus",
                table: "RentalRequests",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "Pending");
        }
    }
}
