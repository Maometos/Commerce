using Commerce.Core.Common;
using Commerce.Core.Common.Entities;
using Commerce.Core.Contacts.Entities;
using Commerce.Core.Memo.Commands;
using Commerce.Core.Memo.Entities;
using Commerce.Core.Memo.Handlers;
using Commerce.Core.Memo.Queries;
using Commerce.Infrastructure.CQRS;
using Commerce.Infrastructure.Dispatcher;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace Commerce.Test.Memo;

public class CreditMemoTest
{
    private EventDispatcher dispatcher;
    private DataContext context;
    private ITestOutputHelper output;

    public CreditMemoTest(ITestOutputHelper output)
    {
        this.output = output;
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        context = new DataContext(options);
        dispatcher = new EventDispatcher();
        dispatcher.AddService(context);
        dispatcher.AddHandler<CreditMemoCommandHandler>();
        dispatcher.AddHandler<CreditMemoQueryHandler>();

        var enterprise = new Enterprise() { Id = 1, Name = "FashionShop" };
        var customer1 = new Customer() { Id = 1, Name = "John Doe" };
        var customer2 = new Customer() { Id = 2, Name = "John Smith" };

        context.Enterprises.Add(enterprise);
        context.Customers.Add(customer1);
        context.Customers.Add(customer2);
        context.SaveChanges();

        var creditMemo1 = new CreditMemo() { Reference = Guid.NewGuid().ToString() };
        creditMemo1.Enterprise = enterprise;
        creditMemo1.Customer = customer1;
        creditMemo1.Lines.Add(new CreditLine() { Code = Guid.NewGuid().ToString(), Name = "Shirt", Price = 50, Quantity = 2 });
        creditMemo1.Lines.Add(new CreditLine() { Code = Guid.NewGuid().ToString(), Name = "Pant", Price = 70, Quantity = 1 });

        var creditMemo2 = new CreditMemo() { Reference = Guid.NewGuid().ToString() };
        creditMemo2.Enterprise = enterprise;
        creditMemo2.Customer = customer2;
        creditMemo2.Lines.Add(new CreditLine() { Code = Guid.NewGuid().ToString(), Name = "T-Shirt", Price = 20, Quantity = 5 });
        creditMemo2.Lines.Add(new CreditLine() { Code = Guid.NewGuid().ToString(), Name = "Short", Price = 50, Quantity = 2 });

        var creditMemo3 = new CreditMemo() { Reference = Guid.NewGuid().ToString() };
        creditMemo3.Enterprise = enterprise;
        creditMemo3.Customer = customer1;
        creditMemo3.Lines.Add(new CreditLine() { Code = Guid.NewGuid().ToString(), Name = "Hat", Price = 30, Quantity = 1 });

        var creditMemo4 = new CreditMemo() { Reference = Guid.NewGuid().ToString() };
        creditMemo4.Enterprise = enterprise;
        creditMemo4.Customer = customer2;
        creditMemo4.Lines.Add(new CreditLine() { Code = Guid.NewGuid().ToString(), Name = "Jacket", Price = 160, Quantity = 1 });

        context.CreditMemos.Add(creditMemo1);
        context.CreditMemos.Add(creditMemo2);
        context.CreditMemos.Add(creditMemo3);
        context.CreditMemos.Add(creditMemo4);
        context.SaveChanges();
    }

    [Fact]
    public async void CreateAsync()
    {
        var creditMemo = new CreditMemo() { Reference = Guid.NewGuid().ToString() };
        creditMemo.EnterpriseId = 1;
        creditMemo.CustomerId = 1;

        var line1 = new CreditLine() { Code = Guid.NewGuid().ToString(), Name = "Shirt", Price = 50, Quantity = 3 };
        var gstTax1 = new CreditLineTax() { Name = "GST", Rate = 5, Line = line1 };
        var qstTax1 = new CreditLineTax() { Name = "QST", Rate = 9.975m, Line = line1 };

        line1.Taxes.Add(gstTax1);
        line1.Taxes.Add(qstTax1);

        creditMemo.Lines.Add(line1);

        var command = new CreditMemoCommand();
        command.Action = CommandAction.Create;
        command.Argument = creditMemo;

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(4, result);

        creditMemo = await context.CreditMemos.FindAsync(5);
        Assert.NotNull(creditMemo);
        Assert.Equal(172.4625m, creditMemo.Total);

        output.WriteLine($"Subtotal: {creditMemo.Subtotal}");

        foreach (var tax in creditMemo.Taxes)
        {
            output.WriteLine($"{tax.Key}: {tax.Value}");
        }

        output.WriteLine($"Total: {creditMemo.Total}");
    }

    [Fact]
    public async void UpdateAsync()
    {
        var creditMemo = await context.CreditMemos.FindAsync(3);
        creditMemo!.Lines[0].Quantity = 2;

        var command = new CreditMemoCommand();
        command.Action = CommandAction.Update;
        command.Argument = creditMemo;

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(2, result);

        creditMemo = await context.CreditMemos.FindAsync(3);
        Assert.Single(creditMemo!.Lines);
        Assert.Equal(2, creditMemo!.Lines[0].Quantity);
    }

    [Fact]
    public async void DeleteAsync()
    {
        var command = new CreditMemoCommand();
        command.Action = CommandAction.Delete;
        command.Argument = 1;

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(3, result);
    }

    [Fact]
    public async void FilterAsync()
    {
        var query = new CreditMemoQuery();
        query.Parameters["Id"] = 1;
        var list = await dispatcher.DispatchAsync(query) as List<CreditMemo>;
        Assert.NotNull(list);
        Assert.Single(list);

        query = new CreditMemoQuery();
        query.Parameters["Date"] = DateTime.Now;
        list = await dispatcher.DispatchAsync(query) as List<CreditMemo>;
        Assert.NotNull(list);
        Assert.Equal(4, list.Count);
    }

    [Fact]
    public async void SortAsync()
    {
        var query = new CreditMemoQuery();
        query.Sort = "Total";

        var list = await dispatcher.DispatchAsync(query) as List<CreditMemo>;
        Assert.NotNull(list);
        Assert.Equal(30, list[0].Total);
        Assert.Equal(160, list[1].Total);
        Assert.Equal(170, list[2].Total);
        Assert.Equal(200, list[3].Total);

        // reverse order by name
        query.Sort = "-Total";
        list = await dispatcher.DispatchAsync(query) as List<CreditMemo>;
        Assert.NotNull(list);
        Assert.Equal(200, list[0].Total);
        Assert.Equal(170, list[1].Total);
        Assert.Equal(160, list[2].Total);
        Assert.Equal(30, list[3].Total);
    }

    [Fact]
    public async void PaginateAsync()
    {
        var query = new CreditMemoQuery();
        query.Offset = 2;
        query.Limit = 2;

        var list = await dispatcher.DispatchAsync(query) as List<CreditMemo>;
        Assert.NotNull(list);

        var invoice1 = list[0];
        var invoice2 = list[1];
        Assert.Equal(3, invoice1.Id);
        Assert.Equal(4, invoice2.Id);
    }
}
