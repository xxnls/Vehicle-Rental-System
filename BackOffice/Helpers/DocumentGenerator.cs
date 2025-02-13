using System.Net.Mime;
using System.Windows.Documents;
using BackOffice.Models.DTOs.FileSystem;
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


        public async Task GenerateInvoice(string? clientName,
            List<(string name, int qty, decimal price)>? items)
        {
            var invoice = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header()
                        .Text("CAR RENTAL INVOICE #" + "<INVOICE ID>")
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

                                table.Cell().Text("FROM").Bold().BackgroundColor(Colors.Grey.Lighten2);
                                table.Cell();
                                table.Cell();
                                table.Cell().Text("BILL TO").Bold().BackgroundColor(Colors.Grey.Lighten2);
                                table.Cell();

                                table.Cell().Text("COMPANY:");
                                table.Cell().AlignRight().Text("<OUR C NAME>");
                                table.Cell();
                                table.Cell().Text("COMPANY:");
                                table.Cell().AlignRight().Text("<CLNT C NAME>");

                                table.Cell().Text("ATTN:");
                                table.Cell().AlignRight().Text("<OUR ATTN>");
                                table.Cell();
                                table.Cell().Text("ATTN:");
                                table.Cell().AlignRight().Text("<CLNT ATTN>");

                                table.Cell().Text("COUNTRY:");
                                table.Cell().AlignRight().Text("<OUR CNT>");
                                table.Cell();
                                table.Cell().Text("COUNTRY:");
                                table.Cell().AlignRight().Text("<CLNT CNT>");

                                table.Cell().Text("ADDRESS:");
                                table.Cell().AlignRight().Text("<OUR ADD>");
                                table.Cell();
                                table.Cell().Text("ADDRESS:");
                                table.Cell().AlignRight().Text("<CLNT ADD>");

                                table.Cell().Text("CITY:");
                                table.Cell().AlignRight().Text("<OUR CITY>");
                                table.Cell();
                                table.Cell().Text("CITY:");
                                table.Cell().AlignRight().Text("<CLNT CITY>");

                                table.Cell().Text("ZIP:");
                                table.Cell().AlignRight().Text("<OUR ZIP>");
                                table.Cell();
                                table.Cell().Text("ZIP:");
                                table.Cell().AlignRight().Text("<CLNT ZIP>");

                                table.Cell().Text("PHONE:");
                                table.Cell().AlignRight().Text("<OUR PHN>");
                                table.Cell();
                                table.Cell().Text("PHONE:");
                                table.Cell().AlignRight().Text("<CLNT PHN>");

                                table.Cell().Text("E-MAIL:");
                                table.Cell().AlignRight().Text("<OUR EML>");
                                table.Cell();
                                table.Cell().Text("E-MAIL:");
                                table.Cell().AlignRight().Text("<CLNT EML>");
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

                                table.Cell().Text("DETAILS").Bold().BackgroundColor(Colors.Grey.Lighten2);
                                table.Cell().ColumnSpan(4);

                                table.Cell().Text("DATE:");
                                table.Cell().AlignRight().Text("<DATE>");
                                table.Cell().ColumnSpan(3);

                                table.Cell().Text("VEHICLE TYPE:");
                                table.Cell().AlignRight().Text("<TYPE>");
                                table.Cell().ColumnSpan(3);

                                table.Cell().Text("MODEL:");
                                table.Cell().AlignRight().Text("<BRN MODEL>");
                                table.Cell().ColumnSpan(3);

                                table.Cell().Text("PLATE NUMBER:");
                                table.Cell().AlignRight().Text("<PL NUM>");
                                table.Cell().ColumnSpan(3);

                                table.Cell().Text("VIN:");
                                table.Cell().AlignRight().Text("<VIN>");
                                table.Cell().ColumnSpan(3);
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
                                    header.Cell().BorderRight(1).BorderBottom(1).AlignCenter().Text("UNITS (DAYS)");
                                    header.Cell().BorderRight(1).BorderBottom(1).AlignCenter().Text("UNIT PRICE (DAY)");
                                    header.Cell().BorderRight(1).BorderBottom(1).AlignCenter().Text("START DATE");
                                    header.Cell().BorderRight(1).BorderBottom(1).AlignCenter().Text("END DATE");
                                    header.Cell().BorderBottom(1).AlignCenter().Text("AMOUNT (<CURR>)");
                                });

                                table.Cell().BorderRight(1).PaddingTop(5).AlignCenter().Text("1");
                                table.Cell().BorderRight(1).PaddingTop(5).AlignCenter().Text("<RNT PRICE>");
                                table.Cell().BorderRight(1).PaddingTop(5).AlignCenter().Text("<START DATE>");
                                table.Cell().BorderRight(1).PaddingTop(5).AlignCenter().Text("<END DATE>");
                                table.Cell().PaddingTop(5).AlignCenter().Text("<PRICE>");
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

                                tab.Cell().ColumnSpan(3).Text("CUSTOMER SIGNATURE").Bold();
                                tab.Cell().PaddingLeft(5).Text("SUBTOTAL");
                                tab.Cell().AlignRight().PaddingRight(5).Text("<SUB>");

                                tab.Cell().ColumnSpan(3);
                                tab.Cell().PaddingLeft(5).Text("DISCOUNT");
                                tab.Cell().AlignRight().PaddingRight(5).Text("<DSC>");

                                tab.Cell().ColumnSpan(2).BorderBottom(1).Text("");
                                tab.Cell();
                                tab.Cell().PaddingLeft(5).Text("TAX/VAT");
                                tab.Cell().AlignRight().PaddingRight(5).Text("23%");

                                tab.Cell().ColumnSpan(3);
                                tab.Cell().PaddingLeft(5).Text("TOTAL");
                                tab.Cell().AlignRight().PaddingRight(5).Text("<TOTAL>");

                                tab.Cell().ColumnSpan(5).Text("EMPLOYEE SIGNATURE").Bold();
                                tab.Cell().ColumnSpan(5).Text("");
                                tab.Cell().ColumnSpan(2).BorderBottom(1).Text("");

                            });
                        });
                });
            }).ShowInCompanionAsync();
        }
    }
    public interface IDocumentGenerator
    {
        public Task GenerateInvoice(string? clientName, List<(string name, int qty, decimal price)>? items);
    }
}