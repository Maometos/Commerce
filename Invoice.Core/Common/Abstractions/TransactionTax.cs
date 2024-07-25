using Invoice.Core.Organization.Entities;

namespace Invoice.Core.Common.Abstractions;

public abstract class TransactionTax
{
    public int Id { get; set; }
    public int TaxId { get; set; }
    public string Name { get; set; } = null!;
    public double Rate { get; set; }
    public decimal Amount { get; set; }

    public Tax Tax { get; set; } = null!;
}
