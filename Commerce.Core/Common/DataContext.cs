using Commerce.Core.Common.Entities;
using Commerce.Core.Inventory.Entities;
using Commerce.Core.Organization.Entities;
using Commerce.Core.Purchase.Entities;
using Commerce.Core.Sale.Entities;
using Microsoft.EntityFrameworkCore;

namespace Commerce.Core.Common;

public class DataContext : DbContext
{
    public DbSet<Enterprise> Enterprises { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Discount> Discounts { get; set; }
    public DbSet<Tax> Taxes { get; set; }
    public DbSet<TaxRate> TaxRates { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<InvoiceLine> InvoiceLines { get; set; }
    public DbSet<InvoiceTax> InvoiceTaxes { get; set; }
    public DbSet<SalePayment> SalePayments { get; set; }
    public DbSet<CreditNote> CreditNotes { get; set; }
    public DbSet<CreditLine> CreditLines { get; set; }
    public DbSet<CreditTax> CreditTaxes { get; set; }
    public DbSet<Vendor> Vendors { get; set; }
    public DbSet<Bill> Bills { get; set; }
    public DbSet<BillLine> BillLines { get; set; }
    public DbSet<BillTax> BillTaxes { get; set; }
    public DbSet<PurchasePayment> PurchasePayments { get; set; }
    public DbSet<DebitNote> DebitNotes { get; set; }

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }
}
