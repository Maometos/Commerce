using Commerce.Core.Common.Values;

namespace Commerce.Core.Common.Abstractions;

public abstract class Statement<TLine, TTax, TPayment> : Transaction<TLine, TTax> where TLine : LineItem<TTax> where TTax : LineTax where TPayment : Payment
{
    public List<TPayment> Payments { get; } = [];

    public DateTime DueDate { get; set; }
    public PaymentStatus PaymentStatus
    {
        get
        {
            if (Balance <= 0)
            {
                return PaymentStatus.Paid;
            }

            if (Balance < Total && Balance > 0)
            {
                return PaymentStatus.PartPaid;
            }

            return PaymentStatus.Unpaid;
        }
        set => PaymentStatus = value;
    }

    public decimal Balance
    {
        get
        {
            decimal Paid = 0;
            foreach (var payment in Payments)
            {
                Paid += payment.Amount;
            }

            return Total - Paid;
        }

        set => Balance = value;
    }
}
