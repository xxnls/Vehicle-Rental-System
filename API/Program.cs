//using API.Context;

using System.Globalization;
using System.Text;
using API;
using API.Context;
using API.Models;
using API.Services;
using API.Services.Other;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using API.Services.Vehicles;
using API.Seeders;
using API.Services.Employees;
using API.Models.Employees;
using API.Services.Customers;
using API.Models.Customers;
using API.Services.Rentals;
using API.BusinessLogic;
using API.Services.PostRentalReports;
using API.Services.FileSystem;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using API.Resources;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddDbContext<ApiDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<EmployeeAuthService>();
builder.Services.AddScoped<EmployeeRolesService>();
builder.Services.AddScoped<EmployeesService>();
builder.Services.AddScoped<EmployeeShiftTypesService>();
builder.Services.AddScoped<EmployeeLeaveTypesService>();
builder.Services.AddScoped<EmployeePositionsService>();
builder.Services.AddScoped<EmployeeFinancesService>();
builder.Services.AddScoped<EmployeeStatisticsService>();

builder.Services.AddScoped<VehiclesService>();
builder.Services.AddScoped<VehicleStatusesService>();
builder.Services.AddScoped<VehicleBrandsService>();
builder.Services.AddScoped<VehicleModelsService>();
builder.Services.AddScoped<VehicleTypesService>();
builder.Services.AddScoped<VehicleStatisticsService>();
builder.Services.AddScoped<VehicleOptionalInformationService>();

builder.Services.AddScoped<CountriesService>();
builder.Services.AddScoped<RentalPlacesService>();
builder.Services.AddScoped<LocationsService>();
builder.Services.AddScoped<AddressesService>();

builder.Services.AddScoped<CustomerTypesService>();
builder.Services.AddScoped<CustomerStatisticsService>();
builder.Services.AddScoped<CustomersService>();
builder.Services.AddScoped<CustomerAuthService>();
builder.Services.AddScoped<LicenseApprovalRequestsService>();

builder.Services.AddScoped<PaymentsService>();

builder.Services.AddScoped<DocumentTypesService>();
builder.Services.AddScoped<DocumentCategoriesService>();
builder.Services.AddScoped<FileSystemService>();

builder.Services.AddScoped<RentalRequestsService>();
builder.Services.AddScoped<RentalsService>();
builder.Services.AddScoped<PostRentalReportsService>();

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddTransient<IRentalCostCalculator, RentalCostCalculator>();
builder.Services.AddTransient<IRentalProcessing, RentalProcessing>();
builder.Services.AddTransient<ITransactionHandler, TransactionHandler>();
builder.Services.AddTransient<IDocumentGenerator, DocumentGenerator>();
builder.Services.AddTransient<ILicenseProcessing, LicenseProcessing>();

#region Localization

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// Configure supported cultures
var supportedCultures = new[]
{
    new CultureInfo("en-US"),
    new CultureInfo("pl-PL")
};

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("en-US");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

builder.Services.AddSingleton<ILocalizationService, LocalizationService>();

#endregion

builder.Services.AddIdentityCore<Employee>(options => { })
    .AddRoles<EmployeeRole>()
    .AddEntityFrameworkStores<ApiDbContext>()
    .AddDefaultTokenProviders()
    .AddApiEndpoints();

builder.Services.AddIdentityCore<Customer>(options => { })
    .AddEntityFrameworkStores<ApiDbContext>()
    .AddDefaultTokenProviders()
    .AddApiEndpoints();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 3;
    options.Password.RequiredUniqueChars = 1;
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddDataProtection();

var app = builder.Build();

// Ensure the database is created and seeded
using (var scope = app.Services.CreateScope())
{
    var countriesService = scope.ServiceProvider.GetRequiredService<CountriesService>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<EmployeeRole>>();
    var dictionaryTablesService = scope.ServiceProvider.GetRequiredService<ApiDbContext>();
    var employeeRoles = scope.ServiceProvider.GetRequiredService<EmployeeRolesService>();
    var employees = scope.ServiceProvider.GetRequiredService<EmployeesService>();
    var employeePositions = scope.ServiceProvider.GetRequiredService<EmployeePositionsService>();
    var rentalPlaces = scope.ServiceProvider.GetRequiredService<RentalPlacesService>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Employee>>();
    var vehicleBrands = scope.ServiceProvider.GetRequiredService<VehicleBrandsService>();
    var vehicleModels = scope.ServiceProvider.GetRequiredService<VehicleModelsService>();
    var vehicleTypes = scope.ServiceProvider.GetRequiredService<VehicleTypesService>();
    var vehicles = scope.ServiceProvider.GetRequiredService<VehiclesService>();
    var vehicleStatuses = scope.ServiceProvider.GetRequiredService<VehicleStatusesService>();
    var customers = scope.ServiceProvider.GetRequiredService<CustomersService>();
    var customerTypes = scope.ServiceProvider.GetRequiredService<CustomerTypesService>();

    await CountrySeeder.SeedAsync(countriesService);
    await EmployeeRolesSeeder.SeedAsync(roleManager);
    await DictionaryTablesSeeder.SeedAsync(dictionaryTablesService);
    await EmployeeSeeder.SeedAsync(employees, employeeRoles, employeePositions, rentalPlaces, userManager);
    await VehiclesSeeder.SeedAsync(dictionaryTablesService, vehicleBrands, vehicleModels, vehicleTypes, rentalPlaces,
        vehicles, vehicleStatuses);
    await CustomersSeeder.SeedAsync(dictionaryTablesService, customers, customerTypes);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.MapIdentityApi<Employee>();
//app.MapIdentityApi<Customer>();

// Configure localization middleware
var localizationOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value;
app.UseRequestLocalization(localizationOptions);

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();
app.UseRequestLocalization();
app.Run();