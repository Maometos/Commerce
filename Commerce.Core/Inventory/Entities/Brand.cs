namespace Commerce.Core.Inventory.Entities;

public class Brand
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public List<Item> Items { get; } = [];
}
