namespace Commerce.Core.Organization.Entities;

public class Tax
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public List<TaxRate> Rates { get; } = [];
}
