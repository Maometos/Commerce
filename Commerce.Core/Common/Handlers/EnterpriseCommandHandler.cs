using Commerce.Core.Common.Entities;
using Commerce.Core.Common.Requests;
using Commerce.Infrastructure.CQRS;
using Microsoft.EntityFrameworkCore;

namespace Commerce.Core.Common.Handlers;

public class EnterpriseCommandHandler : CommandHandler<EnterpriseCommand>
{
    private DataContext context;

    public EnterpriseCommandHandler(DataContext context)
    {
        this.context = context;
    }

    protected override async Task<int> CreateAsync(EnterpriseCommand command, CancellationToken token)
    {
        var enterprise = command.Argument as Enterprise;
        if (enterprise == null)
        {
            return 0;
        }

        context.Enterprises.Add(enterprise);
        return await context.SaveChangesAsync();
    }

    protected override async Task<int> UpdateAsync(EnterpriseCommand command, CancellationToken token)
    {
        var enterprise = command.Argument as Enterprise;
        if (enterprise == null)
        {
            return 0;
        }

        var entity = await context.Enterprises.FindAsync(enterprise.Id, token);
        if (entity == null)
        {
            return 0;
        }

        context.Entry(entity).State = EntityState.Detached;

        context.Enterprises.Update(enterprise);
        return await context.SaveChangesAsync(token);
    }

    protected override async Task<int> DeleteAsync(EnterpriseCommand command, CancellationToken token)
    {
        var enterprise = await context.Enterprises.FindAsync(command.Argument, token);
        if (enterprise == null)
        {
            return 0;
        }

        context.Enterprises.Remove(enterprise);
        return await context.SaveChangesAsync(token);
    }
}
