﻿using Commerce.Core.Common.Values;

namespace Commerce.Core.Common.Abstractions;

public abstract class Payment : Document
{
    public decimal InvoiceAmount { get; set; }
    public decimal PaymentAmount { get; set; }
    public PaymentMode PaymentMode { get; set; } = PaymentMode.Cash;
}
