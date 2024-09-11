using Commerce.Core.Common.Values;

namespace Commerce.Core.Common.Abstractions;

public abstract class Statement<TLine, TTax> : Transaction<TLine, TTax> where TLine : LineItem<TTax> where TTax : LineTax
{
    public DateTime DueDate { get; set; }
    public StatementStatus Status { get; set; } = StatementStatus.Draft;
}
