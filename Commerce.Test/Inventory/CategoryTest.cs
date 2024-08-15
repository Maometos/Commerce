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
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        context = new DataContext(options);
        dispatcher = new EventDispatcher();
        dispatcher.AddService(context);
        dispatcher.AddHandler<CategoryCommandHandler>();
        dispatcher.AddHandler<CategoryQueryHandler>();

        var category1 = new Category() { Id = 1, Name = "Shoes" };
        var category2 = new Category() { Id = 2, Name = "Clothing" };
        var category3 = new Category() { Id = 3, Name = "Underwear" };

        context.Categories.Add(category1);
        context.Categories.Add(category2);
        context.Categories.Add(category3);
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
        command.Id = 3;
        command.Name = "Sport";

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

    [Fact]
    public async void TestFindingCategoryById()
    {
        var query = new CategoryQuery();
        query.Action = QueryAction.Find;
        query.Id = 1;

        var user = await dispatcher.DispatchAsync(query) as Category;
        Assert.NotNull(user);
        Assert.Equal("Shoes", user.Name);
    }

    [Fact]
    public async void TestFilteringCategoryByName()
    {
        var query = new CategoryQuery();
        query.Action = QueryAction.List;
        query.Name = "Clothing";

        var list = await dispatcher.DispatchAsync(query) as List<Category>;
        Assert.NotNull(list);
        Assert.Single(list);
    }

    [Fact]
    public async void TestSortingCategoriesByName()
    {
        var query = new CategoryQuery();
        query.Action = QueryAction.List;
        query.Sort = "Name";

        var list = await dispatcher.DispatchAsync(query) as List<Category>;
        Assert.NotNull(list);

        var category1 = list[0];
        var category2 = list[1];
        var category3 = list[2];

        Assert.Equal("Clothing", category1.Name);
        Assert.Equal("Shoes", category2.Name);
        Assert.Equal("Underwear", category3.Name);
    }

    [Fact]
    public async void TestSortingCategoriesByNameInReversOrder()
    {
        var query = new CategoryQuery();
        query.Action = QueryAction.List;
        query.Sort = "Name";
        query.Reverse = true;

        var list = await dispatcher.DispatchAsync(query) as List<Category>;
        Assert.NotNull(list);

        var category1 = list[0];
        var category2 = list[1];
        var category3 = list[2];

        Assert.Equal("Underwear", category1.Name);
        Assert.Equal("Shoes", category2.Name);
        Assert.Equal("Clothing", category3.Name);
    }

    [Fact]
    public async void TestPaginatingCategories()
    {
        var query = new CategoryQuery();
        query.Action = QueryAction.List;
        query.Page = 2;
        query.Limit = 2;

        var list = await dispatcher.DispatchAsync(query) as List<Category>;
        Assert.NotNull(list);

        var category = list[0];
        Assert.Equal("Underwear", category.Name);
    }
}
