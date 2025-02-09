using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class AddedModifieByEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ApprovedByEmployeeID",
                table: "RentalRequests",
                newName: "ModifiedByEmployeeID");

            migrationBuilder.RenameIndex(
                name: "IX_RentalRequests_ApprovedByEmployeeID",
                table: "RentalRequests",
                newName: "IX_RentalRequests_ModifiedByEmployeeID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ModifiedByEmployeeID",
                table: "RentalRequests",
                newName: "ApprovedByEmployeeID");

            migrationBuilder.RenameIndex(
                name: "IX_RentalRequests_ModifiedByEmployeeID",
                table: "RentalRequests",
                newName: "IX_RentalRequests_ApprovedByEmployeeID");
        }
    }
}
