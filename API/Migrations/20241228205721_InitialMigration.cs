using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    CountryID = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Abbreviation = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: false),
                    DialingCode = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Countries_pk", x => x.CountryID);
                });

            migrationBuilder.CreateTable(
                name: "CustomerStatistics",
                columns: table => new
                {
                    CustomerStatisticsID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalRentals = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    ActiveRentals = table.Column<short>(type: "smallint", nullable: false),
                    CanceledRentals = table.Column<int>(type: "int", nullable: false),
                    FirstRentalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastRentalDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("CustomerStatistics_pk", x => x.CustomerStatisticsID);
                });

            migrationBuilder.CreateTable(
                name: "CustomerTypes",
                columns: table => new
                {
                    CustomerTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MaxRentals = table.Column<short>(type: "smallint", nullable: true),
                    DiscountPercent = table.Column<double>(type: "float", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("CustomerTypes_pk", x => x.CustomerTypeID);
                });

            migrationBuilder.CreateTable(
                name: "DocumentCategories",
                columns: table => new
                {
                    DocumentCategoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentCategoryID = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("DocumentCategories_pk", x => x.DocumentCategoryID);
                    table.ForeignKey(
                        name: "DocumentCategories_DocumentCategories",
                        column: x => x.ParentCategoryID,
                        principalTable: "DocumentCategories",
                        principalColumn: "DocumentCategoryID");
                });

            migrationBuilder.CreateTable(
                name: "DocumentTypes",
                columns: table => new
                {
                    DocumentTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    FileExtension = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MaxFileSizeMB = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("DocumentTypes_pk", x => x.DocumentTypeID);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeLeaveTypes",
                columns: table => new
                {
                    EmployeeLeaveTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    DefaultDays = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("EmployeeLeaveTypes_pk", x => x.EmployeeLeaveTypeID);
                });

            migrationBuilder.CreateTable(
                name: "EmployeePositions",
                columns: table => new
                {
                    EmployeePositionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DefaultBaseSalary = table.Column<decimal>(type: "money", nullable: true),
                    DefaultHourlyRate = table.Column<decimal>(type: "money", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("EmployeePositions_pk", x => x.EmployeePositionID);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeRoles",
                columns: table => new
                {
                    EmployeeRoleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RolePower = table.Column<int>(type: "int", nullable: false),
                    ManageVehicles = table.Column<bool>(type: "bit", nullable: false),
                    ManageEmployees = table.Column<bool>(type: "bit", nullable: false),
                    ManageRentals = table.Column<bool>(type: "bit", nullable: false),
                    ManageLeaves = table.Column<bool>(type: "bit", nullable: false),
                    ManageSchedule = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeRoles", x => x.EmployeeRoleID);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeShiftTypes",
                columns: table => new
                {
                    EmployeeShiftTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TimeStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("EmployeeShiftTypes_pk", x => x.EmployeeShiftTypeID);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeStatistics",
                columns: table => new
                {
                    EmployeeStatisticsID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalWorkDays = table.Column<int>(type: "int", nullable: false),
                    LateArrivals = table.Column<int>(type: "int", nullable: false),
                    EarlyDepartures = table.Column<int>(type: "int", nullable: false),
                    OvertimeHours = table.Column<double>(type: "float", nullable: false),
                    SickLeavesTaken = table.Column<int>(type: "int", nullable: false),
                    VacationDaysTaken = table.Column<int>(type: "int", nullable: false),
                    UnpaidLeavesTaken = table.Column<int>(type: "int", nullable: false),
                    TotalRentalsApproved = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("EmployeeStatistics_pk", x => x.EmployeeStatisticsID);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    LocationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleID = table.Column<int>(type: "int", nullable: true),
                    RentalPlaceID = table.Column<int>(type: "int", nullable: true),
                    GPSLatitude = table.Column<double>(type: "float", nullable: false),
                    GPSLongitude = table.Column<double>(type: "float", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Locations_pk", x => x.LocationID);
                });

            migrationBuilder.CreateTable(
                name: "NewsTypes",
                columns: table => new
                {
                    NewsTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Color = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    MaxContentSize = table.Column<int>(type: "int", nullable: false),
                    AllowImage = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("NewsTypes_pk", x => x.NewsTypeID);
                });

            migrationBuilder.CreateTable(
                name: "VehicleBrands",
                columns: table => new
                {
                    VehicleBrandID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LogoURL = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("VehicleBrands_pk", x => x.VehicleBrandID);
                });

            migrationBuilder.CreateTable(
                name: "VehicleOptionalInformationDto",
                columns: table => new
                {
                    VehicleOptionalInformationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HasNavigation = table.Column<bool>(type: "bit", nullable: false),
                    HasBluetooth = table.Column<bool>(type: "bit", nullable: false),
                    HasAirConditioning = table.Column<bool>(type: "bit", nullable: false),
                    HasAutomaticTransmission = table.Column<bool>(type: "bit", nullable: false),
                    HasParkingSensors = table.Column<bool>(type: "bit", nullable: false),
                    HasCruiseControl = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("VehicleOptionalInformation_pk", x => x.VehicleOptionalInformationID);
                });

            migrationBuilder.CreateTable(
                name: "VehicleStatistics",
                columns: table => new
                {
                    VehicleStatisticsID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalRentals = table.Column<int>(type: "int", nullable: false),
                    RentalRevenue = table.Column<decimal>(type: "money", nullable: false),
                    FirstRentalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastRentalDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("VehicleStatistics_pk", x => x.VehicleStatisticsID);
                });

            migrationBuilder.CreateTable(
                name: "VehicleTypes",
                columns: table => new
                {
                    VehicleTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    BaseDailyRate = table.Column<decimal>(type: "money", nullable: false),
                    BaseWeeklyRate = table.Column<decimal>(type: "money", nullable: false),
                    BaseDeposit = table.Column<decimal>(type: "money", nullable: false),
                    RequiredLicenseType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "B"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("VehicleTypes_pk", x => x.VehicleTypeID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    AddressID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryID = table.Column<short>(type: "smallint", nullable: false),
                    FirstLine = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SecondLine = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ZipCode = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Addresses_pk", x => x.AddressID);
                    table.ForeignKey(
                        name: "Addresses_Countries",
                        column: x => x.CountryID,
                        principalTable: "Countries",
                        principalColumn: "CountryID");
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_EmployeeRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "EmployeeRoles",
                        principalColumn: "EmployeeRoleID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_EmployeeRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "EmployeeRoles",
                        principalColumn: "EmployeeRoleID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VehicleModels",
                columns: table => new
                {
                    VehicleModelID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleBrandID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EngineSize = table.Column<double>(type: "float", nullable: true),
                    HorsePower = table.Column<int>(type: "int", nullable: true),
                    FuelType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("VehicleModels_pk", x => x.VehicleModelID);
                    table.ForeignKey(
                        name: "VehicleModels_VehicleBrands",
                        column: x => x.VehicleBrandID,
                        principalTable: "VehicleBrands",
                        principalColumn: "VehicleBrandID");
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    AddressID = table.Column<int>(type: "int", nullable: false),
                    CustomerTypeID = table.Column<int>(type: "int", nullable: false),
                    CustomerStatisticsID = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "Customers_Addresses",
                        column: x => x.AddressID,
                        principalTable: "Addresses",
                        principalColumn: "AddressID");
                    table.ForeignKey(
                        name: "Customers_CustomerStatistics",
                        column: x => x.CustomerStatisticsID,
                        principalTable: "CustomerStatistics",
                        principalColumn: "CustomerStatisticsID");
                    table.ForeignKey(
                        name: "Customers_CustomerTypes",
                        column: x => x.CustomerTypeID,
                        principalTable: "CustomerTypes",
                        principalColumn: "CustomerTypeID");
                    table.ForeignKey(
                        name: "FK_Customers_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RentalPlaces",
                columns: table => new
                {
                    RentalPlaceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocationID = table.Column<int>(type: "int", nullable: false),
                    AddressID = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("RentalPlaces_pk", x => x.RentalPlaceID);
                    table.ForeignKey(
                        name: "RentalPlaces_Addresses",
                        column: x => x.AddressID,
                        principalTable: "Addresses",
                        principalColumn: "AddressID");
                    table.ForeignKey(
                        name: "RentalPlaces_Locations",
                        column: x => x.LocationID,
                        principalTable: "Locations",
                        principalColumn: "LocationID");
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    VehicleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleModelID = table.Column<int>(type: "int", nullable: false),
                    VehicleTypeID = table.Column<int>(type: "int", nullable: false),
                    VehicleStatisticsID = table.Column<int>(type: "int", nullable: false),
                    VehicleOptionalInformationID = table.Column<int>(type: "int", nullable: false),
                    RentalPlaceID = table.Column<int>(type: "int", nullable: false),
                    LocationID = table.Column<int>(type: "int", nullable: false),
                    VIN = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    LicensePlate = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    Color = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ManufactureYear = table.Column<int>(type: "int", nullable: false),
                    CurrentMileage = table.Column<int>(type: "int", nullable: false),
                    LastMaintenanceMileage = table.Column<int>(type: "int", nullable: true),
                    LastMaintenanceDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NextMaintenanceDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PurchasePrice = table.Column<decimal>(type: "money", nullable: false),
                    Status = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: "OutOfService"),
                    CustomDailyRate = table.Column<decimal>(type: "money", nullable: true),
                    CustomWeeklyRate = table.Column<decimal>(type: "money", nullable: true),
                    CustomDeposit = table.Column<decimal>(type: "money", nullable: true),
                    IsAvailableForRent = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("VehicleID", x => x.VehicleID);
                    table.ForeignKey(
                        name: "Vehicles_Locations",
                        column: x => x.LocationID,
                        principalTable: "Locations",
                        principalColumn: "LocationID");
                    table.ForeignKey(
                        name: "Vehicles_RentalPlaces",
                        column: x => x.RentalPlaceID,
                        principalTable: "RentalPlaces",
                        principalColumn: "RentalPlaceID");
                    table.ForeignKey(
                        name: "Vehicles_VehicleModels",
                        column: x => x.VehicleModelID,
                        principalTable: "VehicleModels",
                        principalColumn: "VehicleModelID");
                    table.ForeignKey(
                        name: "Vehicles_VehicleOptionalInformation",
                        column: x => x.VehicleOptionalInformationID,
                        principalTable: "VehicleOptionalInformationDto",
                        principalColumn: "VehicleOptionalInformationID");
                    table.ForeignKey(
                        name: "Vehicles_VehicleStatistics",
                        column: x => x.VehicleStatisticsID,
                        principalTable: "VehicleStatistics",
                        principalColumn: "VehicleStatisticsID");
                    table.ForeignKey(
                        name: "Vehicles_VehicleTypes",
                        column: x => x.VehicleTypeID,
                        principalTable: "VehicleTypes",
                        principalColumn: "VehicleTypeID");
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    DocumentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentTypeID = table.Column<int>(type: "int", nullable: false),
                    DocumentCategoryID = table.Column<int>(type: "int", nullable: false),
                    VehicleID = table.Column<int>(type: "int", nullable: true),
                    EmployeeID = table.Column<int>(type: "int", nullable: true),
                    CustomerID = table.Column<int>(type: "int", nullable: true),
                    RentalPlaceID = table.Column<int>(type: "int", nullable: true),
                    RentalID = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    OriginalFileName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FileSizeMB = table.Column<double>(type: "float", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByEmployeeID = table.Column<int>(type: "int", nullable: false),
                    ModifiedByEmployeeID = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Documents_pk", x => x.DocumentID);
                    table.ForeignKey(
                        name: "Documents_Customers",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "Documents_DocumentCategories",
                        column: x => x.DocumentCategoryID,
                        principalTable: "DocumentCategories",
                        principalColumn: "DocumentCategoryID");
                    table.ForeignKey(
                        name: "Documents_DocumentTypes",
                        column: x => x.DocumentTypeID,
                        principalTable: "DocumentTypes",
                        principalColumn: "DocumentTypeID");
                    table.ForeignKey(
                        name: "Documents_RentalPlaces",
                        column: x => x.RentalPlaceID,
                        principalTable: "RentalPlaces",
                        principalColumn: "RentalPlaceID");
                    table.ForeignKey(
                        name: "Documents_Vehicles",
                        column: x => x.VehicleID,
                        principalTable: "Vehicles",
                        principalColumn: "VehicleID");
                });

            migrationBuilder.CreateTable(
                name: "EmployeeAttendance",
                columns: table => new
                {
                    EmployeeAttendanceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeID = table.Column<int>(type: "int", nullable: false),
                    CheckInTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    CheckOutTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TotalTime = table.Column<double>(type: "float", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("EmployeeAttendance_pk", x => x.EmployeeAttendanceID);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeFinances",
                columns: table => new
                {
                    EmployeeFinancesID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApprovedByID = table.Column<int>(type: "int", nullable: false),
                    BaseSalary = table.Column<decimal>(type: "money", nullable: true),
                    HourlyRate = table.Column<decimal>(type: "money", nullable: true),
                    Bonuses = table.Column<decimal>(type: "money", nullable: false),
                    Allowances = table.Column<decimal>(type: "money", nullable: false),
                    Deductions = table.Column<decimal>(type: "money", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("EmployeeFinances_pk", x => x.EmployeeFinancesID);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    EmployeeStatisticsID = table.Column<int>(type: "int", nullable: false),
                    EmployeeFinancesID = table.Column<int>(type: "int", nullable: false),
                    RentalPlaceID = table.Column<int>(type: "int", nullable: false),
                    AddressId = table.Column<int>(type: "int", nullable: false),
                    EmployeePositionID = table.Column<int>(type: "int", nullable: false),
                    SupervisorID = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HireDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TerminationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "Employees_Addresses",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "AddressID");
                    table.ForeignKey(
                        name: "Employees_EmployeeFinances",
                        column: x => x.EmployeeFinancesID,
                        principalTable: "EmployeeFinances",
                        principalColumn: "EmployeeFinancesID");
                    table.ForeignKey(
                        name: "Employees_EmployeePositions",
                        column: x => x.EmployeePositionID,
                        principalTable: "EmployeePositions",
                        principalColumn: "EmployeePositionID");
                    table.ForeignKey(
                        name: "Employees_EmployeeStatistics",
                        column: x => x.EmployeeStatisticsID,
                        principalTable: "EmployeeStatistics",
                        principalColumn: "EmployeeStatisticsID");
                    table.ForeignKey(
                        name: "Employees_Employees",
                        column: x => x.SupervisorID,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "Employees_RentalPlaces",
                        column: x => x.RentalPlaceID,
                        principalTable: "RentalPlaces",
                        principalColumn: "RentalPlaceID");
                    table.ForeignKey(
                        name: "FK_Employees_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeLeave",
                columns: table => new
                {
                    EmployeeLeaveID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeID = table.Column<int>(type: "int", nullable: false),
                    EmployeeLeaveTypeID = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    ApprovedByID = table.Column<int>(type: "int", nullable: false),
                    ApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("EmployeeLeave_pk", x => x.EmployeeLeaveID);
                    table.ForeignKey(
                        name: "EmployeeLeave_EmployeeLeaveTypes",
                        column: x => x.EmployeeLeaveTypeID,
                        principalTable: "EmployeeLeaveTypes",
                        principalColumn: "EmployeeLeaveTypeID");
                    table.ForeignKey(
                        name: "EmployeeLeave_Employees",
                        column: x => x.EmployeeID,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "EmployeeLeave_Employees_ApprovedByID",
                        column: x => x.ApprovedByID,
                        principalTable: "Employees",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EmployeeSchedules",
                columns: table => new
                {
                    EmployeeScheduleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeShiftTypeID = table.Column<int>(type: "int", nullable: false),
                    EmployeeID = table.Column<int>(type: "int", nullable: false),
                    PlaceOfWorkID = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByEmployeeID = table.Column<int>(type: "int", nullable: false),
                    ModifiedByEmployeeID = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("EmployeeSchedules_pk", x => x.EmployeeScheduleID);
                    table.ForeignKey(
                        name: "EmployeeSchedule_EmployeeShiftType",
                        column: x => x.EmployeeShiftTypeID,
                        principalTable: "EmployeeShiftTypes",
                        principalColumn: "EmployeeShiftTypeID");
                    table.ForeignKey(
                        name: "EmployeeSchedule_Employees",
                        column: x => x.EmployeeID,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "EmployeeSchedule_Employees_CreatedByEmployeeID",
                        column: x => x.CreatedByEmployeeID,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "EmployeeSchedule_Employees_ModifiedByEmployeeID",
                        column: x => x.ModifiedByEmployeeID,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "EmployeeSchedule_RentalPlaces",
                        column: x => x.PlaceOfWorkID,
                        principalTable: "RentalPlaces",
                        principalColumn: "RentalPlaceID");
                });

            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    NewsID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NewsTypeID = table.Column<int>(type: "int", nullable: false),
                    ImageID = table.Column<int>(type: "int", nullable: true),
                    Content = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByEmployeeID = table.Column<int>(type: "int", nullable: false),
                    ModifiedByEmployeeID = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("News_pk", x => x.NewsID);
                    table.ForeignKey(
                        name: "News_Documents",
                        column: x => x.ImageID,
                        principalTable: "Documents",
                        principalColumn: "DocumentID");
                    table.ForeignKey(
                        name: "News_Employees",
                        column: x => x.CreatedByEmployeeID,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "News_Employees_ModifiedByEmployeeID",
                        column: x => x.ModifiedByEmployeeID,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "News_NewsTypes",
                        column: x => x.NewsTypeID,
                        principalTable: "NewsTypes",
                        principalColumn: "NewsTypeID");
                });

            migrationBuilder.CreateTable(
                name: "PostRentalReports",
                columns: table => new
                {
                    PostRentalReportID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InspectorEmployeeID = table.Column<int>(type: "int", nullable: false),
                    IsCustomerLate = table.Column<bool>(type: "bit", nullable: false),
                    IsCarDamaged = table.Column<bool>(type: "bit", nullable: false),
                    IsCarRefueled = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PostRentalReports_pk", x => x.PostRentalReportID);
                    table.ForeignKey(
                        name: "PostRentalReports_Employees",
                        column: x => x.InspectorEmployeeID,
                        principalTable: "Employees",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Rentals",
                columns: table => new
                {
                    RentalID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostRentalReportID = table.Column<int>(type: "int", nullable: true),
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    VehicleID = table.Column<int>(type: "int", nullable: false),
                    StartedByEmployeeID = table.Column<int>(type: "int", nullable: false),
                    FinishedByEmployeeID = table.Column<int>(type: "int", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActualStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Cost = table.Column<decimal>(type: "money", nullable: false),
                    ActualCost = table.Column<decimal>(type: "money", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Rentals_pk", x => x.RentalID);
                    table.ForeignKey(
                        name: "Rentals_Customers",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "Rentals_Employees",
                        column: x => x.FinishedByEmployeeID,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "Rentals_Employees_StartedByEmployeeID",
                        column: x => x.StartedByEmployeeID,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "Rentals_PostRentalReports",
                        column: x => x.PostRentalReportID,
                        principalTable: "PostRentalReports",
                        principalColumn: "PostRentalReportID");
                    table.ForeignKey(
                        name: "Rentals_Vehicles",
                        column: x => x.VehicleID,
                        principalTable: "Vehicles",
                        principalColumn: "VehicleID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CountryID",
                table: "Addresses",
                column: "CountryID");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_AddressID",
                table: "Customers",
                column: "AddressID");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CustomerStatisticsID",
                table: "Customers",
                column: "CustomerStatisticsID");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CustomerTypeID",
                table: "Customers",
                column: "CustomerTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentCategories_ParentCategoryID",
                table: "DocumentCategories",
                column: "ParentCategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_CreatedByEmployeeID",
                table: "Documents",
                column: "CreatedByEmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_CustomerID",
                table: "Documents",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_DocumentCategoryID",
                table: "Documents",
                column: "DocumentCategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_DocumentTypeID",
                table: "Documents",
                column: "DocumentTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_EmployeeID",
                table: "Documents",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_ModifiedByEmployeeID",
                table: "Documents",
                column: "ModifiedByEmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_RentalID",
                table: "Documents",
                column: "RentalID");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_RentalPlaceID",
                table: "Documents",
                column: "RentalPlaceID");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_VehicleID",
                table: "Documents",
                column: "VehicleID");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAttendance_EmployeeID",
                table: "EmployeeAttendance",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeFinances_ApprovedByID",
                table: "EmployeeFinances",
                column: "ApprovedByID");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeLeave_ApprovedByID",
                table: "EmployeeLeave",
                column: "ApprovedByID");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeLeave_EmployeeID",
                table: "EmployeeLeave",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeLeave_EmployeeLeaveTypeID",
                table: "EmployeeLeave",
                column: "EmployeeLeaveTypeID");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "EmployeeRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_AddressId",
                table: "Employees",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_EmployeeFinancesID",
                table: "Employees",
                column: "EmployeeFinancesID");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_EmployeePositionID",
                table: "Employees",
                column: "EmployeePositionID");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_EmployeeStatisticsID",
                table: "Employees",
                column: "EmployeeStatisticsID");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_RentalPlaceID",
                table: "Employees",
                column: "RentalPlaceID");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_SupervisorID",
                table: "Employees",
                column: "SupervisorID");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSchedules_CreatedByEmployeeID",
                table: "EmployeeSchedules",
                column: "CreatedByEmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSchedules_EmployeeID",
                table: "EmployeeSchedules",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSchedules_EmployeeShiftTypeID",
                table: "EmployeeSchedules",
                column: "EmployeeShiftTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSchedules_ModifiedByEmployeeID",
                table: "EmployeeSchedules",
                column: "ModifiedByEmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSchedules_PlaceOfWorkID",
                table: "EmployeeSchedules",
                column: "PlaceOfWorkID");

            migrationBuilder.CreateIndex(
                name: "IX_News_CreatedByEmployeeID",
                table: "News",
                column: "CreatedByEmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_News_ImageID",
                table: "News",
                column: "ImageID");

            migrationBuilder.CreateIndex(
                name: "IX_News_ModifiedByEmployeeID",
                table: "News",
                column: "ModifiedByEmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_News_NewsTypeID",
                table: "News",
                column: "NewsTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_PostRentalReports_InspectorEmployeeID",
                table: "PostRentalReports",
                column: "InspectorEmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_RentalPlaces_AddressID",
                table: "RentalPlaces",
                column: "AddressID");

            migrationBuilder.CreateIndex(
                name: "IX_RentalPlaces_LocationID",
                table: "RentalPlaces",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_CustomerID",
                table: "Rentals",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_FinishedByEmployeeID",
                table: "Rentals",
                column: "FinishedByEmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_PostRentalReportID",
                table: "Rentals",
                column: "PostRentalReportID");

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_StartedByEmployeeID",
                table: "Rentals",
                column: "StartedByEmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_VehicleID",
                table: "Rentals",
                column: "VehicleID");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleModels_VehicleBrandID",
                table: "VehicleModels",
                column: "VehicleBrandID");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_LocationID",
                table: "Vehicles",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_RentalPlaceID",
                table: "Vehicles",
                column: "RentalPlaceID");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_VehicleModelID",
                table: "Vehicles",
                column: "VehicleModelID");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_VehicleOptionalInformationID",
                table: "Vehicles",
                column: "VehicleOptionalInformationID");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_VehicleStatisticsID",
                table: "Vehicles",
                column: "VehicleStatisticsID");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_VehicleTypeID",
                table: "Vehicles",
                column: "VehicleTypeID");

            migrationBuilder.AddForeignKey(
                name: "Documents_Employees",
                table: "Documents",
                column: "EmployeeID",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "Documents_Employees_CreatedByEmployeeID",
                table: "Documents",
                column: "CreatedByEmployeeID",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "Documents_Employees_ModifiedByEmployeeID",
                table: "Documents",
                column: "ModifiedByEmployeeID",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "Documents_Rentals",
                table: "Documents",
                column: "RentalID",
                principalTable: "Rentals",
                principalColumn: "RentalID");

            migrationBuilder.AddForeignKey(
                name: "EmployeeAttendance_Employees",
                table: "EmployeeAttendance",
                column: "EmployeeID",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "EmployeeFinances_Employees_ApprovedByID",
                table: "EmployeeFinances",
                column: "ApprovedByID",
                principalTable: "Employees",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "Addresses_Countries",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_AspNetUsers_Id",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "Employees_Addresses",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "RentalPlaces_Addresses",
                table: "RentalPlaces");

            migrationBuilder.DropForeignKey(
                name: "EmployeeFinances_Employees_ApprovedByID",
                table: "EmployeeFinances");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "EmployeeAttendance");

            migrationBuilder.DropTable(
                name: "EmployeeLeave");

            migrationBuilder.DropTable(
                name: "EmployeeSchedules");

            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.DropTable(
                name: "EmployeeRoles");

            migrationBuilder.DropTable(
                name: "EmployeeLeaveTypes");

            migrationBuilder.DropTable(
                name: "EmployeeShiftTypes");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "NewsTypes");

            migrationBuilder.DropTable(
                name: "DocumentCategories");

            migrationBuilder.DropTable(
                name: "DocumentTypes");

            migrationBuilder.DropTable(
                name: "Rentals");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "PostRentalReports");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "CustomerStatistics");

            migrationBuilder.DropTable(
                name: "CustomerTypes");

            migrationBuilder.DropTable(
                name: "VehicleModels");

            migrationBuilder.DropTable(
                name: "VehicleOptionalInformationDto");

            migrationBuilder.DropTable(
                name: "VehicleStatistics");

            migrationBuilder.DropTable(
                name: "VehicleTypes");

            migrationBuilder.DropTable(
                name: "VehicleBrands");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "EmployeeFinances");

            migrationBuilder.DropTable(
                name: "EmployeePositions");

            migrationBuilder.DropTable(
                name: "EmployeeStatistics");

            migrationBuilder.DropTable(
                name: "RentalPlaces");

            migrationBuilder.DropTable(
                name: "Locations");
        }
    }
}
