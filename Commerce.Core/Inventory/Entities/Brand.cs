using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Inventory.Entities;

public class Brand : Profile
{
    public List<Item> Items { get; } = [];
}
