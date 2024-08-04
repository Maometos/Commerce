using Commerce.Core.Inventory.Entities;
using Commerce.Core.Organization.Entities;

namespace Commerce.Core.Common.Abstractions;

public abstract class TransactionLine
{
    public int Id { get; set; }
    public int ItemId { get; set; }
    public int TaxId { get; set; }
    public string? Code { get; set; }
    public string Name { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public double Discount { get; set; }
    public decimal Amount { get; set; }

    public Item Item { get; set; } = null!;
    public Tax Tax { get; set; } = null!;
}
