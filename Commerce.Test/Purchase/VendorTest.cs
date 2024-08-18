using Commerce.Core.Common;
using Commerce.Core.Purchase.Entities;
using Commerce.Core.Purchase.Handlers;
using Commerce.Core.Purchase.Requests;
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

        var customer1 = new Vendor() { Id = 1, Name = "John Doe", Email = "john.doe@email.com" };
        var customer2 = new Vendor() { Id = 2, Name = "Jane Doe", Email = "Jane.doe@email.com" };
        var customer3 = new Vendor() { Id = 3, Name = "John Smith", Email = "Jane.Smith@email.com" };
        var customer4 = new Vendor() { Id = 4, Name = "Jane Smith", Email = "Jane.Smith@email.com" };

        context.Vendors.Add(customer1);
        context.Vendors.Add(customer2);
        context.Vendors.Add(customer3);
        context.Vendors.Add(customer4);
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

        var customer = await context.Vendors.FindAsync(4);
        Assert.Equal("Fake street", customer!.Address);
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
}
