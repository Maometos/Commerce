using Commerce.Core.Common.Abstractions;
using Commerce.Core.Common.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Commerce.Core.Inventory.Entities;

public class Item : Profile
{
    public int BrandId { get; set; }
    public int CategoryId { get; set; }
    public int DiscountId { get; set; }
    public int SaleTaxId { get; set; }
    public int PurchaseTaxId { get; set; }
    public string Code { get; set; } = null!;
    public string? Unit { get; set; }
    public decimal Price { get; set; }
    public decimal Cost { get; set; }

    public Brand? Brand { get; set; }
    public Category? Category { get; set; }
    public Discount? Discount { get; set; }

    [ForeignKey(nameof(SaleTaxId))]
    public TaxGroup? SateTax { get; set; }

    [ForeignKey(nameof(PurchaseTaxId))]
    public TaxGroup? PurchaseTax { get; set; }
}
