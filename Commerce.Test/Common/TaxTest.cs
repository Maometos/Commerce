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

        var tax1 = new Tax() { Name = "Quebec sales Tax" };
        tax1.Rates.Add(new TaxRate() { Name = "GST", Value = 5 });
        tax1.Rates.Add(new TaxRate() { Name = "QST", Value = 9.975 });

        var tax2 = new Tax() { Name = "Ontario sales tax" };
        tax2.Rates.Add(new TaxRate() { Name = "HST", Value = 8 });

        var tax3 = new Tax() { Name = "Exempted sales tax" };

        context.Taxes.Add(tax1);
        context.Taxes.Add(tax2);
        context.Taxes.Add(tax3);
        context.SaveChanges();
    }

    [Fact]
    public async void TestCreating()
    {
        var tax = new Tax() { Name = "France sales tax" };
        tax.Rates.Add(new TaxRate() { Name = "TVA", Value = 20 });

        var command = new TaxCommand();
        command.Action = CommandAction.Create;
        command.Argument = tax;

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(2, result);
    }

    [Fact]
    public async void TestUpdating()
    {
        var tax = new Tax() { Id = 2, Name = "Alberta sales tax" };
        tax.Rates.Add(new TaxRate() { Name = "GST", Value = 5 });

        var command = new TaxCommand();
        command.Action = CommandAction.Update;
        command.Argument = tax;

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(2, result);

        tax = await context.Taxes.FindAsync(2);
        Assert.Equal("Alberta sales tax", tax!.Name);
        Assert.Equal("GST", tax!.Rates[0].Name);
    }

    [Fact]
    public async void TestDeleting()
    {
        var command = new TaxCommand();
        command.Action = CommandAction.Delete;
        command.Argument = 1;

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(3, result);
    }
}
