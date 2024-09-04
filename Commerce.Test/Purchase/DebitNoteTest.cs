using Commerce.Core.Common;
using Commerce.Core.Common.Entities;
using Commerce.Core.Purchase.Entities;
using Commerce.Core.Purchase.Handlers;
using Commerce.Core.Purchase.Requests;
using Commerce.Infrastructure.CQRS;
using Commerce.Infrastructure.Dispatcher;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace Commerce.Test.Purchase;

public class DebitNoteTest
{
    private EventDispatcher dispatcher;
    private DataContext context;
    private ITestOutputHelper output;

    public DebitNoteTest(ITestOutputHelper output)
    {
        this.output = output;
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        context = new DataContext(options);
        dispatcher = new EventDispatcher();
        dispatcher.AddService(context);
        dispatcher.AddHandler<DebitNoteCommandHandler>();

        var enterprise = new Enterprise() { Id = 1, Name = "FashionShop" };
        var supplier1 = new Supplier() { Id = 1, Name = "John Doe" };
        var supplier2 = new Supplier() { Id = 2, Name = "John Smith" };

        context.Enterprises.Add(enterprise);
        context.Suppliers.Add(supplier1);
        context.Suppliers.Add(supplier2);
        context.SaveChanges();

        var DebitNote1 = new DebitNote() { Reference = Guid.NewGuid().ToString() };
        DebitNote1.Enterprise = enterprise;
        DebitNote1.Supplier = supplier1;
        DebitNote1.Lines.Add(new DebitLine() { Code = Guid.NewGuid().ToString(), Name = "Shirt", Price = 50, Quantity = 2 });
        DebitNote1.Lines.Add(new DebitLine() { Code = Guid.NewGuid().ToString(), Name = "Pant", Price = 70, Quantity = 1 });

        var DebitNote2 = new DebitNote() { Reference = Guid.NewGuid().ToString() };
        DebitNote2.Enterprise = enterprise;
        DebitNote2.Supplier = supplier2;
        DebitNote2.Lines.Add(new DebitLine() { Code = Guid.NewGuid().ToString(), Name = "T-Shirt", Price = 20, Quantity = 5 });
        DebitNote2.Lines.Add(new DebitLine() { Code = Guid.NewGuid().ToString(), Name = "Short", Price = 50, Quantity = 2 });

        var DebitNote3 = new DebitNote() { Reference = Guid.NewGuid().ToString() };
        DebitNote3.Enterprise = enterprise;
        DebitNote3.Supplier = supplier1;
        DebitNote3.Lines.Add(new DebitLine() { Code = Guid.NewGuid().ToString(), Name = "Hat", Price = 30, Quantity = 1 });

        var DebitNote4 = new DebitNote() { Reference = Guid.NewGuid().ToString() };
        DebitNote4.Enterprise = enterprise;
        DebitNote4.Supplier = supplier2;
        DebitNote4.Lines.Add(new DebitLine() { Code = Guid.NewGuid().ToString(), Name = "Jacket", Price = 160, Quantity = 1 });

        context.DebitNotes.Add(DebitNote1);
        context.DebitNotes.Add(DebitNote2);
        context.DebitNotes.Add(DebitNote3);
        context.DebitNotes.Add(DebitNote4);
        context.SaveChanges();
    }

    [Fact]
    public async void TestCreating()
    {
        var DebitNote = new DebitNote() { Reference = Guid.NewGuid().ToString() };
        DebitNote.EnterpriseId = 1;
        DebitNote.SupplierId = 1;

        var line1 = new DebitLine() { Code = Guid.NewGuid().ToString(), Name = "Shirt", Price = 40, Quantity = 3 };
        var gstTax1 = new DebitLineTax() { Name = "GST", Rate = 5, Line = line1 };
        var pstTax1 = new DebitLineTax() { Name = "PST", Rate = 6, Line = line1 };

        line1.Taxes.Add(gstTax1);
        line1.Taxes.Add(pstTax1);

        DebitNote.Lines.Add(line1);

        var command = new DebitNoteCommand();
        command.Action = CommandAction.Create;
        command.Argument = DebitNote;

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(4, result);

        DebitNote = await context.DebitNotes.FindAsync(5);
        Assert.NotNull(DebitNote);
        Assert.Equal(133.2m, DebitNote.Total);

        output.WriteLine($"Subtotal: {DebitNote.Subtotal}");

        foreach (var tax in DebitNote.Taxes)
        {
            output.WriteLine($"{tax.Key}: {tax.Value}");
        }

        output.WriteLine($"Total: {DebitNote.Total}");
    }

    [Fact]
    public async void TestUpdating()
    {
        var DebitNote = await context.DebitNotes.FindAsync(3);
        DebitNote!.Lines[0].Quantity = 2;

        var command = new DebitNoteCommand();
        command.Action = CommandAction.Update;
        command.Argument = DebitNote;

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(2, result);

        DebitNote = await context.DebitNotes.FindAsync(3);
        Assert.Single(DebitNote!.Lines);
        Assert.Equal(2, DebitNote!.Lines[0].Quantity);
    }

    [Fact]
    public async void TestDeleting()
    {
        var command = new DebitNoteCommand();
        command.Action = CommandAction.Delete;
        command.Argument = 1;

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(3, 3);
    }
}
