using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestPDFSampleProject
{
    public static class ComplexDocExample
    {
        public static void StartGenerationProcess()
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            // Sample data
            var report = new ReportData
            {
                ReportNumber = "INV-2025-0042",
                IssueDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(30),
                CompanyDetails = new CompanyDetails
                {
                    Name = "Acme Corporation",
                    Address = "123 Business Road",
                    City = "Business City, BC 12345",
                    Email = "invoices@acmecorp.com",
                    Phone = "(555) 123-4567"
                },
                CustomerDetails = new CustomerDetails
                {
                    Name = "John Smith",
                    Address = "456 Customer Street",
                    City = "Customer City, CC 67890",
                    Email = "john.smith@example.com",
                    CustomerSince = new DateTime(2022, 5, 12)
                },
                Items = new List<InvoiceItem>
                {
                    new InvoiceItem { Id = "PROD-001", Name = "Premium Laptop", Quantity = 1, UnitPrice = 1299.99m },
                    new InvoiceItem { Id = "PROD-002", Name = "Wireless Mouse", Quantity = 2, UnitPrice = 29.99m },
                    new InvoiceItem { Id = "PROD-003", Name = "External SSD 1TB", Quantity = 1, UnitPrice = 159.99m },
                    new InvoiceItem { Id = "SERV-001", Name = "Extended Warranty", Quantity = 1, UnitPrice = 199.99m },
                    new InvoiceItem { Id = "SERV-002", Name = "Technical Support (Monthly)", Quantity = 12, UnitPrice = 19.99m }
                },
                Notes = "Payment is due within 30 days. Late payments are subject to a 1.5% monthly fee."
            };

            // Create and save the document
            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(20);
                    page.Background(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header().Element(ComposeHeader);

                    page.Content().Element(content =>
                    {
                        ComposeContent(content, report);
                    });

                });
            }).GeneratePdfAndShow();

        }

        // Helper methods for composing the document sections
        static void ComposeHeader(IContainer container)
        {
            container.Row(row =>
            {
                // Company logo area (left)
                row.RelativeItem().Column(column =>
                {
                    column.Item().Container().Height(50).Background(Colors.Grey.Medium)
                        .AlignCenter().AlignMiddle().Text("COMPANY LOGO").FontSize(20).FontColor(Colors.White);

                    column.Item().Text("Your Trusted Business Partner").FontSize(12).Italic();
                });

                // Title area (right)
                row.RelativeItem().Column(column =>
                {
                    column.Item().AlignRight().Text("INVOICE").FontSize(24).Bold().FontColor(Colors.Blue.Medium);
                    column.Item().AlignRight().Text(text =>
                    {
                        text.Span("Date: ").SemiBold();
                        text.Span($"{DateTime.Now:MMMM dd, yyyy}");
                    });
                    column.Item().AlignRight().Text(text =>
                    {
                        text.Span("Invoice #: ").SemiBold();
                        text.Span("INV-2025-0042");
                    });
                });
            });
        }

        static void ComposeContent(IContainer container, ReportData report)
        {
            container.PaddingVertical(20).Column(column =>
            {
                column.Item().Row(row =>
                {
                    ComposeCompanyInfo(row.RelativeItem());

                    ComposeCustomerInfo(row.RelativeItem());
                });
            });
        }

        static void ComposeCompanyInfo(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().Background(Colors.Blue.Medium)
                    .Padding(5).Text("FROM").FontColor(Colors.White).Bold();

                column.Item().PaddingLeft(5).PaddingTop(5).Text("Acme Corporation").Bold();
                column.Item().PaddingLeft(5).Text("123 Business Road");
                column.Item().PaddingLeft(5).Text("Business City, BC 12345");
                column.Item().PaddingLeft(5).Text(text =>
                {
                    text.Span("Email: ").SemiBold();
                    text.Span("invoices@acmecorp.com");
                });
                column.Item().PaddingLeft(5).Text(text =>
                {
                    text.Span("Phone: ").SemiBold();
                    text.Span("(555) 123-4567");
                });
            });
        }

        static void ComposeCustomerInfo(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().Background(Colors.Blue.Medium)
                    .Padding(5).Text("TO").FontColor(Colors.White).Bold();

                column.Item().PaddingLeft(5).PaddingTop(5).Text("John Smith").Bold();
                column.Item().PaddingLeft(5).Text("456 Customer Street");
                column.Item().PaddingLeft(5).Text("Customer City, CC 67890");
                column.Item().PaddingLeft(5).Text(text =>
                {
                    text.Span("Email: ").SemiBold();
                    text.Span("john.smith@example.com");
                });
                column.Item().PaddingLeft(5).Text(text =>
                {
                    text.Span("Customer since: ").SemiBold();
                    text.Span("May 12, 2022");
                });
            });
        }


    }

    // Data model classes
    public class ReportData
    {
        public string ReportNumber { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public CompanyDetails CompanyDetails { get; set; }
        public CustomerDetails CustomerDetails { get; set; }
        public List<InvoiceItem> Items { get; set; }
        public string Notes { get; set; }
    }

    public class CompanyDetails
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }

    public class CustomerDetails
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public DateTime CustomerSince { get; set; }
    }

    public class InvoiceItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => Quantity * UnitPrice;
    }

}
