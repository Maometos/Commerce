using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Inventory.Entities;

public class Brand : Entity
{
    public List<Item> Items { get; } = [];
}
