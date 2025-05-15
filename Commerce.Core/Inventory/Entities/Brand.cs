using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Inventory.Entities;

public class Brand : Definition
{
    public List<Item> Items { get; } = [];
}
