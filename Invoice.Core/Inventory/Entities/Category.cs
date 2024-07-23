﻿namespace Invoice.Core.Inventory.Entities;

public class Category
{
    public int Id { get; set; }
    public int ParentId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}
