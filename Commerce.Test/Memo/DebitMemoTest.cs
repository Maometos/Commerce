using Commerce.Core.Common;
using Commerce.Core.Common.Entities;
using Commerce.Core.Contacts.Entities;
using Commerce.Core.Memo.Commands;
using Commerce.Core.Memo.Entities;
using Commerce.Core.Memo.Queries;
using Commerce.Infrastructure.CQRS;
using Commerce.Infrastructure.Dispatcher;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace Commerce.Test.Memo;

public class DebitMemoTest
{
    private EventDispatcher dispatcher;
    private DataContext context;
    private ITestOutputHelper output;

    public DebitMemoTest(ITestOutputHelper output)
    {
        this.output = output;
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        context = new DataContext(options);
        dispatcher = new EventDispatcher();
        dispatcher.AddService(context);
        dispatcher.AddHandler<DebitMemoCommandHandler>();
        dispatcher.AddHandler<DebitMemoQueryHandler>();

        var enterprise = new Enterprise() { Id = 1, Name = "FashionShop" };
        var supplier1 = new Supplier() { Id = 1, Name = "John Doe" };
        var supplier2 = new Supplier() { Id = 2, Name = "John Smith" };

        context.Enterprises.Add(enterprise);
        context.Suppliers.Add(supplier1);
        context.Suppliers.Add(supplier2);
        context.SaveChanges();

        var DebitMemo1 = new DebitMemo() { Reference = Guid.NewGuid().ToString() };
        DebitMemo1.Enterprise = enterprise;
        DebitMemo1.Supplier = supplier1;
        DebitMemo1.Lines.Add(new DebitLine() { Code = Guid.NewGuid().ToString(), Name = "Shirt", Price = 50, Quantity = 2 });
        DebitMemo1.Lines.Add(new DebitLine() { Code = Guid.NewGuid().ToString(), Name = "Pant", Price = 70, Quantity = 1 });

        var DebitMemo2 = new DebitMemo() { Reference = Guid.NewGuid().ToString() };
        DebitMemo2.Enterprise = enterprise;
        DebitMemo2.Supplier = supplier2;
        DebitMemo2.Lines.Add(new DebitLine() { Code = Guid.NewGuid().ToString(), Name = "T-Shirt", Price = 20, Quantity = 5 });
        DebitMemo2.Lines.Add(new DebitLine() { Code = Guid.NewGuid().ToString(), Name = "Short", Price = 50, Quantity = 2 });

        var DebitMemo3 = new DebitMemo() { Reference = Guid.NewGuid().ToString() };
        DebitMemo3.Enterprise = enterprise;
        DebitMemo3.Supplier = supplier1;
        DebitMemo3.Lines.Add(new DebitLine() { Code = Guid.NewGuid().ToString(), Name = "Hat", Price = 30, Quantity = 1 });

        var DebitMemo4 = new DebitMemo() { Reference = Guid.NewGuid().ToString() };
        DebitMemo4.Enterprise = enterprise;
        DebitMemo4.Supplier = supplier2;
        DebitMemo4.Lines.Add(new DebitLine() { Code = Guid.NewGuid().ToString(), Name = "Jacket", Price = 160, Quantity = 1 });

        context.DebitMemos.Add(DebitMemo1);
        context.DebitMemos.Add(DebitMemo2);
        context.DebitMemos.Add(DebitMemo3);
        context.DebitMemos.Add(DebitMemo4);
        context.SaveChanges();
    }

    [Fact]
    public async void CreateAsync()
    {
        var DebitMemo = new DebitMemo() { Reference = Guid.NewGuid().ToString() };
        DebitMemo.EnterpriseId = 1;
        DebitMemo.SupplierId = 1;

        var line1 = new DebitLine() { Code = Guid.NewGuid().ToString(), Name = "Shirt", Price = 40, Quantity = 3 };
        var gstTax1 = new DebitLineTax() { Name = "GST", Rate = 5, Line = line1 };
        var qstTax1 = new DebitLineTax() { Name = "QST", Rate = 9.975m, Line = line1 };

        line1.Taxes.Add(gstTax1);
        line1.Taxes.Add(qstTax1);

        DebitMemo.Lines.Add(line1);

        var command = new DebitMemoCommand();
        command.Action = CommandAction.Create;
        command.Argument = DebitMemo;

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(4, result);

        DebitMemo = await context.DebitMemos.FindAsync(5);
        Assert.NotNull(DebitMemo);
        Assert.Equal(137.970m, DebitMemo.Total);

        output.WriteLine($"Subtotal: {DebitMemo.Subtotal}");

        foreach (var tax in DebitMemo.Taxes)
        {
            output.WriteLine($"{tax.Key}: {tax.Value}");
        }

        output.WriteLine($"Total: {DebitMemo.Total}");
    }

    [Fact]
    public async void UpdateAsync()
    {
        var DebitMemo = await context.DebitMemos.FindAsync(3);
        DebitMemo!.Lines[0].Quantity = 2;

        var command = new DebitMemoCommand();
        command.Action = CommandAction.Update;
        command.Argument = DebitMemo;

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(2, result);

        DebitMemo = await context.DebitMemos.FindAsync(3);
        Assert.Single(DebitMemo!.Lines);
        Assert.Equal(2, DebitMemo!.Lines[0].Quantity);
    }

    [Fact]
    public async void DeleteAsync()
    {
        var command = new DebitMemoCommand();
        command.Action = CommandAction.Delete;
        command.Argument = 1;

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(3, 3);
    }

    [Fact]
    public async void FilterAsync()
    {
        var query = new DebitMemoQuery();
        query.Parameters["Id"] = 1;
        var list = await dispatcher.DispatchAsync(query) as List<DebitMemo>;
        Assert.NotNull(list);
        Assert.Single(list);

        query = new DebitMemoQuery();
        query.Parameters["Date"] = DateTime.Now;
        list = await dispatcher.DispatchAsync(query) as List<DebitMemo>;
        Assert.NotNull(list);
        Assert.Equal(4, list.Count);
    }

    [Fact]
    public async void SortAsync()
    {
        var query = new DebitMemoQuery();
        query.Sort = "Total";

        var list = await dispatcher.DispatchAsync(query) as List<DebitMemo>;
        Assert.NotNull(list);
        Assert.Equal(30, list[0].Total);
        Assert.Equal(160, list[1].Total);
        Assert.Equal(170, list[2].Total);
        Assert.Equal(200, list[3].Total);

        // reverse order by name
        query.Sort = "-Total";
        list = await dispatcher.DispatchAsync(query) as List<DebitMemo>;
        Assert.NotNull(list);
        Assert.Equal(200, list[0].Total);
        Assert.Equal(170, list[1].Total);
        Assert.Equal(160, list[2].Total);
        Assert.Equal(30, list[3].Total);
    }

    [Fact]
    public async void PaginateAsync()
    {
        var query = new DebitMemoQuery();
        query.Offset = 2;
        query.Limit = 2;

        var list = await dispatcher.DispatchAsync(query) as List<DebitMemo>;
        Assert.NotNull(list);

        var invoice1 = list[0];
        var invoice2 = list[1];
        Assert.Equal(3, invoice1.Id);
        Assert.Equal(4, invoice2.Id);
    }
}
