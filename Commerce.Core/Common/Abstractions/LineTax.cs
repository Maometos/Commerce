namespace Commerce.Core.Common.Abstractions;

public abstract class LineTax
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Rate { get; set; }
}
