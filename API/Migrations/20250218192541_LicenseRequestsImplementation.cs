using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class LicenseRequestsImplementation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LicenseApprovalRequests",
                columns: table => new
                {
                    LicenseApprovalRequestID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    ApprovedByEmployeeID = table.Column<int>(type: "int", nullable: true),
                    DocumentID = table.Column<int>(type: "int", nullable: false),
                    LicenseType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "B"),
                    RequestStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Pending"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("LicenseApprovalRequests_pk", x => x.LicenseApprovalRequestID);
                    table.ForeignKey(
                        name: "LicenseApprovalRequests_Customers",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "LicenseApprovalRequests_Documents",
                        column: x => x.DocumentID,
                        principalTable: "Documents",
                        principalColumn: "DocumentID");
                    table.ForeignKey(
                        name: "LicenseApprovalRequests_Employees",
                        column: x => x.ApprovedByEmployeeID,
                        principalTable: "Employees",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_LicenseApprovalRequests_ApprovedByEmployeeID",
                table: "LicenseApprovalRequests",
                column: "ApprovedByEmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseApprovalRequests_CustomerID",
                table: "LicenseApprovalRequests",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseApprovalRequests_DocumentID",
                table: "LicenseApprovalRequests",
                column: "DocumentID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LicenseApprovalRequests");
        }
    }
}
