using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class RentalTweaks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ActualStartDate",
                table: "Rentals",
                newName: "PickupDateTime");

            migrationBuilder.RenameColumn(
                name: "ActualEndDate",
                table: "Rentals",
                newName: "FinishDateTime");

            migrationBuilder.RenameColumn(
                name: "ActualCost",
                table: "Rentals",
                newName: "FinalCost");

            migrationBuilder.AddColumn<string>(
                name: "RentalStatus",
                table: "Rentals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "AwaitingPickup");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RentalStatus",
                table: "Rentals");

            migrationBuilder.RenameColumn(
                name: "PickupDateTime",
                table: "Rentals",
                newName: "ActualStartDate");

            migrationBuilder.RenameColumn(
                name: "FinishDateTime",
                table: "Rentals",
                newName: "ActualEndDate");

            migrationBuilder.RenameColumn(
                name: "FinalCost",
                table: "Rentals",
                newName: "ActualCost");
        }
    }
}
