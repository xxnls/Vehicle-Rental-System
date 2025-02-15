using API.Context;
using API.Models;
using API.Models.DTOs.Vehicles;
using API.Models.Vehicles;
using API.Services;
using API.Services.Other;
using API.Services.Vehicles;

namespace API.Seeders
{
    public class VehiclesSeeder
    {

        public static async Task SeedAsync(
            ApiDbContext dbContext,
            VehicleBrandsService _vehicleBrandsService,
            VehicleModelsService _vehicleModelsService,
            VehicleTypesService _vehicleTypesService,
            RentalPlacesService _rentalPlacesService,
            VehiclesService _vehiclesService,
            VehicleStatusesService _vehicleStatusesService)
        {
            await SeedVehicleBrandsAsync(dbContext, _vehicleBrandsService);
            await SeedVehicleModelsAsync(dbContext, _vehicleModelsService, _vehicleBrandsService);
            await SeedVehicleTypeAsync(dbContext, _vehicleTypesService);
            await SeedVehiclesAsync(dbContext, _vehicleModelsService, _vehiclesService, _vehicleStatusesService,
                _vehicleTypesService, _rentalPlacesService);
        }

        private static async Task SeedVehicleBrandsAsync(ApiDbContext dbContext, VehicleBrandsService _vehicleBrandsService)
        {
            // Check if ANY VehicleBrands exists before seeding
            if (!dbContext.VehicleBrands.Any())
            {
                await _vehicleBrandsService.CreateAsync(new VehicleBrandDto { Name = "Ford", Website = "https://ford.pl/", LogoUrl = "https://www.ford.pl/content/dam/guxeu/global-shared/header/ford_oval_blue_logo.svg" });
                await _vehicleBrandsService.CreateAsync(new VehicleBrandDto { Name = "Honda", Website = "https://honda.pl/", LogoUrl = "https://www.pngplay.com/wp-content/uploads/2/Honda-Transparent-Free-PNG.png" });
                await _vehicleBrandsService.CreateAsync(new VehicleBrandDto { Name = "Subaru", Website = "https://subaru.pl/", LogoUrl = "https://purepng.com/public/uploads/thumbnail//purepng.com-subaru-car-logologocar-brand-logoscarssubaru-car-logo-1701527429058vce9o.png" });
            }
        }

        private static async Task SeedVehicleModelsAsync(ApiDbContext dbContext, VehicleModelsService _vehicleModelsService, VehicleBrandsService _vehicleBrandsService)
        {
            if (!dbContext.VehicleModels.Any())
            {
                var fordBrand = await _vehicleBrandsService.GetByIdAsync(1);
                var hondaBrand = await _vehicleBrandsService.GetByIdAsync(2);
                var subaruBrand = await _vehicleBrandsService.GetByIdAsync(3);
                await _vehicleModelsService.CreateAsync(new VehicleModelDto
                {
                    Name = "Focus",
                    Description = "The Ford Focus is a popular compact car known for its sporty handling, fuel efficiency, and comfortable interior. It offers a range of engine options and features, making it a versatile choice for a variety of drivers.",
                    VehicleBrand = fordBrand,
                    EngineSize = 1.6,
                    HorsePower = 100,
                    FuelType = "Petrol",
                    ImageUrl = "https://www.autocentrum.pl/NmM0LmpwYQwsUjpeXwxsGG8KbkIRFGMLJFwpQhMWPEA7VT4eGRggQnhTdV0SEXpeLgd4D0NCflh5VXpVQEEtWWNaPApSCg"
                });

                await _vehicleModelsService.CreateAsync(new VehicleModelDto
                {
                    Name = "CR-V",
                    Description = "The Honda CR-V is a popular compact SUV known for its reliability, practicality, and fuel efficiency. It offers a spacious interior, ample cargo space, and a comfortable ride, making it a great choice for families and everyday driving.",
                    VehicleBrand = hondaBrand,
                    EngineSize = 2.2,
                    HorsePower = 150,
                    FuelType = "Diesel",
                    ImageUrl = "https://hips.hearstapps.com/hmg-prod/images/2020-honda-cr-v-hybrid-drive-109-1584417693.jpg?crop=0.936xw:0.790xh;0.0586xw,0.134xh&resize=2048:*"
                });

                await _vehicleModelsService.CreateAsync(new VehicleModelDto
                {
                    Name = "Impreza",
                    Description = "The Subaru Impreza is known for its all-wheel-drive capability, making it a reliable option for driving in various weather conditions. It offers a robust and sporty driving experience, combined with good fuel economy and a spacious interior.",
                    VehicleBrand = subaruBrand,
                    EngineSize = 2.0,
                    HorsePower = 150,
                    FuelType = "Petrol",
                    ImageUrl = "https://superauto.wpcdn.pl/articles/a1d224cfb72567cb17e0a474cd364d17.jpg"
                });
            }
        }

