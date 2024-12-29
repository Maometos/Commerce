using Commerce.Core.Common.Values;

namespace Commerce.Core.Common.Abstractions;

public abstract class Adjustment<TLine, TTax> : TransactionAggregate<TLine, TTax> where TLine : LineItem<TTax> where TTax : LineTax
{
    public AdjustmentStatus Status { get; set; } = AdjustmentStatus.Draft;
}
