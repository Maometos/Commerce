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
    public async void TestDispatchingFindQuery()
    {
        var query = new EntityQuery() { Id = 1, Action = QueryAction.Find };
        var list = (List<Entity>?)await dispatcher.DispatchAsync(query);
        Assert.NotEmpty(list!);
        Assert.Equal(1, list!.First().Id);
    }

    [Fact]
    public async void TestDispatchingListQuery()
    {
        var query = new EntityQuery() { Action = QueryAction.List };
        var list = (List<Entity>?)await dispatcher.DispatchAsync(query);
        Assert.NotEmpty(list!);
        Assert.Equal(2, list!.Count);
    }

    [Fact]
    public async void TestDispatchingFailedQuery()
    {
        var query = new EntityQuery();
        var list = (List<Entity>?)await dispatcher.DispatchAsync(query);
        Assert.Empty(list!);
    }

    [Fact]
    public async void TestDispatchingCreateCommand()
    {
        var command = new EntityCommand() { Action = CommandAction.Create };
        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal("Created", command.Log);
        Assert.Equal(1, result);
    }

    [Fact]
    public async void TestDispatchingUpdateCommand()
    {
        var command = new EntityCommand() { Action = CommandAction.Update };
        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal("Updated", command.Log);
        Assert.Equal(1, result);
    }

    [Fact]
    public async void TestDispatchingDeleteCommand()
    {
        var command = new EntityCommand() { Action = CommandAction.Delete };
        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal("Deleted", command.Log);
        Assert.Equal(1, result);
    }

    [Fact]
    public async void TestDispatchingFailedCommand()
    {
        var command = new EntityCommand();
        var result = await dispatcher.DispatchAsync(command);
        Assert.Equal("Failed", command.Log);
        Assert.Equal(0, result);
    }
}

public class Entity
{
    public int Id { get; set; } = 0;
}

public class EntityQuery : Query
{
    public int Id { get; set; } = 1;
}

public class EntityQueryHandler : QueryHandler<EntityQuery, Entity>
{
    protected override Task<Entity?> FindAsync(EntityQuery query, CancellationToken token)
    {
        if (query.Id <= 0)
        {
            return Task.FromResult<Entity?>(null);
        }

        var entity = new Entity() { Id = query.Id };
        return Task.FromResult<Entity?>(entity);
    }

    protected override Task<List<Entity>> ListAsync(EntityQuery query, CancellationToken token)
    {
        var entity1 = new Entity() { Id = 1 };
        var entity2 = new Entity() { Id = 2 };
        var list = new List<Entity>();
        list.Add(entity1);
        list.Add(entity2);

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
