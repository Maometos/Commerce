using Commerce.Core.Common;
using Commerce.Core.Common.Entities;
using Commerce.Core.Common.Handlers;
using Commerce.Core.Common.Requests;
using Commerce.Infrastructure.CQRS;
using Commerce.Infrastructure.Dispatcher;
using Microsoft.EntityFrameworkCore;

namespace Commerce.Test.Common;

public class TaxTest
{
    private EventDispatcher dispatcher;
    private DataContext context;

    public TaxTest()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        context = new DataContext(options);
        dispatcher = new EventDispatcher();
        dispatcher.AddService(context);
        dispatcher.AddHandler<TaxCommandHandler>();
        dispatcher.AddHandler<TaxQueryHandler>();

        var group1 = new TaxGroup() { Name = "Ontario sales tax" };
        group1.Taxes.Add(new Tax() { Name = "HST", Rate = 13 });

        var group2 = new TaxGroup() { Name = "Alberta sales tax" };
        group2.Taxes.Add(new Tax() { Name = "GST", Rate = 5 });

        var group3 = new TaxGroup() { Name = "Saskatchewan sales Tax" };
        group3.Taxes.Add(new Tax() { Name = "GST", Rate = 5 });
        group3.Taxes.Add(new Tax() { Name = "PST", Rate = 6 });

        var group4 = new TaxGroup() { Name = "Quebec sales Tax" };
        group4.Taxes.Add(new Tax() { Name = "GST", Rate = 5 });
        group4.Taxes.Add(new Tax() { Name = "QST", Rate = 9.975m });

        context.TaxGroups.Add(group1);
        context.TaxGroups.Add(group2);
        context.TaxGroups.Add(group3);
        context.TaxGroups.Add(group4);
        context.SaveChanges();
    }

    [Fact]
    public async void CreateAsync()
    {
        var group = new TaxGroup() { Name = "Manitoba sales tax" };
        group.Taxes.Add(new Tax() { Name = "GST", Rate = 5 });
        group.Taxes.Add(new Tax() { Name = "RST", Rate = 7 });

        var command = new TaxGroupCommand();
        command.Action = CommandAction.Create;
        command.Argument = group;

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(3, result);
    }

    [Fact]
    public async void UpdateAsync()
    {
        var group = new TaxGroup() { Id = 4, Name = "Exempted sales tax" };
        var command = new TaxGroupCommand();
        command.Action = CommandAction.Update;
        command.Argument = group;

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(1, result);

        group = await context.TaxGroups.FindAsync(4);
        Assert.Equal("Exempted sales tax", group!.Name);
        Assert.Empty(group.Taxes);
    }

    [Fact]
    public async void DeleteAsync()
    {
        var command = new TaxGroupCommand();
        command.Action = CommandAction.Delete;
        command.Argument = 1;

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(2, result);
    }

    [Fact]
    public async void FilterAsync()
    {
        var query = new TaxGroupQuery();
        query.Parameters["Name"] = "Alberta sales tax";

        var list = await dispatcher.DispatchAsync(query) as List<TaxGroup>;
        Assert.NotNull(list);
        Assert.Single(list);
    }

    [Fact]
    public async void SortAsync()
    {
        var query = new TaxGroupQuery();
        query.Sort = "Name";

        var list = await dispatcher.DispatchAsync(query) as List<TaxGroup>;
        Assert.NotNull(list);

        var group1 = list[0];
        var group2 = list[1];
        var group3 = list[2];
        var group4 = list[3];

        Assert.Equal("Alberta sales tax", group1.Name);
        Assert.Equal("Ontario sales tax", group2.Name);
        Assert.Equal("Quebec sales Tax", group3.Name);
        Assert.Equal("Saskatchewan sales Tax", group4.Name);

        // reverse order by name
        query.Sort = "-Name";
        list = await dispatcher.DispatchAsync(query) as List<TaxGroup>;
        Assert.NotNull(list);

        group1 = list[0];
        group2 = list[1];
        group3 = list[2];
        group4 = list[3];

        Assert.Equal("Saskatchewan sales Tax", group1.Name);
        Assert.Equal("Quebec sales Tax", group2.Name);
        Assert.Equal("Ontario sales tax", group3.Name);
        Assert.Equal("Alberta sales tax", group4.Name);
    }

    [Fact]
    public async void PaginateAsync()
    {
        var query = new TaxGroupQuery();
        query.Offset = 2;
        query.Limit = 2;

        var list = await dispatcher.DispatchAsync(query) as List<TaxGroup>;
        Assert.NotNull(list);

        var group1 = list[0];
        var group2 = list[1];
        Assert.Equal("Saskatchewan sales Tax", group1.Name);
        Assert.Equal("Quebec sales Tax", group2.Name);
    }
}
