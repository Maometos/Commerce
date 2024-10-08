﻿using Commerce.Core.Common;
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

        var category1 = new Category() { Name = "Shoes" };
        var category2 = new Category() { Name = "Clothing" };
        var category3 = new Category() { Name = "Underwear" };
        var category4 = new Category() { Name = "Swimwear" };

        context.Categories.Add(category1);
        context.Categories.Add(category2);
        context.Categories.Add(category3);
        context.Categories.Add(category4);
        context.SaveChanges();
    }

    [Fact]
    public async void CreateAsync()
    {
        var command = new CategoryCommand();
        command.Action = CommandAction.Create;
        command.Argument = new Category() { Name = "Accessories" }; ;

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(1, result);
    }

    [Fact]
    public async void UpdateAsync()
    {
        var command = new CategoryCommand();
        command.Action = CommandAction.Update;
        command.Argument = new Category() { Id = 3, Name = "Sport" };

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(1, result);

        var category = await context.Categories.FindAsync(3);
        Assert.Equal("Sport", category!.Name);
    }

    [Fact]
    public async void DeleteAsync()
    {
        var command = new CategoryCommand();
        command.Action = CommandAction.Delete;
        command.Argument = 1;

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(1, result);
    }

    [Fact]
    public async void FilterAsync()
    {
        var query = new CategoryQuery();
        query.Parameters["Name"] = "Clothing";

        var list = await dispatcher.DispatchAsync(query) as List<Category>;
        Assert.NotNull(list);
        Assert.Single(list);
    }

    [Fact]
    public async void SortAsync()
    {
        var query = new CategoryQuery();
        query.Sort = "Name";

        var list = await dispatcher.DispatchAsync(query) as List<Category>;
        Assert.NotNull(list);

        var category1 = list[0];
        var category2 = list[1];
        var category3 = list[2];
        var category4 = list[3];

        Assert.Equal("Clothing", category1.Name);
        Assert.Equal("Shoes", category2.Name);
        Assert.Equal("Swimwear", category3.Name);
        Assert.Equal("Underwear", category4.Name);

        // reverse order by name
        query.Sort = "-Name";
        list = await dispatcher.DispatchAsync(query) as List<Category>;
        Assert.NotNull(list);

        category1 = list[0];
        category2 = list[1];
        category3 = list[2];
        category4 = list[3];

        Assert.Equal("Underwear", category1.Name);
        Assert.Equal("Swimwear", category2.Name);
        Assert.Equal("Shoes", category3.Name);
        Assert.Equal("Clothing", category4.Name);
    }

    [Fact]
    public async void PaginateAsync()
    {
        var query = new CategoryQuery();
        query.Offset = 2;
        query.Limit = 2;

        var list = await dispatcher.DispatchAsync(query) as List<Category>;
        Assert.NotNull(list);

        var category1 = list[0];
        var category2 = list[1];
        Assert.Equal("Underwear", category1.Name);
        Assert.Equal("Swimwear", category2.Name);
    }
}
