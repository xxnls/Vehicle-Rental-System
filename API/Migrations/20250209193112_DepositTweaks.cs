using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class DepositTweaks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Rentals",
                table: "Payments");

            migrationBuilder.AddColumn<decimal>(
                name: "DepositAmount",
                table: "Rentals",
                type: "money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DepositRefundAmount",
                table: "Rentals",
                type: "money",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DepositStatus",
                table: "Rentals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Pending");

            migrationBuilder.AlterColumn<int>(
                name: "RentID",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Rentals",
                table: "Payments",
                column: "RentID",
                principalTable: "Rentals",
                principalColumn: "RentalID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Rentals",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "DepositAmount",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "DepositRefundAmount",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "DepositStatus",
                table: "Rentals");

            migrationBuilder.AlterColumn<int>(
                name: "RentID",
                table: "Payments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Rentals",
                table: "Payments",
                column: "RentID",
                principalTable: "Rentals",
                principalColumn: "RentalID");
        }
    }
}
