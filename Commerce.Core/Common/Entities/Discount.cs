using Commerce.Core.Inventory.Entities;

namespace Commerce.Core.Common.Entities;

public class Discount
{
    public int Id { get; set; }
    public int ItemId { get; set; }
    public int MinQuantity { get; set; }
    public double Rate { get; set; }

    public List<Item> Items { get; } = [];
}
