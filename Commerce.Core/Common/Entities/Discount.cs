using Commerce.Core.Inventory.Entities;
using Commerce.Infrastructure.CQRS;

namespace Commerce.Core.Common.Entities;

public class Discount : Entity
{
    public int ItemId { get; set; }
    public int MinQuantity { get; set; }
    public double Rate { get; set; }

    public List<Item> Items { get; } = [];
}
