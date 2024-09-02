using Commerce.Core.Common;
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
    public async void TestCreating()
    {
        var invoice = new Invoice() { Reference = Guid.NewGuid().ToString() };
        invoice.EnterpriseId = 1;
        invoice.CustomerId = 1;

        var line1 = new InvoiceLine() { Code = Guid.NewGuid().ToString(), Name = "Shirt", Price = 50, Quantity = 3 };
        var gstTax1 = new InvoiceLineTax() { Name = "GST", Rate = 5, Line = line1 };
        var pstTax1 = new InvoiceLineTax() { Name = "PST", Rate = 6, Line = line1 };

        line1.Taxes.Add(gstTax1);
        line1.Taxes.Add(pstTax1);

        var line2 = new InvoiceLine() { Code = Guid.NewGuid().ToString(), Name = "Pant", Price = 70, Quantity = 2 };
        var gstTax2 = new InvoiceLineTax() { Name = "GST", Rate = 5, Line = line2 };
        var pstTax2 = new InvoiceLineTax() { Name = "PST", Rate = 6, Line = line2 };

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
        Assert.Equal(321.9m, invoice.Total);

        output.WriteLine($"Subtotal: {invoice.Subtotal}");

        foreach (var tax in invoice.Taxes)
        {
            output.WriteLine($"{tax.Key}: {tax.Value}");
        }

        output.WriteLine($"Total: {invoice.Total}");
    }

    [Fact]
    public async void TestUpdating()
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
    public async void TestDeleting()
    {
        var command = new InvoiceCommand();
        command.Action = CommandAction.Delete;
        command.Argument = 1;

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(3, 3);
    }
}
