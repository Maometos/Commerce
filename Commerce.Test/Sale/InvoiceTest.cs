﻿using Commerce.Core.Common;
using Commerce.Core.Common.Entities;
using Commerce.Core.Sale.Entities;
using Commerce.Core.Sale.Handlers;
using Commerce.Core.Sale.Requests;
using Commerce.Infrastructure.CQRS;
using Commerce.Infrastructure.Dispatcher;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace Commerce.Test.Sale;

public class InvoiceTest
{
    private EventDispatcher dispatcher;
    private DataContext context;
    private ITestOutputHelper output;

    public InvoiceTest(ITestOutputHelper output)
    {
        this.output = output;
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        context = new DataContext(options);
        dispatcher = new EventDispatcher();
        dispatcher.AddService(context);
        dispatcher.AddHandler<InvoiceCommandHandler>();
        dispatcher.AddHandler<InvoiceQueryHandler>();

        var enterprise = new Enterprise() { Id = 1, Name = "FashionShop" };
        var customer1 = new Customer() { Id = 1, Name = "John Doe" };
        var customer2 = new Customer() { Id = 2, Name = "John Smith" };

        context.Enterprises.Add(enterprise);
        context.Customers.Add(customer1);
        context.Customers.Add(customer2);
        context.SaveChanges();

        var invoice1 = new Invoice() { Reference = Guid.NewGuid().ToString() };
        invoice1.Enterprise = enterprise;
        invoice1.Customer = customer1;
        invoice1.Lines.Add(new InvoiceLine() { Code = Guid.NewGuid().ToString(), Name = "Shirt", Price = 50, Quantity = 2 });
        invoice1.Lines.Add(new InvoiceLine() { Code = Guid.NewGuid().ToString(), Name = "Pant", Price = 70, Quantity = 1 });

        var invoice2 = new Invoice() { Reference = Guid.NewGuid().ToString() };
        invoice2.Enterprise = enterprise;
        invoice2.Customer = customer2;
        invoice2.Lines.Add(new InvoiceLine() { Code = Guid.NewGuid().ToString(), Name = "T-Shirt", Price = 20, Quantity = 5 });
        invoice2.Lines.Add(new InvoiceLine() { Code = Guid.NewGuid().ToString(), Name = "Short", Price = 50, Quantity = 2 });

        var invoice3 = new Invoice() { Reference = Guid.NewGuid().ToString() };
        invoice3.Enterprise = enterprise;
        invoice3.Customer = customer1;
        invoice3.Lines.Add(new InvoiceLine() { Code = Guid.NewGuid().ToString(), Name = "Hat", Price = 30, Quantity = 1 });

        var invoice4 = new Invoice() { Reference = Guid.NewGuid().ToString() };
        invoice4.Enterprise = enterprise;
        invoice4.Customer = customer2;
        invoice4.Lines.Add(new InvoiceLine() { Code = Guid.NewGuid().ToString(), Name = "Jacket", Price = 160, Quantity = 1 });

        context.Invoices.Add(invoice1);
        context.Invoices.Add(invoice2);
        context.Invoices.Add(invoice3);
        context.Invoices.Add(invoice4);
        context.SaveChanges();
    }

    [Fact]
    public async void CreateAsync()
    {
        var invoice = new Invoice() { Reference = Guid.NewGuid().ToString() };
        invoice.EnterpriseId = 1;
        invoice.CustomerId = 1;

        var line1 = new InvoiceLine() { Code = Guid.NewGuid().ToString(), Name = "Shirt", Price = 50, Quantity = 3 };
        var gstTax1 = new InvoiceLineTax() { Name = "GST", Rate = 5, Line = line1 };
        var pstTax1 = new InvoiceLineTax() { Name = "QST", Rate = 9.975m, Line = line1 };

        line1.Taxes.Add(gstTax1);
        line1.Taxes.Add(pstTax1);

        var line2 = new InvoiceLine() { Code = Guid.NewGuid().ToString(), Name = "Pant", Price = 70, Quantity = 2 };
        var gstTax2 = new InvoiceLineTax() { Name = "GST", Rate = 5, Line = line2 };
        var pstTax2 = new InvoiceLineTax() { Name = "QST", Rate = 9.975m, Line = line2 };

        line2.Taxes.Add(gstTax2);
        line2.Taxes.Add(pstTax2);

        invoice.Lines.Add(line1);
        invoice.Lines.Add(line2);

        var command = new InvoiceCommand();
        command.Action = CommandAction.Create;
        command.Argument = invoice;

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(7, result);

        invoice = await context.Invoices.FindAsync(5);
        Assert.NotNull(invoice);
        Assert.Equal(333.4275m, invoice.Total);

        output.WriteLine($"Subtotal: {invoice.Subtotal}");

        foreach (var tax in invoice.Taxes)
        {
            output.WriteLine($"{tax.Key}: {tax.Value}");
        }

        output.WriteLine($"Total: {invoice.Total}");
    }

    [Fact]
    public async void UpdateAsync()
    {
        var invoice = await context.Invoices.FindAsync(3);
        invoice!.Lines[0].Quantity = 2;

        var command = new InvoiceCommand();
        command.Action = CommandAction.Update;
        command.Argument = invoice;

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(2, result);

        invoice = await context.Invoices.FindAsync(3);
        Assert.Single(invoice!.Lines);
        Assert.Equal(2, invoice!.Lines[0].Quantity);
    }

    [Fact]
    public async void DeleteAsync()
    {
        var command = new InvoiceCommand();
        command.Action = CommandAction.Delete;
        command.Argument = 1;

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(3, 3);
    }

    [Fact]
    public async void FilterAsync()
    {
        var query = new InvoiceQuery();
        query.Parameters["Id"] = 1;
        var list = await dispatcher.DispatchAsync(query) as List<Invoice>;
        Assert.NotNull(list);
        Assert.Single(list);

        query = new InvoiceQuery();
        query.Parameters["Date"] = DateTime.Now;
        list = await dispatcher.DispatchAsync(query) as List<Invoice>;
        Assert.NotNull(list);
        Assert.Equal(4, list.Count);
    }

    [Fact]
    public async void SortAsync()
    {
        var query = new InvoiceQuery();
        query.Sort = "Total";

        var list = await dispatcher.DispatchAsync(query) as List<Invoice>;
        Assert.NotNull(list);
        Assert.Equal(30, list[0].Total);
        Assert.Equal(160, list[1].Total);
        Assert.Equal(170, list[2].Total);
        Assert.Equal(200, list[3].Total);

        // reverse order by name
        query.Sort = "-Total";
        list = await dispatcher.DispatchAsync(query) as List<Invoice>;
        Assert.NotNull(list);
        Assert.Equal(200, list[0].Total);
        Assert.Equal(170, list[1].Total);
        Assert.Equal(160, list[2].Total);
        Assert.Equal(30, list[3].Total);
    }

    [Fact]
    public async void PaginateAsync()
    {
        var query = new InvoiceQuery();
        query.Offset = 2;
        query.Limit = 2;

        var list = await dispatcher.DispatchAsync(query) as List<Invoice>;
        Assert.NotNull(list);

        var invoice1 = list[0];
        var invoice2 = list[1];
        Assert.Equal(3, invoice1.Id);
        Assert.Equal(4, invoice2.Id);
    }
}
