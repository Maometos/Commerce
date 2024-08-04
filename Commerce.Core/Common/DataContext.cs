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
    public DbSet<SaleInvoice> SaleInvoices { get; set; }
    public DbSet<SaleInvoiceLine> SaleInvoiceLines { get; set; }
    public DbSet<SaleInvoiceTax> SaleInvoiceTaxes { get; set; }
    public DbSet<Vendor> Vendors { get; set; }
    public DbSet<PurchaseInvoice> PurchaseInvoices { get; set; }
    public DbSet<PurchaseInvoiceLine> PurchaseInvoiceLines { get; set; }
    public DbSet<PurchaseInvoiceTax> PurchaseInvoiceTaxes { get; set; }

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }
}
