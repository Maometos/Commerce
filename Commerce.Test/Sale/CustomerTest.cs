using Commerce.Core.Common;
using Commerce.Core.Sale.Entities;
using Commerce.Core.Sale.Handlers;
using Commerce.Core.Sale.Requests;
using Commerce.Infrastructure.CQRS;
using Commerce.Infrastructure.Dispatcher;
using Microsoft.EntityFrameworkCore;

namespace Commerce.Test.Sale;

public class CustomerTest
{
    private EventDispatcher dispatcher;
    private DataContext context;

    public CustomerTest()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        context = new DataContext(options);
        dispatcher = new EventDispatcher();
        dispatcher.AddService(context);
        dispatcher.AddHandler<CustomerCommandHandler>();
        dispatcher.AddHandler<CustomerQueryHandler>();

        var customer1 = new Customer() { Id = 1, Name = "John Doe", Email = "john.doe@email.com", Country = "Canada" };
        var customer2 = new Customer() { Id = 2, Name = "Jane Doe", Email = "Jane.doe@email.com", Country = "Canada" };
        var customer3 = new Customer() { Id = 3, Name = "John Smith", Email = "Jane.Smith@email.com", Country = "USA" };
        var customer4 = new Customer() { Id = 4, Name = "Jane Smith", Email = "Jane.Smith@email.com", Country = "USA" };

        context.Customers.Add(customer1);
        context.Customers.Add(customer2);
        context.Customers.Add(customer3);
        context.Customers.Add(customer4);
        context.SaveChanges();
    }

    [Fact]
    public async void TestCreating()
    {
        var command = new CustomerCommand();
        command.Action = CommandAction.Create;
        command.Name = "Joe Blow";
        command.Email = "joe.blow@email.com";

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(1, result);
    }

    [Fact]
    public async void TestUpdating()
    {
        var command = new CustomerCommand();
        command.Action = CommandAction.Update;
        command.Id = 4;
        command.Address = "Fake street";

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(1, result);

        var customer = await context.Customers.FindAsync(4);
        Assert.Equal("Fake street", customer!.Address);
    }

    [Fact]
    public async void TestDeleting()
    {
        var command = new CustomerCommand();
        command.Action = CommandAction.Delete;
        command.Id = 1;

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(1, result);
    }

    [Fact]
    public async void TestFinding()
    {
        var query = new CustomerQuery();
        query.Action = QueryAction.Find;
        query.Id = 1;

        var customer = await dispatcher.DispatchAsync(query) as Customer;
        Assert.NotNull(customer);
        Assert.Equal("John Doe", customer.Name);
    }

    [Fact]
    public async void TestFiltering()
    {
        var query = new CustomerQuery();
        query.Action = QueryAction.List;
        query.Country = "Canada";

        var list = await dispatcher.DispatchAsync(query) as List<Customer>;
        Assert.NotNull(list);
        Assert.Equal(2, list.Count);
    }

    [Fact]
    public async void TestSorting()
    {
        var query = new CustomerQuery();
        query.Action = QueryAction.List;
        query.Sort = "Name";

        var list = await dispatcher.DispatchAsync(query) as List<Customer>;
        Assert.NotNull(list);

        var customer1 = list[0];
        var customer2 = list[1];
        var customer3 = list[2];
        var customer4 = list[3];

        Assert.Equal("Jane Doe", customer1.Name);
        Assert.Equal("Jane Smith", customer2.Name);
        Assert.Equal("John Doe", customer3.Name);
        Assert.Equal("John Smith", customer4.Name);
    }

    [Fact]
    public async void TestReverseSorting()
    {
        var query = new CustomerQuery();
        query.Action = QueryAction.List;
        query.Sort = "Name";
        query.Reverse = true;

        var list = await dispatcher.DispatchAsync(query) as List<Customer>;
        Assert.NotNull(list);

        var customer1 = list[0];
        var customer2 = list[1];
        var customer3 = list[2];
        var customer4 = list[3];

        Assert.Equal("John Smith", customer1.Name);
        Assert.Equal("John Doe", customer2.Name);
        Assert.Equal("Jane Smith", customer3.Name);
        Assert.Equal("Jane Doe", customer4.Name);
    }

    [Fact]
    public async void TestPaginating()
    {
        var query = new CustomerQuery();
        query.Action = QueryAction.List;
        query.Page = 2;
        query.Limit = 2;

        var list = await dispatcher.DispatchAsync(query) as List<Customer>;
        Assert.NotNull(list);

        var customer1 = list[0];
        var customer2 = list[1];
        Assert.Equal("John Smith", customer1.Name);
        Assert.Equal("Jane Smith", customer2.Name);
    }
}
