using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Inventory.Entities;

public class Category : Profile
{
    public int ParentId { get; set; }
    public List<Item> Items { get; } = [];
}
