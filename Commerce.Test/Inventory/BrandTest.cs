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

        var brand1 = new Brand() { Id = 1, Name = "Dior" };
        var brand2 = new Brand() { Id = 2, Name = "Zara" };
        var brand3 = new Brand() { Id = 3, Name = "Boss" };
        var brand4 = new Brand() { Id = 4, Name = "Nike" };

        context.Brands.Add(brand1);
        context.Brands.Add(brand2);
        context.Brands.Add(brand3);
        context.Brands.Add(brand4);
        context.SaveChanges();
    }

    [Fact]
    public async void TestCreating()
    {
        var command = new BrandCommand();
        command.Action = CommandAction.Create;
        command.Name = "Gucci";

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(1, result);
    }

    [Fact]
    public async void TestUpdating()
    {
        var command = new BrandCommand();
        command.Action = CommandAction.Update;
        command.Id = 3;
        command.Name = "Prada";

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(1, result);
    }

    [Fact]
    public async void TestDeleting()
    {
        var command = new BrandCommand();
        command.Action = CommandAction.Delete;
        command.Id = 1;

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(1, result);
    }

    [Fact]
    public async void TestFinding()
    {
        var query = new BrandQuery();
        query.Action = QueryAction.Find;
        query.Id = 1;

        var brand = await dispatcher.DispatchAsync(query) as Brand;
        Assert.NotNull(brand);
        Assert.Equal("Dior", brand.Name);
    }

    [Fact]
    public async void TestFiltering()
    {
        var query = new BrandQuery();
        query.Action = QueryAction.List;
        query.Name = "Boss";

        var list = await dispatcher.DispatchAsync(query) as List<Brand>;
        Assert.NotNull(list);
        Assert.Single(list);
    }

    [Fact]
    public async void TestSorting()
    {
        var query = new BrandQuery();
        query.Action = QueryAction.List;
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
    }

    [Fact]
    public async void TestReverseSorting()
    {
        var query = new BrandQuery();
        query.Action = QueryAction.List;
        query.Sort = "Name";
        query.Reverse = true;

        var list = await dispatcher.DispatchAsync(query) as List<Brand>;
        Assert.NotNull(list);

        var brand1 = list[0];
        var brand2 = list[1];
        var brand3 = list[2];
        var brand4 = list[3];

        Assert.Equal("Zara", brand1.Name);
        Assert.Equal("Nike", brand2.Name);
        Assert.Equal("Dior", brand3.Name);
        Assert.Equal("Boss", brand4.Name);
    }

    [Fact]
    public async void TestPaginating()
    {
        var query = new BrandQuery();
        query.Action = QueryAction.List;
        query.Page = 2;
        query.Limit = 2;

        var list = await dispatcher.DispatchAsync(query) as List<Brand>;
        Assert.NotNull(list);

        var brand1 = list[0];
        var brand2 = list[1];
        Assert.Equal("Boss", brand1.Name);
        Assert.Equal("Nike", brand2.Name);
    }
}