        private static async Task SeedVehicleTypeAsync(ApiDbContext dbContext, VehicleTypesService _vehicleTypesService)
        {
            if (!dbContext.VehicleTypes.Any())
            {
                await _vehicleTypesService.CreateAsync(new VehicleTypeDto
                {
                    Name = "Sedan",
                    Description = "Sedans are a classic car type known for their sleek design and comfortable ride. They offer good fuel efficiency and are suitable for everyday commuting and long-distance travel. Sedans typically have four doors and a trunk, providing ample space for passengers and luggage.",
                    BaseDailyRate = 250,
                    BaseWeeklyRate = 1800,
                    BaseDeposit = 400,
                    RequiredLicenseType = "B"
                });
                await _vehicleTypesService.CreateAsync(new VehicleTypeDto {
                    Name = "SUV",
                    Description = "SUVs (Sport Utility Vehicles) are popular for their versatility and spaciousness. They offer a raised ride height, making them suitable for off-road driving and handling rough terrain. SUVs typically have ample cargo space and can accommodate multiple passengers comfortably. They are a popular choice for families and outdoor enthusiasts.",
                    BaseDailyRate = 300,
                    BaseWeeklyRate = 2100,
                    BaseDeposit = 500,
                    RequiredLicenseType = "B"
                });
                await _vehicleTypesService.CreateAsync(new VehicleTypeDto {
                    Name = "Hatchback",
                    Description = "Hatchbacks are known for their practicality and versatility. They combine the features of a sedan with a rear door that opens upwards, providing easy access to the cargo area. Hatchbacks are ideal for city driving and offer a good balance of passenger space and cargo capacity. They are often more fuel-efficient than larger vehicles like SUVs.",
                    BaseDailyRate = 200,
                    BaseWeeklyRate = 1400,
                    BaseDeposit = 300,
                    RequiredLicenseType = "B"
                });
            }
        }

        private static async Task SeedVehiclesAsync(
            ApiDbContext dbContext,
            VehicleModelsService _vehicleModelsService,
            VehiclesService _vehiclesService,
            VehicleStatusesService _vehicleStatusesService,
            VehicleTypesService _vehicleTypesService,
            RentalPlacesService _rentalPlacesService)
        {
            if (!dbContext.Vehicles.Any())
            {
                var focusModel = await _vehicleModelsService.GetByIdAsync(1);
                var crvModel = await _vehicleModelsService.GetByIdAsync(2);
                var imprezaModel = await _vehicleModelsService.GetByIdAsync(3);
                var available = await _vehicleStatusesService.GetByIdAsync(1);
                var rentalPlace = await _rentalPlacesService.GetByIdAsync(1);
                var sedanType = await _vehicleTypesService.GetByIdAsync(1);
                var suvType = await _vehicleTypesService.GetByIdAsync(2);
                var hatchbackType = await _vehicleTypesService.GetByIdAsync(3);
                await _vehiclesService.CreateAsync(new VehicleDto {
                    LicensePlate = "KR12345",
                    VehicleModel = focusModel,
                    VehicleStatus = available,
                    VehicleType = sedanType,
                    VehicleOptionalInformation = new VehicleOptionalInformationDto
                    {
                        HasAirConditioning = true,
                        HasAutomaticTransmission = true,
                        HasBluetooth = true,
                        HasCruiseControl = true,
                        HasNavigation = true,
                        HasParkingSensors = true,
                    },
                    IsAvailableForRent = true,
                    RentalPlace = rentalPlace,
                    Vin = "00000000000000000",
                    Color = "Black",
                    ManufactureYear = 2010,
                    CurrentMileage = 100000,
                    PurchaseDate = DateTime.Now.AddYears(-2),
                    PurchasePrice = 20000,
                });
                await _vehiclesService.CreateAsync(new VehicleDto
                {
                    LicensePlate = "KR54321",
                    VehicleModel = crvModel,
                    VehicleStatus = available,
                    VehicleType = suvType,
                    VehicleOptionalInformation = new VehicleOptionalInformationDto
                    {
                        HasAirConditioning = true,
                        HasAutomaticTransmission = true,
                        HasBluetooth = true,
                        HasCruiseControl = false,
                        HasNavigation = true,
                        HasParkingSensors = false,
                    },
                    IsAvailableForRent = true,
                    RentalPlace = rentalPlace,
                    Vin = "11111111111111111",
                    Color = "White",
                    ManufactureYear = 2015,
                    CurrentMileage = 80000,
                    PurchaseDate = DateTime.Now.AddYears(-1),
                    PurchasePrice = 30000,
                });
                await _vehiclesService.CreateAsync(new VehicleDto
                {
                    LicensePlate = "KR67890",
                    VehicleModel = imprezaModel,
                    VehicleStatus = available,
                    VehicleType = hatchbackType,
                    VehicleOptionalInformation = new VehicleOptionalInformationDto
                    {
                        HasAirConditioning = true,
                        HasAutomaticTransmission = false,
                        HasBluetooth = true,
                        HasCruiseControl = true,
                        HasNavigation = false,
                        HasParkingSensors = false,
                    },
                    IsAvailableForRent = true,
                    RentalPlace = rentalPlace,
                    Vin = "22222222222222222",
                    Color = "Red",
                    ManufactureYear = 2018,
                    CurrentMileage = 60000,
                    PurchaseDate = DateTime.Now.AddYears(-1),
                    PurchasePrice = 25000,
                });
            }
        }
    }
}
