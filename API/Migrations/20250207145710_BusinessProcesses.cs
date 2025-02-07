using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class BusinessProcesses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Vehicles");

            migrationBuilder.AddColumn<int>(
                name: "VehicleStatusID",
                table: "Vehicles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "ConcurrencyStamp",
                table: "EmployeeRoles",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ApprovedA",
                table: "Customers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ApprovedB",
                table: "Customers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ApprovedC",
                table: "Customers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "RentalRequests",
                columns: table => new
                {
                    RentalRequestID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    VehicleID = table.Column<int>(type: "int", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    RequestStatus = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("RentalRequests_pk", x => x.RentalRequestID);
                    table.ForeignKey(
                        name: "RentalRequests_Customers",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "RentalRequests_Vehicles",
                        column: x => x.VehicleID,
                        principalTable: "Vehicles",
                        principalColumn: "VehicleID");
                });

            migrationBuilder.CreateTable(
                name: "VehicleStatuses",
                columns: table => new
                {
                    VehicleStatusID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("VehicleStatus_pk", x => x.VehicleStatusID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_VehicleStatusID",
                table: "Vehicles",
                column: "VehicleStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_RentalRequests_CustomerID",
                table: "RentalRequests",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_RentalRequests_VehicleID",
                table: "RentalRequests",
                column: "VehicleID");

            migrationBuilder.AddForeignKey(
                name: "Vehicles_VehicleStatus",
                table: "Vehicles",
                column: "VehicleStatusID",
                principalTable: "VehicleStatuses",
                principalColumn: "VehicleStatusID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "Vehicles_VehicleStatus",
                table: "Vehicles");

            migrationBuilder.DropTable(
                name: "RentalRequests");

            migrationBuilder.DropTable(
                name: "VehicleStatuses");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_VehicleStatusID",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "VehicleStatusID",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "ApprovedA",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "ApprovedB",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "ApprovedC",
                table: "Customers");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Vehicles",
                type: "varchar(20)",
                unicode: false,
                maxLength: 20,
                nullable: false,
                defaultValue: "OutOfService");

            migrationBuilder.AlterColumn<string>(
                name: "ConcurrencyStamp",
                table: "EmployeeRoles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);
        }
    }
}
