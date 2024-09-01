namespace Commerce.Core.Common.Entities;

public class TaxGroup
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public List<Tax> Taxes { get; } = [];
}
