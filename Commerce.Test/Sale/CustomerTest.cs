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

        var customer1 = new Customer() { Id = 1, Name = "John Doe", Email = "john.doe@email.com" };
        var customer2 = new Customer() { Id = 2, Name = "Jane Doe", Email = "Jane.doe@email.com" };
        var customer3 = new Customer() { Id = 3, Name = "John Smith", Email = "Jane.Smith@email.com" };
        var customer4 = new Customer() { Id = 4, Name = "Jane Smith", Email = "Jane.Smith@email.com" };

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
}
