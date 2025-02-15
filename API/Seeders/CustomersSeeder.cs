using API.Context;
using API.Models.DTOs.Customers;
using API.Models.DTOs.Other;
using API.Services.Customers;

namespace API.Seeders
{
    public class CustomersSeeder
    {
        public static async Task SeedAsync(ApiDbContext context, CustomersService _customersService,
            CustomerTypesService _customerTypesService)
        {
            if (!context.CustomerTypes.Any())
            {
                await _customerTypesService.CreateAsync(new CustomerTypeDto
                {
                    CustomerType = "Regular",
                    MaxRentals = 3,
                    DiscountPercent = 0,
                    IsActive = true
                });
                await _customerTypesService.CreateAsync(new CustomerTypeDto
                {
                    CustomerType = "Premium",
                    MaxRentals = 5,
                    DiscountPercent = 10,
                    IsActive = true
                });
                await _customerTypesService.CreateAsync(new CustomerTypeDto
                {
                    CustomerType = "Company",
                    MaxRentals = 10,
                    DiscountPercent = 5,
                    IsActive = true
                });
            }

            if (!context.Customers.Any())
            {
                var regular = await _customerTypesService.GetByIdAsync(1);
                var premium = await _customerTypesService.GetByIdAsync(2);
                var company = await _customerTypesService.GetByIdAsync(3);

                await _customersService.CreateAsync(new CustomerDto
                {
                    FirstName = "John",
                    LastName = "Doe",
                    CustomerType = regular,
                    Email = "john.doe@gmail.com",
                    PhoneNumber = "123456789",
                    Password = "123",
                    Address = new AddressDto
                    {
                        FirstLine = "Customer Address",
                        SecondLine = "28",
                        City = "Customer City",
                        Country = new CountryDto
                        {
                            CountryId = 1,
                            FullName = "Republic of Poland",
                            Abbreviation = "PL",
                            DialingCode = "+48"
                        },
                        ZipCode = "12-345"
                    }
                });
                await _customersService.CreateAsync(new CustomerDto
                {
                    FirstName = "Jane",
                    LastName = "Doe",
                    CustomerType = premium,
                    Email = "jane.doe@gmail.com",
                    PhoneNumber = "987654321",
                    Password = "123",
                    Address = new AddressDto
                    {
                        FirstLine = "Customer Address",
                        SecondLine = "52",
                        City = "Customer City District",
                        Country = new CountryDto
                        {
                            CountryId = 1,
                            FullName = "Republic of Poland",
                            Abbreviation = "PL",
                            DialingCode = "+48"
                        },
                        ZipCode = "12-345"
                    }
                });
                await _customersService.CreateAsync(new CustomerDto
                {
                    FirstName = "Jack",
                    LastName = "Mooped",
                    CustomerType = company,
                    Email = "jack.mooped@gmail.com",
                    PhoneNumber = "123456789",
                    Password = "123",
                    Address = new AddressDto
                    {
                        FirstLine = "Customer Address",
                        SecondLine = "123",
                        City = "Customer City",
                        Country = new CountryDto
                        {
                            CountryId = 1,
                            FullName = "Republic of Poland",
                            Abbreviation = "PL",
                            DialingCode = "+48"
                        },
                        ZipCode = "12-521"
                    },
                    CompanyName = "Jack Mooped Company"
                });
            }
        }
    }
}
