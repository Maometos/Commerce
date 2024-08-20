using Commerce.Core.Common;
using Commerce.Core.Purchase.Entities;
using Commerce.Core.Purchase.Handlers;
using Commerce.Core.Purchase.Requests;
using Commerce.Core.Sale.Requests;
using Commerce.Infrastructure.CQRS;
using Commerce.Infrastructure.Dispatcher;
using Microsoft.EntityFrameworkCore;

namespace Commerce.Test.Purchase;

public class VendorTest
{
    private EventDispatcher dispatcher;
    private DataContext context;

    public VendorTest()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        context = new DataContext(options);
        dispatcher = new EventDispatcher();
        dispatcher.AddService(context);
        dispatcher.AddHandler<VendorCommandHandler>();
        dispatcher.AddHandler<VendorQueryHandler>();

        var vendor1 = new Vendor() { Id = 1, Name = "John Doe", Email = "john.doe@email.com", Country = "Canada" };
        var vendor2 = new Vendor() { Id = 2, Name = "Jane Doe", Email = "Jane.doe@email.com", Country = "Canada" };
        var vendor3 = new Vendor() { Id = 3, Name = "John Smith", Email = "Jane.Smith@email.com", Country = "USA" };
        var vendor4 = new Vendor() { Id = 4, Name = "Jane Smith", Email = "Jane.Smith@email.com", Country = "USA" };

        context.Vendors.Add(vendor1);
        context.Vendors.Add(vendor2);
        context.Vendors.Add(vendor3);
        context.Vendors.Add(vendor4);
        context.SaveChanges();
    }

    [Fact]
    public async void TestCreating()
    {
        var command = new VendorCommand();
        command.Action = CommandAction.Create;
        command.Name = "Joe Blow";
        command.Email = "joe.blow@email.com";

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(1, result);
    }

    [Fact]
    public async void TestUpdating()
    {
        var command = new VendorCommand();
        command.Action = CommandAction.Update;
        command.Id = 4;
        command.Address = "Fake street";

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(1, result);

        var vendor = await context.Vendors.FindAsync(4);
        Assert.Equal("Fake street", vendor!.Address);
    }

    [Fact]
    public async void TestDeleting()
    {
        var command = new VendorCommand();
        command.Action = CommandAction.Delete;
        command.Id = 1;

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(1, result);
    }

    [Fact]
    public async void TestFinding()
    {
        var query = new VendorQuery();
        query.Action = QueryAction.Find;
        query.Id = 1;

        var vendor = await dispatcher.DispatchAsync(query) as Vendor;
        Assert.NotNull(vendor);
        Assert.Equal("John Doe", vendor.Name);
    }

    [Fact]
    public async void TestFiltering()
    {
        var query = new VendorQuery();
        query.Action = QueryAction.List;
        query.Country = "Canada";

        var list = await dispatcher.DispatchAsync(query) as List<Vendor>;
        Assert.NotNull(list);
        Assert.Equal(2, list.Count);
    }

    [Fact]
    public async void TestSorting()
    {
        var query = new VendorQuery();
        query.Action = QueryAction.List;
        query.Sort = "Name";

        var list = await dispatcher.DispatchAsync(query) as List<Vendor>;
        Assert.NotNull(list);

        var vendor1 = list[0];
        var vendor2 = list[1];
        var vendor3 = list[2];
        var vendor4 = list[3];

        Assert.Equal("Jane Doe", vendor1.Name);
        Assert.Equal("Jane Smith", vendor2.Name);
        Assert.Equal("John Doe", vendor3.Name);
        Assert.Equal("John Smith", vendor4.Name);
    }

    [Fact]
    public async void TestReverseSorting()
    {
        var query = new VendorQuery();
        query.Action = QueryAction.List;
        query.Sort = "Name";
        query.Reverse = true;

        var list = await dispatcher.DispatchAsync(query) as List<Vendor>;
        Assert.NotNull(list);

        var vendor1 = list[0];
        var vendor2 = list[1];
        var vendor3 = list[2];
        var vendor4 = list[3];

        Assert.Equal("John Smith", vendor1.Name);
        Assert.Equal("John Doe", vendor2.Name);
        Assert.Equal("Jane Smith", vendor3.Name);
        Assert.Equal("Jane Doe", vendor4.Name);
    }

    [Fact]
    public async void TestPaginating()
    {
        var query = new VendorQuery();
        query.Action = QueryAction.List;
        query.Page = 2;
        query.Limit = 2;

        var list = await dispatcher.DispatchAsync(query) as List<Vendor>;
        Assert.NotNull(list);

        var vendor1 = list[0];
        var vendor2 = list[1];
        Assert.Equal("John Smith", vendor1.Name);
        Assert.Equal("Jane Smith", vendor2.Name);
    }
}
