using Commerce.Core.Common;
using Commerce.Core.Identity.Handlers;
using Commerce.Core.Identity.Requests;
using Commerce.Core.Inventory.Entities;
using Commerce.Infrastructure.CQRS;
using Commerce.Infrastructure.Dispatcher;
using Microsoft.EntityFrameworkCore;

namespace Commerce.Test.Inventory;

public class CategoryTest
{
    private EventDispatcher dispatcher;
    private DataContext context;

    public CategoryTest()
    {
        var options = new DbContextOptionsBuilder<DataContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        context = new DataContext(options);
        dispatcher = new EventDispatcher();
        dispatcher.AddService(context);
        dispatcher.AddHandler<CategoryCommandHandler>();

        var category1 = new Category() { Id = 1, Name = "Shoes" };
        var category2 = new Category() { Id = 2, Name = "Clothes" };

        context.Categories.Add(category1);
        context.Categories.Add(category2);
        context.SaveChanges();
    }

    [Fact]
    public async void TestCreatingCategory()
    {
        var command = new CategoryCommand();
        command.Action = CommandAction.Create;
        command.Name = "Accessories";

        var result = await dispatcher.DispatchAsync(command);

        Assert.Equal(1, result);
    }

    [Fact]
    public async void TestUpdatingCategory()
    {
        var command = new CategoryCommand();
        command.Action = CommandAction.Update;
        command.Id = 1;
        command.Name = "Underwear";

        var result = await dispatcher.DispatchAsync(command);

        Assert.Equal(1, result);
    }

    [Fact]
    public async void TestDeletingCategory()
    {
        var command = new CategoryCommand();
        command.Action = CommandAction.Delete;
        command.Id = 1;

        var result = await dispatcher.DispatchAsync(command);

        Assert.Equal(1, result);
    }
}
