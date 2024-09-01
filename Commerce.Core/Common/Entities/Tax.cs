namespace Commerce.Core.Common.Entities;

public class Tax
{
    public int Id { get; set; }
    public int TaxId { get; set; }
    public string Name { get; set; } = null!;
    public decimal Rate { get; set; }

    public TaxGroup Group { get; set; } = null!;
}
