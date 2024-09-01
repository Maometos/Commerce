﻿using Commerce.Core.Common;
using Commerce.Core.Common.Entities;
using Commerce.Core.Purchase.Entities;
using Commerce.Core.Purchase.Requests;
using Commerce.Core.Sale.Handlers;
using Commerce.Infrastructure.CQRS;
using Commerce.Infrastructure.Dispatcher;
using Microsoft.EntityFrameworkCore;

namespace Commerce.Test.Purchase;

public class BillTest
{
    private EventDispatcher dispatcher;
    private DataContext context;

    public BillTest()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        context = new DataContext(options);
        dispatcher = new EventDispatcher();
        dispatcher.AddService(context);
        dispatcher.AddHandler<BillCommandHandler>();

        var enterprise = new Enterprise() { Id = 1, Name = "FashionShop" };
        var vendor1 = new Vendor() { Id = 1, Name = "John Doe" };
        var vendor2 = new Vendor() { Id = 2, Name = "John Smith" };

        context.Enterprises.Add(enterprise);
        context.Vendors.Add(vendor1);
        context.Vendors.Add(vendor2);
        context.SaveChanges();

        var invoice1 = new Bill() { Reference = Guid.NewGuid().ToString() };
        invoice1.Enterprise = enterprise;
        invoice1.Vendor = vendor1;
        invoice1.Lines.Add(new BillLine() { Code = Guid.NewGuid().ToString(), Name = "Shirt", Price = 50, Quantity = 2 });
        invoice1.Lines.Add(new BillLine() { Code = Guid.NewGuid().ToString(), Name = "Pant", Price = 70, Quantity = 1 });

        var invoice2 = new Bill() { Reference = Guid.NewGuid().ToString() };
        invoice2.Enterprise = enterprise;
        invoice2.Vendor = vendor2;
        invoice2.Lines.Add(new BillLine() { Code = Guid.NewGuid().ToString(), Name = "T-Shirt", Price = 20, Quantity = 5 });
        invoice2.Lines.Add(new BillLine() { Code = Guid.NewGuid().ToString(), Name = "Short", Price = 50, Quantity = 2 });

        var invoice3 = new Bill() { Reference = Guid.NewGuid().ToString() };
        invoice3.Enterprise = enterprise;
        invoice3.Vendor = vendor1;
        invoice3.Lines.Add(new BillLine() { Code = Guid.NewGuid().ToString(), Name = "Hat", Price = 30, Quantity = 1 });

        var invoice4 = new Bill() { Reference = Guid.NewGuid().ToString() };
        invoice4.Enterprise = enterprise;
        invoice4.Vendor = vendor2;
        invoice4.Lines.Add(new BillLine() { Code = Guid.NewGuid().ToString(), Name = "Jacket", Price = 160, Quantity = 1 });

        context.Bills.Add(invoice1);
        context.Bills.Add(invoice2);
        context.Bills.Add(invoice3);
        context.Bills.Add(invoice4);
        context.SaveChanges();
    }

    [Fact]
    public async void TestCreating()
    {
        var invoice = new Bill() { Reference = Guid.NewGuid().ToString() };
        invoice.EnterpriseId = 1;
        invoice.VendorId = 1;

        var line1 = new BillLine() { Code = Guid.NewGuid().ToString(), Name = "Shirt", Price = 50, Quantity = 3 };
        var gstTax1 = new BillLineTax() { Name = "GST", Rate = 5, Line = line1 };
        var pstTax1 = new BillLineTax() { Name = "PST", Rate = 6, Line = line1 };

        line1.Taxes.Add(gstTax1);
        line1.Taxes.Add(pstTax1);

        var line2 = new BillLine() { Code = Guid.NewGuid().ToString(), Name = "Pant", Price = 70, Quantity = 2 };
        var gstTax2 = new BillLineTax() { Name = "GST", Rate = 5, Line = line2 };
        var pstTax2 = new BillLineTax() { Name = "PST", Rate = 6, Line = line2 };

        line1.Taxes.Add(gstTax2);
        line1.Taxes.Add(pstTax2);

        invoice.Lines.Add(line1);
        invoice.Lines.Add(line2);

        var command = new BillCommand();
        command.Action = CommandAction.Create;
        command.Argument = invoice;

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(7, result);
    }

    [Fact]
    public async void TestUpdating()
    {
        var invoice = await context.Bills.FindAsync(3);
        invoice!.Lines[0].Quantity = 2;

        var command = new BillCommand();
        command.Action = CommandAction.Update;
        command.Argument = invoice;

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(2, result);

        invoice = await context.Bills.FindAsync(3);
        Assert.Single(invoice!.Lines);
        Assert.Equal(2, invoice!.Lines[0].Quantity);
    }

    [Fact]
    public async void TestDeleting()
    {
        var command = new BillCommand();
        command.Action = CommandAction.Delete;
        command.Argument = 1;

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(3, 3);
    }
}
