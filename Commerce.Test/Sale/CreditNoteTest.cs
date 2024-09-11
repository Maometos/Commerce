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

public class CreditNoteTest
{
    private EventDispatcher dispatcher;
    private DataContext context;
    private ITestOutputHelper output;

    public CreditNoteTest(ITestOutputHelper output)
    {
        this.output = output;
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        context = new DataContext(options);
        dispatcher = new EventDispatcher();
        dispatcher.AddService(context);
        dispatcher.AddHandler<CreditNoteCommandHandler>();
        dispatcher.AddHandler<CreditNoteQueryHandler>();

        var enterprise = new Enterprise() { Id = 1, Name = "FashionShop" };
        var customer1 = new Customer() { Id = 1, Name = "John Doe" };
        var customer2 = new Customer() { Id = 2, Name = "John Smith" };

        context.Enterprises.Add(enterprise);
        context.Customers.Add(customer1);
        context.Customers.Add(customer2);
        context.SaveChanges();

        var creditNote1 = new CreditNote() { Reference = Guid.NewGuid().ToString() };
        creditNote1.Enterprise = enterprise;
        creditNote1.Customer = customer1;
        creditNote1.Lines.Add(new CreditLine() { Code = Guid.NewGuid().ToString(), Name = "Shirt", Price = 50, Quantity = 2 });
        creditNote1.Lines.Add(new CreditLine() { Code = Guid.NewGuid().ToString(), Name = "Pant", Price = 70, Quantity = 1 });

        var creditNote2 = new CreditNote() { Reference = Guid.NewGuid().ToString() };
        creditNote2.Enterprise = enterprise;
        creditNote2.Customer = customer2;
        creditNote2.Lines.Add(new CreditLine() { Code = Guid.NewGuid().ToString(), Name = "T-Shirt", Price = 20, Quantity = 5 });
        creditNote2.Lines.Add(new CreditLine() { Code = Guid.NewGuid().ToString(), Name = "Short", Price = 50, Quantity = 2 });

        var creditNote3 = new CreditNote() { Reference = Guid.NewGuid().ToString() };
        creditNote3.Enterprise = enterprise;
        creditNote3.Customer = customer1;
        creditNote3.Lines.Add(new CreditLine() { Code = Guid.NewGuid().ToString(), Name = "Hat", Price = 30, Quantity = 1 });

        var creditNote4 = new CreditNote() { Reference = Guid.NewGuid().ToString() };
        creditNote4.Enterprise = enterprise;
        creditNote4.Customer = customer2;
        creditNote4.Lines.Add(new CreditLine() { Code = Guid.NewGuid().ToString(), Name = "Jacket", Price = 160, Quantity = 1 });

        context.CreditNotes.Add(creditNote1);
        context.CreditNotes.Add(creditNote2);
        context.CreditNotes.Add(creditNote3);
        context.CreditNotes.Add(creditNote4);
        context.SaveChanges();
    }

    [Fact]
    public async void CreateAsync()
    {
        var creditNote = new CreditNote() { Reference = Guid.NewGuid().ToString() };
        creditNote.EnterpriseId = 1;
        creditNote.CustomerId = 1;

        var line1 = new CreditLine() { Code = Guid.NewGuid().ToString(), Name = "Shirt", Price = 50, Quantity = 3 };
        var gstTax1 = new CreditLineTax() { Name = "GST", Rate = 5, Line = line1 };
        var qstTax1 = new CreditLineTax() { Name = "QST", Rate = 9.975m, Line = line1 };

        line1.Taxes.Add(gstTax1);
        line1.Taxes.Add(qstTax1);

        creditNote.Lines.Add(line1);

        var command = new CreditNoteCommand();
        command.Action = CommandAction.Create;
        command.Argument = creditNote;

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(4, result);

        creditNote = await context.CreditNotes.FindAsync(5);
        Assert.NotNull(creditNote);
        Assert.Equal(172.4625m, creditNote.Total);

        output.WriteLine($"Subtotal: {creditNote.Subtotal}");

        foreach (var tax in creditNote.Taxes)
        {
            output.WriteLine($"{tax.Key}: {tax.Value}");
        }

        output.WriteLine($"Total: {creditNote.Total}");
    }

    [Fact]
    public async void UpdateAsync()
    {
        var creditNote = await context.CreditNotes.FindAsync(3);
        creditNote!.Lines[0].Quantity = 2;

        var command = new CreditNoteCommand();
        command.Action = CommandAction.Update;
        command.Argument = creditNote;

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(2, result);

        creditNote = await context.CreditNotes.FindAsync(3);
        Assert.Single(creditNote!.Lines);
        Assert.Equal(2, creditNote!.Lines[0].Quantity);
    }

    [Fact]
    public async void DeleteAsync()
    {
        var command = new CreditNoteCommand();
        command.Action = CommandAction.Delete;
        command.Argument = 1;

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(3, result);
    }

    [Fact]
    public async void FilterAsync()
    {
        var query = new CreditNoteQuery();
        query.Parameters["Id"] = 1;
        var list = await dispatcher.DispatchAsync(query) as List<CreditNote>;
        Assert.NotNull(list);
        Assert.Single(list);

        query = new CreditNoteQuery();
        query.Parameters["Date"] = DateTime.Now;
        list = await dispatcher.DispatchAsync(query) as List<CreditNote>;
        Assert.NotNull(list);
        Assert.Equal(4, list.Count);
    }

    [Fact]
    public async void SortAsync()
    {
        var query = new CreditNoteQuery();
        query.Sort = "Total";

        var list = await dispatcher.DispatchAsync(query) as List<CreditNote>;
        Assert.NotNull(list);
        Assert.Equal(30, list[0].Total);
        Assert.Equal(160, list[1].Total);
        Assert.Equal(170, list[2].Total);
        Assert.Equal(200, list[3].Total);

        // reverse order by name
        query.Sort = "-Total";
        list = await dispatcher.DispatchAsync(query) as List<CreditNote>;
        Assert.NotNull(list);
        Assert.Equal(200, list[0].Total);
        Assert.Equal(170, list[1].Total);
        Assert.Equal(160, list[2].Total);
        Assert.Equal(30, list[3].Total);
    }

    [Fact]
    public async void PaginateAsync()
    {
        var query = new CreditNoteQuery();
        query.Offset = 2;
        query.Limit = 2;

        var list = await dispatcher.DispatchAsync(query) as List<CreditNote>;
        Assert.NotNull(list);

        var invoice1 = list[0];
        var invoice2 = list[1];
        Assert.Equal(3, invoice1.Id);
        Assert.Equal(4, invoice2.Id);
    }
}
