namespace Commerce.Core.Common.Abstractions;

public abstract class LineTax
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public double Rate { get; set; }
    public decimal Amount { get; set; }
}
