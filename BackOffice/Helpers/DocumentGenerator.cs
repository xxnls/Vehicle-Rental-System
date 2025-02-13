using System.Net.Mime;
using System.Windows.Documents;
using BackOffice.Models.DTOs.FileSystem;
using BackOffice.Models.DTOs.Rentals;
using BackOffice.Services;
using QuestPDF.Companion;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace BackOffice.Helpers
{

    public class DocumentGenerator
    {

        public DocumentGenerator()
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }

        private readonly ApiClient _context = new ApiClient();
        public async Task GenerateInvoice(RentalDto rentalDto = null)
        {
            rentalDto = await _context.GetAsync<RentalDto>("Rentals", 1);

            var invoice = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header()
                        .Text(LocalizationHelper.GetString("Documents", "HeadTitle") + rentalDto.RentalId)
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

                                table.Cell().Text(LocalizationHelper.GetString("Documents", "From")).Bold().BackgroundColor(Colors.Grey.Lighten2);
                                table.Cell();
                                table.Cell();
                                table.Cell().Text(LocalizationHelper.GetString("Documents", "BillTo")).Bold().BackgroundColor(Colors.Grey.Lighten2);
                                table.Cell();

                                table.Cell().Text(LocalizationHelper.GetString("Documents", "Company"));
                                table.Cell().AlignRight().Text("Lorem Ipsum Rentals");
                                table.Cell().ColumnSpan(3);

                                table.Cell().Text(LocalizationHelper.GetString("Documents", "Seller"));
                                table.Cell().AlignRight().Text(rentalDto.StartedByEmployee.FirstName + " " + rentalDto.StartedByEmployee.LastName);
                                table.Cell();
                                table.Cell().Text(LocalizationHelper.GetString("Documents", "Client"));
                                table.Cell().AlignRight().Text(rentalDto.Customer.FirstName + " " + rentalDto.Customer.LastName);

                                table.Cell().Text(LocalizationHelper.GetString("Documents", "Country"));
                                table.Cell().AlignRight().Text(rentalDto.StartedByEmployee.RentalPlace.Address.Country.Name);
                                table.Cell();
                                table.Cell().Text(LocalizationHelper.GetString("Documents", "Country"));
                                table.Cell().AlignRight().Text(rentalDto.Customer.Address.Country.Name);

                                table.Cell().Text(LocalizationHelper.GetString("Documents", "Address"));
                                table.Cell().AlignRight().Text(rentalDto.StartedByEmployee.RentalPlace.Address.FirstLine + " " + rentalDto.StartedByEmployee.RentalPlace.Address.SecondLine);
                                table.Cell();
                                table.Cell().Text(LocalizationHelper.GetString("Documents", "Address"));
                                table.Cell().AlignRight().Text(rentalDto.Customer.Address.FirstLine + " " + rentalDto.Customer.Address.SecondLine);

                                table.Cell().Text(LocalizationHelper.GetString("Documents", "City"));
                                table.Cell().AlignRight().Text(rentalDto.StartedByEmployee.RentalPlace.Address.City);
                                table.Cell();
                                table.Cell().Text(LocalizationHelper.GetString("Documents", "City"));
                                table.Cell().AlignRight().Text(rentalDto.Customer.Address.City);

                                table.Cell().Text(LocalizationHelper.GetString("Documents", "Zip"));
                                table.Cell().AlignRight().Text(rentalDto.StartedByEmployee.RentalPlace.Address.ZipCode);
                                table.Cell();
                                table.Cell().Text(LocalizationHelper.GetString("Documents", "Zip"));
                                table.Cell().AlignRight().Text(rentalDto.Customer.Address.ZipCode);

                                table.Cell().Text(LocalizationHelper.GetString("Documents", "Phone"));
                                table.Cell().AlignRight().Text("Lorem Ipsum Cmp Phone");
                                table.Cell();
                                table.Cell().Text(LocalizationHelper.GetString("Documents", "Phone"));
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

                                table.Cell().Text(LocalizationHelper.GetString("Documents", "Details")).Bold().BackgroundColor(Colors.Grey.Lighten2);
                                table.Cell().ColumnSpan(4);

                                table.Cell().Text(LocalizationHelper.GetString("Documents", "Date"));
                                table.Cell().ColumnSpan(2).AlignRight().Text(rentalDto.FinishDateTime.ToString());
                                table.Cell().ColumnSpan(2);

                                table.Cell().Text(LocalizationHelper.GetString("Documents", "VehicleType"));
                                table.Cell().ColumnSpan(2).AlignRight().Text(rentalDto.Vehicle.VehicleType.Name);
                                table.Cell().ColumnSpan(2);

                                table.Cell().Text("MODEL:");
                                table.Cell().ColumnSpan(2).AlignRight().Text(rentalDto.Vehicle.VehicleModel.VehicleBrand.Name + " " + rentalDto.Vehicle.VehicleModel.Name);
                                table.Cell().ColumnSpan(2);

                                table.Cell().Text(LocalizationHelper.GetString("Documents", "PlateNumbers"));
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
                                    header.Cell().BorderRight(1).BorderBottom(1).AlignCenter().Text(LocalizationHelper.GetString("Documents", "Units"));
                                    header.Cell().BorderRight(1).BorderBottom(1).AlignCenter().Text(LocalizationHelper.GetString("Documents", "UnitPrice"));
                                    header.Cell().BorderRight(1).BorderBottom(1).AlignCenter().Text(LocalizationHelper.GetString("Documents", "StartDate"));
                                    header.Cell().BorderRight(1).BorderBottom(1).AlignCenter().Text(LocalizationHelper.GetString("Documents", "EndDate"));
                                    header.Cell().BorderBottom(1).AlignCenter().Text(LocalizationHelper.GetString("Documents", "Amount"));
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

                                tab.Cell().ColumnSpan(3).Text(LocalizationHelper.GetString("Documents", "CustomerSignature")).Bold();
                                tab.Cell().PaddingLeft(5).Text(LocalizationHelper.GetString("Documents", "Subtotal"));
                                tab.Cell().AlignRight().PaddingRight(5).Text((rentalDto.FinalCost.Value / (1.0m - 0.1m)).ToString("N2"));

                                tab.Cell().ColumnSpan(3);
                                tab.Cell().PaddingLeft(5).Text(LocalizationHelper.GetString("Documents", "Discount"));
                                tab.Cell().AlignRight().PaddingRight(5).Text(rentalDto.Customer.CustomerType.DiscountPercent + "%");

                                tab.Cell().ColumnSpan(2).BorderBottom(1).Text("");
                                tab.Cell();
                                tab.Cell().PaddingLeft(5).Text("VAT");
                                tab.Cell().AlignRight().PaddingRight(5).Text("23%");

                                tab.Cell().ColumnSpan(3);
                                tab.Cell().PaddingLeft(5).Text(LocalizationHelper.GetString("Documents", "Total"));
                                tab.Cell().AlignRight().PaddingRight(5).Text(rentalDto.FinalCost.ToString());

                                tab.Cell().ColumnSpan(5).Text(LocalizationHelper.GetString("Documents", "EmployeeSignature")).Bold();
                                tab.Cell().ColumnSpan(5).Text("");
                                tab.Cell().ColumnSpan(2).BorderBottom(1).Text("");

                            });
                        });
                });
            }).ShowInCompanionAsync();
        }
    }
}