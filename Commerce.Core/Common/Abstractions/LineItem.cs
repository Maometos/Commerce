using Commerce.Core.Inventory.Entities;
using Commerce.Infrastructure.CQRS;

namespace Commerce.Core.Common.Abstractions;

public abstract class LineItem<TTax> : Entity where TTax : LineTax
{
    public int ItemId { get; set; }
    public string? Code { get; set; }
    public string Name { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Discount { get; set; }

    public Item Item { get; set; } = null!;
    public List<TTax> Taxes { get; set; } = [];
}
