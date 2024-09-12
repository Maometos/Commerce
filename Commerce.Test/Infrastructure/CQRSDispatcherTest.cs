using Commerce.Infrastructure.CQRS;
using Commerce.Infrastructure.Dispatcher;

namespace Commerce.Test.Infrastructure;

public class CQRSDispatcherTest
{
    private EventDispatcher dispatcher;

    public CQRSDispatcherTest()
    {
        dispatcher = new EventDispatcher();
        dispatcher.AddService(new Response());
        dispatcher.AddHandler<EntityQueryHandler>();
        dispatcher.AddHandler<EntityCommandHandler>();
    }

    [Fact]
    public async void DispatchFetchingQueryAsync()
    {
        var query = new EntityQuery();
        query.Parameters["Id"] = 2;
        var list = await dispatcher.DispatchAsync(query) as List<CustomEntity>;
        Assert.NotNull(list);
        Assert.Equal(2, list[0].Id);
    }

    [Fact]
    public async void DispatchFailedQueryAsync()
    {
        var query = new EntityQuery();
        var list = await dispatcher.DispatchAsync(query) as List<CustomEntity>;
        Assert.NotNull(list);
        Assert.Empty(list);
    }

    [Fact]
    public async void DispatchCreateCommandAsync()
    {
        var command = new EntityCommand() { Action = CommandAction.Create };
        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal("Created", command.Log);
        Assert.Equal(1, result);
    }

    [Fact]
    public async void DispatchUpdateCommandAsync()
    {
        var command = new EntityCommand() { Action = CommandAction.Update };
        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal("Updated", command.Log);
        Assert.Equal(1, result);
    }

    [Fact]
    public async void DispatchDeleteCommandAsync()
    {
        var command = new EntityCommand() { Action = CommandAction.Delete };
        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal("Deleted", command.Log);
        Assert.Equal(1, result);
    }

    [Fact]
    public async void DispatchFailedCommandAsync()
    {
        var command = new EntityCommand();
        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal("Failed", command.Log);
        Assert.Equal(0, result);
    }
}

public class CustomEntity : Entity
{
}

public class EntityQuery : Query
{
    public int Id { get; set; } = 1;
}

public class EntityQueryHandler : QueryHandler<EntityQuery, CustomEntity>
{
    protected override Task<List<CustomEntity>> FetchAsync(EntityQuery query, CancellationToken token)
    {
        var list = new List<CustomEntity>();

        if (query.Parameters.ContainsKey("Id"))
        {
            switch ((int)query.Parameters["Id"])
            {
                case 1:
                    list.Add(new CustomEntity() { Id = 1 });
                    break;
                case 2:
                    list.Add(new CustomEntity() { Id = 2 });
                    break;
                case 3:
                    list.Add(new CustomEntity() { Id = 3 });
                    break;
            }
        }

        return Task.FromResult(list);
    }
}

public class EntityCommand : Command
{
    public string Log { get; set; } = "Failed";
}

public class EntityCommandHandler : CommandHandler<EntityCommand>
{
    protected override Task<int> CreateAsync(EntityCommand command, CancellationToken token)
    {
        command.Log = "Created";
        return Task.FromResult(1);
    }

    protected override Task<int> DeleteAsync(EntityCommand command, CancellationToken token)
    {
        command.Log = "Deleted";
        return Task.FromResult(1);
    }

    protected override Task<int> UpdateAsync(EntityCommand command, CancellationToken token)
    {
        command.Log = "Updated";
        return Task.FromResult(1);
    }
}
