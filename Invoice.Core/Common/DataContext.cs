using Invoice.Core.Common.Entities;
using Invoice.Core.Inventory.Entities;
using Invoice.Core.Organization.Entities;
using Invoice.Core.Purchase.Entities;
using Invoice.Core.Sale.Entities;
using Microsoft.EntityFrameworkCore;

namespace Invoice.Core.Common;

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
    public DbSet<SaleInvoice> SaleInvoices { get; set; }
    public DbSet<SaleInvoiceLine> SaleInvoiceLines { get; set; }
    public DbSet<SaleInvoiceTax> SaleInvoiceTaxes { get; set; }
    public DbSet<Vendor> Vendors { get; set; }
    public DbSet<PurchaseInvoice> PurchaseInvoices { get; set; }
    public DbSet<PurchaseInvoiceLine> PurchaseInvoiceLines { get; set; }

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }
}
