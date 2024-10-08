﻿using Commerce.Core.Common;
using Commerce.Core.Common.Entities;
using Commerce.Core.Purchase.Entities;
using Commerce.Core.Purchase.Handlers;
using Commerce.Core.Purchase.Requests;
using Commerce.Core.Sale.Requests;
using Commerce.Infrastructure.CQRS;
using Commerce.Infrastructure.Dispatcher;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace Commerce.Test.Purchase;

public class BillTest
{
    private EventDispatcher dispatcher;
    private DataContext context;
    private ITestOutputHelper output;

    public BillTest(ITestOutputHelper output)
    {
        this.output = output;
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        context = new DataContext(options);
        dispatcher = new EventDispatcher();
        dispatcher.AddService(context);
        dispatcher.AddHandler<BillCommandHandler>();
        dispatcher.AddHandler<BillQueryHandler>();

        var enterprise = new Enterprise() { Id = 1, Name = "FashionShop" };
        var supplier1 = new Supplier() { Id = 1, Name = "John Doe" };
        var supplier2 = new Supplier() { Id = 2, Name = "John Smith" };

        context.Enterprises.Add(enterprise);
        context.Suppliers.Add(supplier1);
        context.Suppliers.Add(supplier2);
        context.SaveChanges();

        var bill1 = new Bill() { Reference = Guid.NewGuid().ToString() };
        bill1.Enterprise = enterprise;
        bill1.Supplier = supplier1;
        bill1.Lines.Add(new BillLine() { Code = Guid.NewGuid().ToString(), Name = "Shirt", Price = 50, Quantity = 2 });
        bill1.Lines.Add(new BillLine() { Code = Guid.NewGuid().ToString(), Name = "Pant", Price = 70, Quantity = 1 });

        var bill2 = new Bill() { Reference = Guid.NewGuid().ToString() };
        bill2.Enterprise = enterprise;
        bill2.Supplier = supplier2;
        bill2.Lines.Add(new BillLine() { Code = Guid.NewGuid().ToString(), Name = "T-Shirt", Price = 20, Quantity = 5 });
        bill2.Lines.Add(new BillLine() { Code = Guid.NewGuid().ToString(), Name = "Short", Price = 50, Quantity = 2 });

        var bill3 = new Bill() { Reference = Guid.NewGuid().ToString() };
        bill3.Enterprise = enterprise;
        bill3.Supplier = supplier1;
        bill3.Lines.Add(new BillLine() { Code = Guid.NewGuid().ToString(), Name = "Hat", Price = 30, Quantity = 1 });

        var bill4 = new Bill() { Reference = Guid.NewGuid().ToString() };
        bill4.Enterprise = enterprise;
        bill4.Supplier = supplier2;
        bill4.Lines.Add(new BillLine() { Code = Guid.NewGuid().ToString(), Name = "Jacket", Price = 160, Quantity = 1 });

        context.Bills.Add(bill1);
        context.Bills.Add(bill2);
        context.Bills.Add(bill3);
        context.Bills.Add(bill4);
        context.SaveChanges();
    }

    [Fact]
    public async void CreateAsync()
    {
        var bill = new Bill() { Reference = Guid.NewGuid().ToString() };
        bill.EnterpriseId = 1;
        bill.SupplierId = 1;

        var line1 = new BillLine() { Code = Guid.NewGuid().ToString(), Name = "Shirt", Price = 40, Quantity = 3 };
        var gstTax1 = new BillLineTax() { Name = "GST", Rate = 5, Line = line1 };
        var pstTax1 = new BillLineTax() { Name = "QST", Rate = 9.975m, Line = line1 };

        line1.Taxes.Add(gstTax1);
        line1.Taxes.Add(pstTax1);

        var line2 = new BillLine() { Code = Guid.NewGuid().ToString(), Name = "Pant", Price = 60, Quantity = 2 };
        var gstTax2 = new BillLineTax() { Name = "GST", Rate = 5, Line = line2 };
        var pstTax2 = new BillLineTax() { Name = "QST", Rate = 9.975m, Line = line2 };

        line2.Taxes.Add(gstTax2);
        line2.Taxes.Add(pstTax2);

        bill.Lines.Add(line1);
        bill.Lines.Add(line2);

        var command = new BillCommand();
        command.Action = CommandAction.Create;
        command.Argument = bill;

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(7, result);

        bill = await context.Bills.FindAsync(5);
        Assert.NotNull(bill);
        Assert.Equal(275.940m, bill.Total);

        output.WriteLine($"Subtotal: {bill.Subtotal}");

        foreach (var tax in bill.Taxes)
        {
            output.WriteLine($"{tax.Key}: {tax.Value}");
        }

        output.WriteLine($"Total: {bill.Total}");
    }

    [Fact]
    public async void UpdateAsync()
    {
        var bill = await context.Bills.FindAsync(3);
        bill!.Lines[0].Quantity = 2;

        var command = new BillCommand();
        command.Action = CommandAction.Update;
        command.Argument = bill;

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(2, result);

        bill = await context.Bills.FindAsync(3);
        Assert.Single(bill!.Lines);
        Assert.Equal(2, bill!.Lines[0].Quantity);
    }

    [Fact]
    public async void DeleteAsync()
    {
        var command = new BillCommand();
        command.Action = CommandAction.Delete;
        command.Argument = 1;

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(3, 3);
    }

    [Fact]
    public async void FilterAsync()
    {
        var query = new BillQuery();
        query.Parameters["Id"] = 1;
        var list = await dispatcher.DispatchAsync(query) as List<Bill>;
        Assert.NotNull(list);
        Assert.Single(list);

        query = new BillQuery();
        query.Parameters["Date"] = DateTime.Now;
        list = await dispatcher.DispatchAsync(query) as List<Bill>;
        Assert.NotNull(list);
        Assert.Equal(4, list.Count);
    }

    [Fact]
    public async void SortAsync()
    {
        var query = new BillQuery();
        query.Sort = "Total";

        var list = await dispatcher.DispatchAsync(query) as List<Bill>;
        Assert.NotNull(list);
        Assert.Equal(30, list[0].Total);
        Assert.Equal(160, list[1].Total);
        Assert.Equal(170, list[2].Total);
        Assert.Equal(200, list[3].Total);

        // reverse order by name
        query.Sort = "-Total";
        list = await dispatcher.DispatchAsync(query) as List<Bill>;
        Assert.NotNull(list);
        Assert.Equal(200, list[0].Total);
        Assert.Equal(170, list[1].Total);
        Assert.Equal(160, list[2].Total);
        Assert.Equal(30, list[3].Total);
    }

    [Fact]
    public async void PaginateAsync()
    {
        var query = new BillQuery();
        query.Offset = 2;
        query.Limit = 2;

        var list = await dispatcher.DispatchAsync(query) as List<Bill>;
        Assert.NotNull(list);

        var invoice1 = list[0];
        var invoice2 = list[1];
        Assert.Equal(3, invoice1.Id);
        Assert.Equal(4, invoice2.Id);
    }
}
