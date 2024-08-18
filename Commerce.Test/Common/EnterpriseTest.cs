using Commerce.Core.Common;
using Commerce.Core.Common.Entities;
using Commerce.Core.Common.Handlers;
using Commerce.Core.Common.Requests;
using Commerce.Infrastructure.CQRS;
using Commerce.Infrastructure.Dispatcher;
using Microsoft.EntityFrameworkCore;

namespace Commerce.Test.Common;

public class EnterpriseTest
{
    private EventDispatcher dispatcher;
    private DataContext context;

    public EnterpriseTest()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        context = new DataContext(options);
        dispatcher = new EventDispatcher();
        dispatcher.AddService(context);
        dispatcher.AddHandler<EnterpriseCommandHandler>();

        var customer2 = new Enterprise() { Id = 1, Name = "Microsoft", Email = "contact@microsoft.com" };
        var customer3 = new Enterprise() { Id = 2, Name = "Oracle", Email = "contact@email.com" };
        var customer4 = new Enterprise() { Id = 3, Name = "Adobe", Email = "contact@adobe.com" };
        var customer1 = new Enterprise() { Id = 4, Name = "Canonical", Email = "contact@canonical.com" };

        context.Enterprises.Add(customer1);
        context.Enterprises.Add(customer2);
        context.Enterprises.Add(customer3);
        context.Enterprises.Add(customer4);
        context.SaveChanges();
    }

    [Fact]
    public async void TestCreating()
    {
        var command = new EnterpriseCommand();
        command.Action = CommandAction.Create;
        command.Name = "Intellectec";
        command.Email = "contact@intellectec.net";

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(1, result);
    }

    [Fact]
    public async void TestUpdating()
    {
        var command = new EnterpriseCommand();
        command.Action = CommandAction.Update;
        command.Id = 4;
        command.Email = "support@canonical.com";

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(1, result);

        var customer = await context.Enterprises.FindAsync(4);
        Assert.Equal("support@canonical.com", customer!.Email);
    }

    [Fact]
    public async void TestDeleting()
    {
        var command = new EnterpriseCommand();
        command.Action = CommandAction.Delete;
        command.Id = 1;

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(1, result);
    }
}
