using Commerce.Core.Common.Entities;
using Commerce.Core.Contacts.Entities;
using Commerce.Core.Inventory.Entities;
using Commerce.Core.Invoice.Entities;
using Commerce.Core.Memo.Entities;
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
    public DbSet<TaxGroup> TaxGroups { get; set; }
    public DbSet<TaxRate> TaxRates { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<SalesInvoice> SaleInvoices { get; set; }
    public DbSet<SalesInvoiceLine> SaleInvoiceLines { get; set; }
    public DbSet<SalesInvoiceLineTax> SaleInvoiceLineTaxes { get; set; }
    public DbSet<PaymentReceipt> PaymentReceipts { get; set; }
    public DbSet<CreditMemo> CreditMemos { get; set; }
    public DbSet<CreditLine> CreditLines { get; set; }
    public DbSet<CreditLineTax> CreditLineTaxes { get; set; }
    public DbSet<RefundAdvice> RefundAdvices { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<PurchaseInvoice> PurchaseInvoices { get; set; }
    public DbSet<PurchaseInvoiceLine> PurchaseInvoiceLines { get; set; }
    public DbSet<PurchaseInvoiceLineTax> PurchaseInvoiceLineTaxes { get; set; }
    public DbSet<PaymentAdvice> PaymentAdvices { get; set; }
    public DbSet<DebitMemo> DebitMemos { get; set; }
    public DbSet<DebitLine> DebitLines { get; set; }
    public DbSet<DebitLineTax> DebitLineTaxes { get; set; }
    public DbSet<RefundReceipt> RefundReceipts { get; set; }

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }
}
