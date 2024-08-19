using Commerce.Core.Common;
using Commerce.Core.Common.Entities;
using Commerce.Core.Common.Handlers;
using Commerce.Core.Common.Requests;
using Commerce.Infrastructure.CQRS;
using Commerce.Infrastructure.Dispatcher;
using Microsoft.EntityFrameworkCore;

namespace Commerce.Test.Common;

public class EnterpriseTest
{
    private EventDispatcher dispatcher;
    private DataContext context;

    public EnterpriseTest()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        context = new DataContext(options);
        dispatcher = new EventDispatcher();
        dispatcher.AddService(context);
        dispatcher.AddHandler<EnterpriseCommandHandler>();
        dispatcher.AddHandler<EnterpriseQueryHandler>();

        var enterprise1 = new Enterprise() { Id = 1, Name = "Microsoft", Email = "contact@microsoft.com" };
        var enterprise2 = new Enterprise() { Id = 2, Name = "Oracle", Email = "contact@oracle.com" };
        var enterprise3 = new Enterprise() { Id = 3, Name = "Adobe", Email = "contact@adobe.com" };
        var enterprise4 = new Enterprise() { Id = 4, Name = "Canonical", Email = "contact@canonical.com" };

        context.Enterprises.Add(enterprise1);
        context.Enterprises.Add(enterprise2);
        context.Enterprises.Add(enterprise3);
        context.Enterprises.Add(enterprise4);
        context.SaveChanges();
    }

    [Fact]
    public async void TestCreating()
    {
        var command = new EnterpriseCommand();
        command.Action = CommandAction.Create;
        command.Name = "Intellectec";
        command.Email = "contact@intellectec.net";

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(1, result);
    }

    [Fact]
    public async void TestUpdating()
    {
        var command = new EnterpriseCommand();
        command.Action = CommandAction.Update;
        command.Id = 4;
        command.Email = "support@canonical.com";

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(1, result);

        var enterprise = await context.Enterprises.FindAsync(4);
        Assert.Equal("support@canonical.com", enterprise!.Email);
    }

    [Fact]
    public async void TestDeleting()
    {
        var command = new EnterpriseCommand();
        command.Action = CommandAction.Delete;
        command.Id = 1;

        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal(1, result);
    }

    [Fact]
    public async void TestFinding()
    {
        var query = new EnterpriseQuery();
        query.Action = QueryAction.Find;
        query.Id = 1;

        var enterprise = await dispatcher.DispatchAsync(query) as Enterprise;
        Assert.NotNull(enterprise);
        Assert.Equal("Microsoft", enterprise.Name);
    }

    [Fact]
    public async void TestFiltering()
    {
        var query = new EnterpriseQuery();
        query.Action = QueryAction.List;
        query.Name = "Canonical";

        var list = await dispatcher.DispatchAsync(query) as List<Enterprise>;
        Assert.NotNull(list);
        Assert.Single(list);
    }

    [Fact]
    public async void TestSorting()
    {
        var query = new EnterpriseQuery();
        query.Action = QueryAction.List;
        query.Sort = "Name";

        var list = await dispatcher.DispatchAsync(query) as List<Enterprise>;
        Assert.NotNull(list);

        var enterprise1 = list[0];
        var enterprise2 = list[1];
        var enterprise3 = list[2];
        var enterprise4 = list[3];

        Assert.Equal("Adobe", enterprise1.Name);
        Assert.Equal("Canonical", enterprise2.Name);
        Assert.Equal("Microsoft", enterprise3.Name);
        Assert.Equal("Oracle", enterprise4.Name);
    }

    [Fact]
    public async void TestReverseSorting()
    {
        var query = new EnterpriseQuery();
        query.Action = QueryAction.List;
        query.Sort = "Name";
        query.Reverse = true;

        var list = await dispatcher.DispatchAsync(query) as List<Enterprise>;
        Assert.NotNull(list);

        var enterprise1 = list[0];
        var enterprise2 = list[1];
        var enterprise3 = list[2];
        var enterprise4 = list[3];

        Assert.Equal("Oracle", enterprise1.Name);
        Assert.Equal("Microsoft", enterprise2.Name);
        Assert.Equal("Canonical", enterprise3.Name);
        Assert.Equal("Adobe", enterprise4.Name);
    }

    [Fact]
    public async void TestPaginating()
    {
        var query = new EnterpriseQuery();
        query.Action = QueryAction.List;
        query.Page = 2;
        query.Limit = 2;

        var list = await dispatcher.DispatchAsync(query) as List<Enterprise>;
        Assert.NotNull(list);

        var enterprise1 = list[0];
        var enterprise2 = list[1];
        Assert.Equal("Adobe", enterprise1.Name);
        Assert.Equal("Canonical", enterprise2.Name);
    }
}
