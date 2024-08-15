using Commerce.Core.Common;
using Commerce.Core.Identity.Handlers;
using Commerce.Core.Identity.Requests;
using Commerce.Core.Inventory.Entities;
using Commerce.Infrastructure.CQRS;
using Commerce.Infrastructure.Dispatcher;
using Microsoft.EntityFrameworkCore;

namespace Commerce.Test.Inventory;

public class BrandTest
{
    private EventDispatcher dispatcher;
    private DataContext context;

    public BrandTest()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        context = new DataContext(options);
        dispatcher = new EventDispatcher();
        dispatcher.AddService(context);
        dispatcher.AddHandler<BrandCommandHandler>();

        var brand1 = new Brand() { Id = 1, Name = "Dior" };
        var brand2 = new Brand() { Id = 2, Name = "Zara" };
        var brand3 = new Brand() { Id = 3, Name = "Boss" };
        var brand4 = new Brand() { Id = 3, Name = "Nike" };

        context.Brands.Add(brand1);
        context.Brands.Add(brand2);
        context.Brands.Add(brand3);
        context.SaveChanges();
    }

    [Fact]
    public async void TestCreatingBrand()
    {
        var command = new BrandCommand();
        command.Action = CommandAction.Create;
        command.Name = "Gucci";

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(1, result);
    }

    [Fact]
    public async void TestUpdatingBrand()
    {
        var command = new BrandCommand();
        command.Action = CommandAction.Update;
        command.Id = 3;
        command.Name = "Prada";

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(1, result);
    }

    [Fact]
    public async void TestDeletingBrand()
    {
        var command = new BrandCommand();
        command.Action = CommandAction.Delete;
        command.Id = 1;

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(1, result);
    }
}
