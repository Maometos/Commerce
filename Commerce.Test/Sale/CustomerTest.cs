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

        var customer1 = new Customer() { Name = "John Doe", Email = "john.doe@email.com", Country = "Canada" };
        var customer2 = new Customer() { Name = "Jane Doe", Email = "Jane.doe@email.com", Country = "Canada" };
        var customer3 = new Customer() { Name = "John Smith", Email = "Jane.Smith@email.com", Country = "USA" };
        var customer4 = new Customer() { Name = "Jane Smith", Email = "Jane.Smith@email.com", Country = "USA" };

        context.Customers.Add(customer1);
        context.Customers.Add(customer2);
        context.Customers.Add(customer3);
        context.Customers.Add(customer4);
        context.SaveChanges();
    }

    [Fact]
    public async void CreateAsync()
    {
        var command = new CustomerCommand();
        command.Action = CommandAction.Create;
        command.Argument = new Customer() { Name = "Joe Blow", Email = "joe.blow@email.com" };

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(1, result);
    }

    [Fact]
    public async void UpdateAsync()
    {
        var command = new CustomerCommand();
        command.Action = CommandAction.Update;
        command.Argument = new Customer() { Id = 4, Name = "Jane Smith", Email = "Jane.Smith@email.com", Country = "UK" };

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(1, result);

        var customer = await context.Customers.FindAsync(4);
        Assert.Equal("UK", customer!.Country);
    }

    [Fact]
    public async void DeleteAsync()
    {
        var command = new CustomerCommand();
        command.Action = CommandAction.Delete;
        command.Argument = 1;

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(1, result);
    }

    [Fact]
    public async void FilterAsync()
    {
        var query = new CustomerQuery();
        query.Parameters["Id"] = 1;

        var list = await dispatcher.DispatchAsync(query) as List<Customer>;
        Assert.NotNull(list);
        Assert.Single(list);

        query = new CustomerQuery();
        query.Parameters["Country"] = "Canada";

        list = await dispatcher.DispatchAsync(query) as List<Customer>;
        Assert.NotNull(list);
        Assert.Equal(2, list.Count);
    }

    [Fact]
    public async void SortAsync()
    {
        var query = new CustomerQuery();
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

        // reverse order by name
        query.Sort = "-Name";
        list = await dispatcher.DispatchAsync(query) as List<Customer>;
        Assert.NotNull(list);

        customer1 = list[0];
        customer2 = list[1];
        customer3 = list[2];
        customer4 = list[3];

        Assert.Equal("John Smith", customer1.Name);
        Assert.Equal("John Doe", customer2.Name);
        Assert.Equal("Jane Smith", customer3.Name);
        Assert.Equal("Jane Doe", customer4.Name);
    }

    [Fact]
    public async void PaginateAsync()
    {
        var query = new CustomerQuery();
        query.Offset = 2;
        query.Limit = 2;

        var list = await dispatcher.DispatchAsync(query) as List<Customer>;
        Assert.NotNull(list);

        var customer1 = list[0];
        var customer2 = list[1];
        Assert.Equal("John Smith", customer1.Name);
        Assert.Equal("Jane Smith", customer2.Name);
    }
}
