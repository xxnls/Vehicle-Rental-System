using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class AddedApprovedByEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApprovedByEmployeeID",
                table: "RentalRequests",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RentalRequests_ApprovedByEmployeeID",
                table: "RentalRequests",
                column: "ApprovedByEmployeeID");

            migrationBuilder.AddForeignKey(
                name: "RentalRequests_Employees",
                table: "RentalRequests",
                column: "ApprovedByEmployeeID",
                principalTable: "Employees",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "RentalRequests_Employees",
                table: "RentalRequests");

            migrationBuilder.DropIndex(
                name: "IX_RentalRequests_ApprovedByEmployeeID",
                table: "RentalRequests");

            migrationBuilder.DropColumn(
                name: "ApprovedByEmployeeID",
                table: "RentalRequests");
        }
    }
}
