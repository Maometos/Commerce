using Commerce.Core.Common;
using Commerce.Core.Purchase.Entities;
using Commerce.Core.Purchase.Handlers;
using Commerce.Core.Purchase.Requests;
using Commerce.Core.Sale.Requests;
using Commerce.Infrastructure.CQRS;
using Commerce.Infrastructure.Dispatcher;
using Microsoft.EntityFrameworkCore;

namespace Commerce.Test.Purchase;

public class SupplierTest
{
    private EventDispatcher dispatcher;
    private DataContext context;

    public SupplierTest()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        context = new DataContext(options);
        dispatcher = new EventDispatcher();
        dispatcher.AddService(context);
        dispatcher.AddHandler<SupplierCommandHandler>();
        dispatcher.AddHandler<SupplierQueryHandler>();

        var supplier1 = new Supplier() { Name = "John Doe", Email = "john.doe@email.com", Country = "Canada" };
        var supplier2 = new Supplier() { Name = "Jane Doe", Email = "Jane.doe@email.com", Country = "Canada" };
        var supplier3 = new Supplier() { Name = "John Smith", Email = "Jane.Smith@email.com", Country = "USA" };
        var supplier4 = new Supplier() { Name = "Jane Smith", Email = "Jane.Smith@email.com", Country = "USA" };

        context.Suppliers.Add(supplier1);
        context.Suppliers.Add(supplier2);
        context.Suppliers.Add(supplier3);
        context.Suppliers.Add(supplier4);
        context.SaveChanges();
    }

    [Fact]
    public async void TestCreating()
    {
        var command = new SupplierCommand();
        command.Action = CommandAction.Create;
        command.Argument = new Supplier() { Name = "Joe Blow", Email = "joe.blow@email.com" };

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(1, result);
    }

    [Fact]
    public async void TestUpdating()
    {
        var command = new SupplierCommand();
        command.Action = CommandAction.Update;
        command.Action = CommandAction.Update;
        command.Argument = new Supplier() { Id = 4, Name = "Jane Smith", Email = "Jane.Smith@email.com", Country = "UK" };

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(1, result);

        var supplier = await context.Suppliers.FindAsync(4);
        Assert.Equal("UK", supplier!.Country);
    }

    [Fact]
    public async void TestDeleting()
    {
        var command = new SupplierCommand();
        command.Action = CommandAction.Delete;
        command.Argument = 1;

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(1, result);
    }

    [Fact]
    public async void TestFinding()
    {
        var query = new SupplierQuery();
        query.Action = QueryAction.Find;
        query.Parameters["Id"] = 1;

        var supplier = await dispatcher.DispatchAsync(query) as Supplier;
        Assert.NotNull(supplier);
        Assert.Equal("John Doe", supplier.Name);
    }

    [Fact]
    public async void TestFiltering()
    {
        var query = new SupplierQuery();
        query.Action = QueryAction.List;
        query.Parameters["Country"] = "Canada";

        var list = await dispatcher.DispatchAsync(query) as List<Supplier>;
        Assert.NotNull(list);
        Assert.Equal(2, list.Count);
    }

    [Fact]
    public async void TestSorting()
    {
        var query = new SupplierQuery();
        query.Action = QueryAction.List;
        query.Sort = "Name";

        var list = await dispatcher.DispatchAsync(query) as List<Supplier>;
        Assert.NotNull(list);

        var supplier1 = list[0];
        var supplier2 = list[1];
        var supplier3 = list[2];
        var supplier4 = list[3];

        Assert.Equal("Jane Doe", supplier1.Name);
        Assert.Equal("Jane Smith", supplier2.Name);
        Assert.Equal("John Doe", supplier3.Name);
        Assert.Equal("John Smith", supplier4.Name);
    }

    [Fact]
    public async void TestReverseSorting()
    {
        var query = new SupplierQuery();
        query.Action = QueryAction.List;
        query.Sort = "-Name";

        var list = await dispatcher.DispatchAsync(query) as List<Supplier>;
        Assert.NotNull(list);

        var supplier1 = list[0];
        var supplier2 = list[1];
        var supplier3 = list[2];
        var supplier4 = list[3];

        Assert.Equal("John Smith", supplier1.Name);
        Assert.Equal("John Doe", supplier2.Name);
        Assert.Equal("Jane Smith", supplier3.Name);
        Assert.Equal("Jane Doe", supplier4.Name);
    }

    [Fact]
    public async void TestPaginating()
    {
        var query = new SupplierQuery();
        query.Action = QueryAction.List;
        query.Offset = 2;
        query.Limit = 2;

        var list = await dispatcher.DispatchAsync(query) as List<Supplier>;
        Assert.NotNull(list);

        var supplier1 = list[0];
        var supplier2 = list[1];
        Assert.Equal("John Smith", supplier1.Name);
        Assert.Equal("Jane Smith", supplier2.Name);
    }
}
