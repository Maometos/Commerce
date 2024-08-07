﻿using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Sale.Entities;

public class SaleInvoice : Statement
{
    public int CustomerId { get; set; }

    public Customer Customer { get; set; } = null!;
    public List<SaleInvoiceLine> Lines { get; } = [];
    public List<SaleInvoiceTax> Taxes { get; } = [];
    public List<SalePayment> Payments { get; } = [];
}
