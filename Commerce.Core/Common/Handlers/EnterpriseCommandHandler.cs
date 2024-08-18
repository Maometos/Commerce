using Commerce.Core.Common.Entities;
using Commerce.Core.Common.Requests;
using Commerce.Infrastructure.CQRS;

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
        var item = new Enterprise();
        item.Name = command.Name!;
        item.Email = command.Email!;
        item.Phone = command.Phone!;
        item.Address = command.Address!;
        item.Locality = command.Locality!;
        item.Territory = command.Territory!;
        item.Postcode = command.Postcode!;
        item.Country = command.Country!;

        context.Enterprises.Add(item);
        return await context.SaveChangesAsync();
    }

    protected override async Task<int> UpdateAsync(EnterpriseCommand command, CancellationToken token)
    {
        var item = await context.Enterprises.FindAsync(command.Id, token);
        if (item == null)
        {
            return 0;
        }

        item.Name = command.Name ?? item.Name;
        item.Email = command.Email ?? item.Email;
        item.Phone = command.Phone ?? item.Phone;
        item.Address = command.Address ?? item.Address;
        item.Locality = command.Locality ?? item.Locality;
        item.Territory = command.Territory ?? item.Territory;
        item.Postcode = command.Postcode ?? item.Postcode;
        item.Country = command.Country ?? item.Country;

        context.Enterprises.Update(item);
        return await context.SaveChangesAsync(token);
    }

    protected override async Task<int> DeleteAsync(EnterpriseCommand command, CancellationToken token)
    {
        var item = await context.Enterprises.FindAsync(command.Id, token);
        if (item == null)
        {
            return 0;
        }

        context.Enterprises.Remove(item);
        return await context.SaveChangesAsync(token);
    }
}
