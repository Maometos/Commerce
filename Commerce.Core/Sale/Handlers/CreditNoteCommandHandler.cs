using Commerce.Core.Common;
using Commerce.Core.Sale.Entities;
using Commerce.Core.Sale.Requests;
using Commerce.Infrastructure.CQRS;
using Microsoft.EntityFrameworkCore;

namespace Commerce.Core.Sale.Handlers;

public class CreditNoteCommandHandler : CommandHandler<CreditNoteCommand>
{
    private DataContext context;

    public CreditNoteCommandHandler(DataContext context)
    {
        this.context = context;
    }

    protected override async Task<int> CreateAsync(CreditNoteCommand command, CancellationToken token)
    {
        var creditNote = command.Argument as CreditNote;
        if (creditNote == null)
        {
            return 0;
        }

        context.CreditNotes.Add(creditNote);
        return await context.SaveChangesAsync();
    }

    protected override async Task<int> UpdateAsync(CreditNoteCommand command, CancellationToken token)
    {
        var creditNote = command.Argument as CreditNote;
        if (creditNote == null)
        {
            return 0;
        }

        var entity = await context.CreditNotes.FindAsync(creditNote.Id, token);
        if (entity == null)
        {
            return 0;
        }

        context.Entry(entity).State = EntityState.Detached;

        context.CreditNotes.Update(creditNote);
        return await context.SaveChangesAsync(token);
    }

    protected override async Task<int> DeleteAsync(CreditNoteCommand command, CancellationToken token)
    {
        var creditNote = await context.CreditNotes.FindAsync(command.Argument, token);
        if (creditNote == null)
        {
            return 0;
        }

        context.CreditNotes.Remove(creditNote);
        return await context.SaveChangesAsync(token);
    }
}
