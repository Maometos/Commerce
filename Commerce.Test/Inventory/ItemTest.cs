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
        dispatcher.AddHandler<ItemQueryHandler>();

        var item1 = new Item() { Code = Guid.NewGuid().ToString(), Name = "Shirt", Price = 50 };
        var item2 = new Item() { Code = Guid.NewGuid().ToString(), Name = "T-Shirt", Price = 20 };
        var item3 = new Item() { Code = Guid.NewGuid().ToString(), Name = "Pant", Price = 70 };
        var item4 = new Item() { Code = Guid.NewGuid().ToString(), Name = "Short", Price = 50 };

        context.Items.Add(item1);
        context.Items.Add(item2);
        context.Items.Add(item3);
        context.Items.Add(item4);
        context.SaveChanges();
    }

    [Fact]
    public async void CreateAsync()
    {
        var command = new ItemCommand();
        command.Action = CommandAction.Create;
        command.Argument = new Item() { Code = Guid.NewGuid().ToString(), Name = "Hat", Price = 30 }; ;

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(1, result);
    }

    [Fact]
    public async void UpdateAsync()
    {
        var command = new ItemCommand();
        command.Action = CommandAction.Update;
        command.Argument = new Item() { Id = 4, Code = Guid.NewGuid().ToString(), Name = "Jacket", Price = 160 };

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(1, result);

        var item = await context.Items.FindAsync(4);
        Assert.Equal("Jacket", item!.Name);
        Assert.Equal(160, item!.Price);
    }

    [Fact]
    public async void DeleteAsync()
    {
        var command = new ItemCommand();
        command.Action = CommandAction.Delete;
        command.Argument = 1;

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(1, result);
    }

    [Fact]
    public async void FilterAsync()
    {
        var query = new ItemQuery();
        query.Parameters["Id"] = 1;

        var list = await dispatcher.DispatchAsync(query) as List<Item>;
        Assert.NotNull(list);
        Assert.Single(list);

        query = new ItemQuery();
        query.Parameters["Price"] = 50m;

        list = await dispatcher.DispatchAsync(query) as List<Item>;
        Assert.NotNull(list);
        Assert.Equal(2, list.Count);
    }

    [Fact]
    public async void SortAsync()
    {
        var query = new ItemQuery();
        query.Sort = "Name";

        var list = await dispatcher.DispatchAsync(query) as List<Item>;
        Assert.NotNull(list);

        var item1 = list[0];
        var item2 = list[1];
        var item3 = list[2];
        var item4 = list[3];

        Assert.Equal("Pant", item1.Name);
        Assert.Equal("Shirt", item2.Name);
        Assert.Equal("Short", item3.Name);
        Assert.Equal("T-Shirt", item4.Name);

        // reverse order by name
        query.Sort = "-Name";
        list = await dispatcher.DispatchAsync(query) as List<Item>;
        Assert.NotNull(list);

        item1 = list[0];
        item2 = list[1];
        item3 = list[2];
        item4 = list[3];

        Assert.Equal("T-Shirt", item1.Name);
        Assert.Equal("Short", item2.Name);
        Assert.Equal("Shirt", item3.Name);
        Assert.Equal("Pant", item4.Name);
    }

    [Fact]
    public async void PaginateAsync()
    {
        var query = new ItemQuery();
        query.Offset = 2;
        query.Limit = 2;

        var list = await dispatcher.DispatchAsync(query) as List<Item>;
        Assert.NotNull(list);

        var item1 = list[0];
        var item2 = list[1];
        Assert.Equal("Pant", item1.Name);
        Assert.Equal("Short", item2.Name);
    }
}
