using Commerce.Core.Common.Values;

namespace Commerce.Core.Common.Abstractions;

public abstract class Payment : Transaction
{
    public decimal InvoiceAmount { get; set; }
    public decimal PaymentAmount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
}
