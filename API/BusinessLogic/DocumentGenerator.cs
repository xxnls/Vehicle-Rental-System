using API.Models.DTOs.FileSystem;
using API.Models.DTOs.Rentals;
using API.Resources;
using API.Services.FileSystem;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using QuestPDF.Companion;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Globalization;

namespace API.BusinessLogic
{

    public class DocumentGenerator : IDocumentGenerator
    {
        private readonly FileSystemService _fileSystemService;
        private readonly DocumentCategoriesService _documentCategoriesService;
        private readonly DocumentTypesService _documentTypesService;
        private readonly ILocalizationService _localization;

        public DocumentGenerator(FileSystemService fileSystemService, ILocalizationService localizationService, DocumentCategoriesService documentCategoriesService, DocumentTypesService documentTypesService)
        {
            _fileSystemService = fileSystemService;
            QuestPDF.Settings.License = LicenseType.Community;
            _localization = localizationService;
            _documentCategoriesService = documentCategoriesService;
            _documentTypesService = documentTypesService;
        }


        public async Task GenerateInvoiceAsync(RentalDto rentalDto)
        {
            var culture = GetCultureFromCountry(rentalDto.StartedByEmployee.RentalPlace.Address.Country.Abbreviation);
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            // Verify the culture is applied
            Console.WriteLine("Current Culture: " + CultureInfo.CurrentUICulture);

            var title = _localization.GetString("HeadTitle");
            Console.WriteLine("Localized Title: " + title); // Debugging

            var invoice = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header()
                        .Text(/*_localization.GetString("HeadTitle")*/"CAR RENTAL INVOICE #" + rentalDto.RentalId)
                        .SemiBold().FontSize(24).AlignLeft();

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(column =>
                        {
                            column.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                });

                                table.Cell().Text(/*_localization.GetString("From")*/"FROM").Bold().BackgroundColor(Colors.Grey.Lighten2);
                                table.Cell();
                                table.Cell();
                                table.Cell().Text(/*_localization.GetString("BillTo")*/"BILL TO").Bold().BackgroundColor(Colors.Grey.Lighten2);
                                table.Cell();

                                table.Cell().Text(/*_localization.GetString( "Company")*/"COMPANY:");
                                table.Cell().AlignRight().Text("Lorem Ipsum Rentals");
                                table.Cell().ColumnSpan(3);

                                table.Cell().Text(/*_localization.GetString("Seller")*/"SELLER:");
                                table.Cell().AlignRight().Text(rentalDto.StartedByEmployee.FirstName + " " + rentalDto.StartedByEmployee.LastName);
                                table.Cell();
                                table.Cell().Text(/*_localization.GetString("Client")*/"CLIENT:");
                                table.Cell().AlignRight().Text(rentalDto.Customer.FirstName + " " + rentalDto.Customer.LastName);

                                table.Cell().Text(/*_localization.GetString("Country")*/"COUNTRY:");
                                table.Cell().AlignRight().Text(rentalDto.StartedByEmployee.RentalPlace.Address.Country.Name);
                                table.Cell();
                                table.Cell().Text(/*_localization.GetString("Country")*/"COUNTRY:");
                                table.Cell().AlignRight().Text(rentalDto.Customer.Address.Country.Name);

                                table.Cell().Text(/*_localization.GetString("Address")*/"ADDRESS:");
                                table.Cell().AlignRight().Text(rentalDto.StartedByEmployee.RentalPlace.Address.FirstLine + " " + rentalDto.StartedByEmployee.RentalPlace.Address.SecondLine);
                                table.Cell();
                                table.Cell().Text(/*_localization.GetString("Address")*/"ADDRESS:");
                                table.Cell().AlignRight().Text(rentalDto.Customer.Address.FirstLine + " " + rentalDto.Customer.Address.SecondLine);

                                table.Cell().Text(/*_localization.GetString("City")*/"CITY:");
                                table.Cell().AlignRight().Text(rentalDto.StartedByEmployee.RentalPlace.Address.City);
                                table.Cell();
                                table.Cell().Text(/*_localization.GetString("City")*/"CITY:");
                                table.Cell().AlignRight().Text(rentalDto.Customer.Address.City);

                                table.Cell().Text(/*_localization.GetString("Zip")*/"ZIP:");
                                table.Cell().AlignRight().Text(rentalDto.StartedByEmployee.RentalPlace.Address.ZipCode);
                                table.Cell();
                                table.Cell().Text(/*_localization.GetString("Zip")*/"ZIP:");
                                table.Cell().AlignRight().Text(rentalDto.Customer.Address.ZipCode);

                                table.Cell().Text(/*_localization.GetString("Phone")*/"PHONE:");
                                table.Cell().AlignRight().Text("Lorem Ipsum Cmp Phone");
                                table.Cell();
                                table.Cell().Text(/*_localization.GetString("Phone")*/"PHONE:");
                                table.Cell().AlignRight().Text(rentalDto.Customer.Address.Country.DialingCode + rentalDto.Customer.PhoneNumber);

                                table.Cell().Text("E-MAIL:");
                                table.Cell().AlignRight().Text("Lorem Ipsum Cmp Email");
                                table.Cell();
                                table.Cell().Text("E-MAIL:");
                                table.Cell().AlignRight().Text(rentalDto.Customer.Email);
                            });

