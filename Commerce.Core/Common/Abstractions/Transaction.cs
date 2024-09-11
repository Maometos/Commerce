namespace Commerce.Core.Common.Abstractions;

public abstract class Transaction<TLine, TTax> : Document where TLine : LineItem<TTax> where TTax : LineTax
{
    public List<TLine> Lines { get; set; } = new List<TLine>();
    public decimal Subtotal => Lines.Sum(line => line.Quantity * line.Price);
    public decimal Total
    {
        get => Subtotal + Taxes.Values.Sum();
        set => Total = value;
    }
    public Dictionary<string, decimal> Taxes
    {
        get
        {
            var taxes = new Dictionary<string, decimal>();

            foreach (var line in Lines)
            {
                foreach (var tax in line.Taxes)
                {
                    if (!taxes.ContainsKey(tax.Name))
                    {
                        taxes[tax.Name] = 0;
                    }
                    taxes[tax.Name] += line.Quantity * line.Price * tax.Rate / 100;
                }
            }

            return taxes;
        }
    }
}
