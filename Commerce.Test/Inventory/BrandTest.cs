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
        dispatcher.AddHandler<BrandQueryHandler>();

        var brand1 = new Brand() { Name = "Dior" };
        var brand2 = new Brand() { Name = "Zara" };
        var brand3 = new Brand() { Name = "Boss" };
        var brand4 = new Brand() { Name = "Nike" };

        context.Brands.Add(brand1);
        context.Brands.Add(brand2);
        context.Brands.Add(brand3);
        context.Brands.Add(brand4);
        context.SaveChanges();
    }

    [Fact]
    public async void CreateAsync()
    {
        var command = new BrandCommand();
        command.Action = CommandAction.Create;
        command.Argument = new Brand() { Name = "Gucci" };

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(1, result);
    }

    [Fact]
    public async void UpdateAsync()
    {
        var command = new BrandCommand();
        command.Action = CommandAction.Update;
        command.Argument = new Brand() { Id = 3, Name = "Prada" };

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(1, result);

        var brand = await context.Brands.FindAsync(3);
        Assert.Equal("Prada", brand!.Name);
    }

    [Fact]
    public async void DeleteAsync()
    {
        var command = new BrandCommand();
        command.Action = CommandAction.Delete;
        command.Argument = 1;

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(1, result);
    }

    [Fact]
    public async void FilterAsync()
    {
        var query = new BrandQuery();
        query.Parameters["Id"] = 1;

        var list = await dispatcher.DispatchAsync(query) as List<Brand>;
        Assert.NotNull(list);
        Assert.Single(list);

        query = new BrandQuery();
        query.Parameters["Name"] = "Boss";

        list = await dispatcher.DispatchAsync(query) as List<Brand>;
        Assert.NotNull(list);
        Assert.Single(list);
    }

    [Fact]
    public async void SortAsync()
    {
        var query = new BrandQuery();
        query.Sort = "Name";

        var list = await dispatcher.DispatchAsync(query) as List<Brand>;
        Assert.NotNull(list);

        var brand1 = list[0];
        var brand2 = list[1];
        var brand3 = list[2];
        var brand4 = list[3];

        Assert.Equal("Boss", brand1.Name);
        Assert.Equal("Dior", brand2.Name);
        Assert.Equal("Nike", brand3.Name);
        Assert.Equal("Zara", brand4.Name);

        // reverse order by name
        query.Sort = "-Name";
        list = await dispatcher.DispatchAsync(query) as List<Brand>;
        Assert.NotNull(list);

        brand1 = list[0];
        brand2 = list[1];
        brand3 = list[2];
        brand4 = list[3];

        Assert.Equal("Zara", brand1.Name);
        Assert.Equal("Nike", brand2.Name);
        Assert.Equal("Dior", brand3.Name);
        Assert.Equal("Boss", brand4.Name);
    }

    [Fact]
    public async void PaginateAsync()
    {
        var query = new BrandQuery();
        query.Offset = 2;
        query.Limit = 2;

        var list = await dispatcher.DispatchAsync(query) as List<Brand>;
        Assert.NotNull(list);

        var brand1 = list[0];
        var brand2 = list[1];
        Assert.Equal("Boss", brand1.Name);
        Assert.Equal("Nike", brand2.Name);
    }
}