                            column.Item().PaddingTop(1, Unit.Centimetre).Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                });

                                table.Cell().Text(/*_localization.GetString("Details")*/"DETAILS").Bold().BackgroundColor(Colors.Grey.Lighten2);
                                table.Cell().ColumnSpan(4);

                                table.Cell().Text(/*_localization.GetString("Date")*/"DATE:");
                                table.Cell().ColumnSpan(2).AlignRight().Text(rentalDto.FinishDateTime.ToString());
                                table.Cell().ColumnSpan(2);

                                table.Cell().Text(/*_localization.GetString("VehicleType")*/"VEHICLE TYPE:");
                                table.Cell().ColumnSpan(2).AlignRight().Text(rentalDto.Vehicle.VehicleType.Name);
                                table.Cell().ColumnSpan(2);

                                table.Cell().Text("MODEL:");
                                table.Cell().ColumnSpan(2).AlignRight().Text(rentalDto.Vehicle.VehicleModel.VehicleBrand.Name + " " + rentalDto.Vehicle.VehicleModel.Name);
                                table.Cell().ColumnSpan(2);

                                table.Cell().Text(/*_localization.GetString("PlateNumbers")*/"PLATE NUMBERS:");
                                table.Cell().ColumnSpan(2).AlignRight().Text(rentalDto.Vehicle.LicensePlate);
                                table.Cell().ColumnSpan(2);

                                table.Cell().Text("VIN:");
                                table.Cell().ColumnSpan(2).AlignRight().Text(rentalDto.Vehicle.Vin);
                                table.Cell().ColumnSpan(2);
                            });

                            column.Item().PaddingTop(1, Unit.Centimetre).Border(1).PaddingBottom(5).Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                });

                                table.Header(header =>
                                {
                                    header.Cell().BorderRight(1).BorderBottom(1).AlignCenter().Text(/*_localization.GetString("Units")*/"UNITS (DAYS)");
                                    header.Cell().BorderRight(1).BorderBottom(1).AlignCenter().Text(/*_localization.GetString("UnitPrice")*/"UNIT PRICE (DAY)");
                                    header.Cell().BorderRight(1).BorderBottom(1).AlignCenter().Text(/*_localization.GetString("StartDate")*/"START DATE");
                                    header.Cell().BorderRight(1).BorderBottom(1).AlignCenter().Text(/*_localization.GetString("EndDate")*/"END DATE");
                                    header.Cell().BorderBottom(1).AlignCenter().Text(/*_localization.GetString("Amount")*/"AMOUNT ($)");
                                });

                                table.Cell().BorderRight(1).PaddingTop(5).AlignCenter().Text((rentalDto.EndDate - rentalDto.StartDate).Days + 1);
                                table.Cell().BorderRight(1).PaddingTop(5).AlignCenter().Text($"{rentalDto.Vehicle.CustomDailyRate ?? 0:N2}");
                                table.Cell().BorderRight(1).PaddingTop(5).AlignCenter().Text(rentalDto.StartDate.ToString("dd/MM/yyyy"));
                                table.Cell().BorderRight(1).PaddingTop(5).AlignCenter().Text(rentalDto.EndDate.ToString("dd/MM/yyyy"));
                                table.Cell().PaddingTop(5).AlignCenter().Text($"{rentalDto.FinalCost ?? 0:N2}");
                            });

                            column.Item().PaddingTop(1, Unit.Centimetre).Table(tab =>
                            {
                                tab.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                });

                                tab.Cell().ColumnSpan(3).Text(/*_localization.GetString("CustomerSignature")*/"CUSTOMER SIGNATURE").Bold();
                                tab.Cell().PaddingLeft(5).Text(/*_localization.GetString("Subtotal")*/"SUBTOTAL");
                                tab.Cell().AlignRight().PaddingRight(5).Text((rentalDto.FinalCost.Value / (1.0m - 0.1m)).ToString("N2"));

                                tab.Cell().ColumnSpan(3);
                                tab.Cell().PaddingLeft(5).Text(/*_localization.GetString("Discount")*/"DISCOUNT");
                                tab.Cell().AlignRight().PaddingRight(5).Text(rentalDto.Customer.CustomerType.DiscountPercent + "%");

                                tab.Cell().ColumnSpan(2).BorderBottom(1).Text("");
                                tab.Cell();
                                tab.Cell().PaddingLeft(5).Text("VAT");
                                tab.Cell().AlignRight().PaddingRight(5).Text("23%");

                                tab.Cell().ColumnSpan(3);
                                tab.Cell().PaddingLeft(5).Text(/*_localization.GetString("Total")*/"TOTAL");
                                tab.Cell().AlignRight().PaddingRight(5).Text(rentalDto.FinalCost.ToString());

                                tab.Cell().ColumnSpan(5).Text(/*_localization.GetString("EmployeeSignature")*/"EMPLOYEE SIGNATURE").Bold();
                                tab.Cell().ColumnSpan(5).Text("");
                                tab.Cell().ColumnSpan(2).BorderBottom(1).Text("");

                            });
                        });
                });
            }).GeneratePdf();

            var financeCategory = await _documentCategoriesService.GetByIdAsync(4);
            var pdfType = await _documentTypesService.GetByIdAsync(10);

            //Check if finance category is really financial category
            if (financeCategory is not { Name: "Financial" })
            {
                throw new Exception("Financial category isn't found.");
            }

            //Check if pdfType is really pdfType
            if (pdfType is not { Name: "PDF File" })
            {
                throw new Exception("PDF type isn't found.");
            }

            var fileUpload = new FileUploadDto
            {
                FileContent = invoice,
                FileName = $"Invoice_{rentalDto.RentalId}.pdf",
                DocumentTypeId = 3,
                DocumentCategoryId = 4,
                RentalId = rentalDto.RentalId,
                CreatedByEmployeeId = rentalDto.StartedByEmployeeId,
                Title = "Rental_#" + rentalDto.RentalId + "_Invoice",
                Description = "Invoice for rental #" + rentalDto.RentalId
            };

            await _fileSystemService.UploadFileAsync(fileUpload);
        }

        // Helper method to get CultureInfo from country abbreviation
        private CultureInfo GetCultureFromCountry(string countryCode)
        {
            return countryCode.ToUpper() switch
            {
                "PL" => new CultureInfo("pl-PL"),
                "EN" => new CultureInfo("en-US"),
                // Add more mappings as needed
                _ => new CultureInfo("en-US") // Default fallback
            };
        }
    }
    public interface IDocumentGenerator
    {
        public Task GenerateInvoiceAsync(RentalDto rentalDto);
    }
}