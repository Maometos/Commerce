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

        var tax1 = new Tax() { Name = "Ontario sales tax" };
        tax1.Rates.Add(new TaxRate() { Name = "HST", Value = 13 });

        var tax2 = new Tax() { Name = "Alberta sales tax" };
        tax2.Rates.Add(new TaxRate() { Name = "GST", Value = 5 });

        var tax3 = new Tax() { Name = "Saskatchewan sales Tax" };
        tax3.Rates.Add(new TaxRate() { Name = "GST", Value = 5 });
        tax3.Rates.Add(new TaxRate() { Name = "PST", Value = 6 });

        var tax4 = new Tax() { Name = "Quebec sales Tax" };
        tax4.Rates.Add(new TaxRate() { Name = "GST", Value = 5 });
        tax4.Rates.Add(new TaxRate() { Name = "QST", Value = 9.975 });

        context.Taxes.Add(tax1);
        context.Taxes.Add(tax2);
        context.Taxes.Add(tax3);
        context.Taxes.Add(tax4);
        context.SaveChanges();
    }

    [Fact]
    public async void TestCreating()
    {
        var tax = new Tax() { Name = "Manitoba sales tax" };
        tax.Rates.Add(new TaxRate() { Name = "GST", Value = 5 });
        tax.Rates.Add(new TaxRate() { Name = "RST", Value = 7 });

        var command = new TaxCommand();
        command.Action = CommandAction.Create;
        command.Argument = tax;

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(3, result);
    }

    [Fact]
    public async void TestUpdating()
    {
        var tax = new Tax() { Id = 4, Name = "Exempted sales tax" };
        var command = new TaxCommand();
        command.Action = CommandAction.Update;
        command.Argument = tax;

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(1, result);

        tax = await context.Taxes.FindAsync(4);
        Assert.Equal("Exempted sales tax", tax!.Name);
        Assert.Empty(tax.Rates);
    }

    [Fact]
    public async void TestDeleting()
    {
        var command = new TaxCommand();
        command.Action = CommandAction.Delete;
        command.Argument = 1;

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(2, result);
    }

    [Fact]
    public async void TestFinding()
    {
        var query = new TaxQuery();
        query.Action = QueryAction.Find;
        query.Parameters["Id"] = 1;

        var tax = await dispatcher.DispatchAsync(query) as Tax;
        Assert.NotNull(tax);
        Assert.Equal("Ontario sales tax", tax.Name);
    }

    [Fact]
    public async void TestFiltering()
    {
        var query = new TaxQuery();
        query.Action = QueryAction.List;
        query.Parameters["Name"] = "Alberta sales tax";

        var list = await dispatcher.DispatchAsync(query) as List<Tax>;
        Assert.NotNull(list);
        Assert.Single(list);
    }

    [Fact]
    public async void TestSorting()
    {
        var query = new TaxQuery();
        query.Action = QueryAction.List;
        query.Sort = "Name";

        var list = await dispatcher.DispatchAsync(query) as List<Tax>;
        Assert.NotNull(list);

        var tax1 = list[0];
        var tax2 = list[1];
        var tax3 = list[2];
        var tax4 = list[3];

        Assert.Equal("Alberta sales tax", tax1.Name);
        Assert.Equal("Ontario sales tax", tax2.Name);
        Assert.Equal("Quebec sales Tax", tax3.Name);
        Assert.Equal("Saskatchewan sales Tax", tax4.Name);
    }

    [Fact]
    public async void TestReverseSorting()
    {
        var query = new TaxQuery();
        query.Action = QueryAction.List;
        query.Sort = "-Name";

        var list = await dispatcher.DispatchAsync(query) as List<Tax>;
        Assert.NotNull(list);

        var tax1 = list[0];
        var tax2 = list[1];
        var tax3 = list[2];
        var tax4 = list[3];

        Assert.Equal("Saskatchewan sales Tax", tax1.Name);
        Assert.Equal("Quebec sales Tax", tax2.Name);
        Assert.Equal("Ontario sales tax", tax3.Name);
        Assert.Equal("Alberta sales tax", tax4.Name);
    }

    [Fact]
    public async void TestPaginating()
    {
        var query = new TaxQuery();
        query.Action = QueryAction.List;
        query.Offset = 2;
        query.Limit = 2;

        var list = await dispatcher.DispatchAsync(query) as List<Tax>;
        Assert.NotNull(list);

        var tax1 = list[0];
        var tax2 = list[1];
        Assert.Equal("Saskatchewan sales Tax", tax1.Name);
        Assert.Equal("Quebec sales Tax", tax2.Name);
    }
}
