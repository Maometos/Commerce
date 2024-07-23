namespace Invoice.Core.Inventory.Entities;

public class Item
{
    public int Id { get; set; }
    public int BrandId { get; set; }
    public int CategoryId { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Unit { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public decimal Cost { get; set; }

    public Brand? Brand { get; set; }
    public Category? Category { get; set; }
    public List<Discount> Discounts { get; } = [];
}
