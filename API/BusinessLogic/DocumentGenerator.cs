using API.Models.DTOs.FileSystem;
using API.Services.FileSystem;
using QuestPDF.Companion;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace API.BusinessLogic
{

    public class DocumentGenerator : IDocumentGenerator
    {
        private readonly FileSystemService _fileSystemService;

        public DocumentGenerator(FileSystemService fileSystemService)
        {
            _fileSystemService = fileSystemService;
            QuestPDF.Settings.License = LicenseType.Community;
        }


        public async Task GenerateInvoice(string? clientName,
            List<(string name, int qty, decimal price)>? items)
        {
            items = new List<(string, int, decimal)>
            {
                ("Laptop", 1, 3000),
                ("Mouse", 2, 50),
                ("Keyboard", 1, 100)
            };
            clientName = "John Doe";

            var doc = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);
                    page.Header().AlignCenter().Text("Vehicle Rental Invoice").Bold().FontSize(24);

                    page.Content().Column(col =>
                    {
                        col.Item().Text("Customer: John Doe").Bold();
                        col.Item().Text("Address: 123 Main St, City");
                        col.Item().Text("Rental ID: 1001");
                        col.Item().Text($"Date: {DateTime.Now.ToShortDateString()}");

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(2);
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("Vehicle").Bold();
                                header.Cell().Text("Model").Bold();
                                header.Cell().Text("Plate Number").Bold();
                                header.Cell().Text("Cost").Bold();
                            });

                            table.Cell().Text("Toyota Corolla");
                            table.Cell().Text("2022");
                            table.Cell().Text("XYZ-123");
                            table.Cell().Text("500 zł");
                        });

                        col.Item().AlignRight().Text("Deposit: 200 zł");
                        col.Item().AlignRight().Text("Damage Fee: 0 zł");
                        col.Item().AlignRight().Text("Total: 500 zł").Bold().FontSize(16);
                    });

                    page.Footer().AlignCenter().Text("Thank you for your business!");
                });
            }).GeneratePdf();

            var dto = new FileUploadDto
            {
                FileContent = doc,
                DocumentTypeId = 9,
                DocumentCategoryId = 1,
                Title = "TEST Invoice",
                FileName = "invoice.pdf",
                CreatedByEmployeeId = 1
            };

            await _fileSystemService.UploadFileAsync(dto);
        }
    }
    public interface IDocumentGenerator
    {
        public Task GenerateInvoice(string? clientName, List<(string name, int qty, decimal price)>? items);
    }
}