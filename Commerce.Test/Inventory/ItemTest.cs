using Commerce.Core.Common;
using Commerce.Core.Identity.Handlers;
using Commerce.Core.Identity.Requests;
using Commerce.Core.Inventory.Entities;
using Commerce.Infrastructure.CQRS;
using Commerce.Infrastructure.Dispatcher;
using Microsoft.EntityFrameworkCore;

namespace Commerce.Test.Inventory;

public class ItemTest
{
    private EventDispatcher dispatcher;
    private DataContext context;

    public ItemTest()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        context = new DataContext(options);
        dispatcher = new EventDispatcher();
        dispatcher.AddService(context);
        dispatcher.AddHandler<ItemCommandHandler>();

        var item1 = new Item() { Id = 1, Code = Guid.NewGuid().ToString(), Name = "Shirt", Price = 40 };
        var item2 = new Item() { Id = 2, Code = Guid.NewGuid().ToString(), Name = "T-Shirt", Price = 20 };
        var item3 = new Item() { Id = 3, Code = Guid.NewGuid().ToString(), Name = "Pant", Price = 70 };
        var item4 = new Item() { Id = 4, Code = Guid.NewGuid().ToString(), Name = "Short", Price = 50 };

        context.Items.Add(item1);
        context.Items.Add(item2);
        context.Items.Add(item3);
        context.Items.Add(item4);
        context.SaveChanges();
    }

    [Fact]
    public async void TestCreatingItem()
    {
        var command = new ItemCommand();
        command.Action = CommandAction.Create;
        command.Code = Guid.NewGuid().ToString();
        command.Name = "Hat";
        command.Price = 30;

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(1, result);
    }

    [Fact]
    public async void TestUpdatingItem()
    {
        var command = new ItemCommand();
        command.Action = CommandAction.Update;
        command.Id = 4;
        command.Name = "Jacket";
        command.Price = 160;

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(1, result);
    }

    [Fact]
    public async void TestDeletingItem()
    {
        var command = new ItemCommand();
        command.Action = CommandAction.Delete;
        command.Id = 1;

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(1, result);
    }
}
