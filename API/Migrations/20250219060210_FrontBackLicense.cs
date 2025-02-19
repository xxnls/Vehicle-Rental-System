using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class FrontBackLicense : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "LicenseApprovalRequests_Documents",
                table: "LicenseApprovalRequests");

            migrationBuilder.DropIndex(
                name: "IX_LicenseApprovalRequests_DocumentID",
                table: "LicenseApprovalRequests");

            migrationBuilder.DropColumn(
                name: "DocumentID",
                table: "LicenseApprovalRequests");

            migrationBuilder.AddColumn<int>(
                name: "DocumentBackID",
                table: "LicenseApprovalRequests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DocumentFrontID",
                table: "LicenseApprovalRequests",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LicenseApprovalRequests_DocumentBackID",
                table: "LicenseApprovalRequests",
                column: "DocumentBackID");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseApprovalRequests_DocumentFrontID",
                table: "LicenseApprovalRequests",
                column: "DocumentFrontID");

            migrationBuilder.AddForeignKey(
                name: "LicenseApprovalRequests_DocumentsBack",
                table: "LicenseApprovalRequests",
                column: "DocumentBackID",
                principalTable: "Documents",
                principalColumn: "DocumentID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "LicenseApprovalRequests_DocumentsFront",
                table: "LicenseApprovalRequests",
                column: "DocumentFrontID",
                principalTable: "Documents",
                principalColumn: "DocumentID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "LicenseApprovalRequests_DocumentsBack",
                table: "LicenseApprovalRequests");

            migrationBuilder.DropForeignKey(
                name: "LicenseApprovalRequests_DocumentsFront",
                table: "LicenseApprovalRequests");

            migrationBuilder.DropIndex(
                name: "IX_LicenseApprovalRequests_DocumentBackID",
                table: "LicenseApprovalRequests");

            migrationBuilder.DropIndex(
                name: "IX_LicenseApprovalRequests_DocumentFrontID",
                table: "LicenseApprovalRequests");

            migrationBuilder.DropColumn(
                name: "DocumentBackID",
                table: "LicenseApprovalRequests");

            migrationBuilder.DropColumn(
                name: "DocumentFrontID",
                table: "LicenseApprovalRequests");

            migrationBuilder.AddColumn<int>(
                name: "DocumentID",
                table: "LicenseApprovalRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_LicenseApprovalRequests_DocumentID",
                table: "LicenseApprovalRequests",
                column: "DocumentID");

            migrationBuilder.AddForeignKey(
                name: "LicenseApprovalRequests_Documents",
                table: "LicenseApprovalRequests",
                column: "DocumentID",
                principalTable: "Documents",
                principalColumn: "DocumentID");
        }
    }
}
