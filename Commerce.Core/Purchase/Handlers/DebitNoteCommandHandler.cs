using Commerce.Core.Common;
using Commerce.Core.Purchase.Entities;
using Commerce.Core.Purchase.Requests;
using Commerce.Infrastructure.CQRS;
using Microsoft.EntityFrameworkCore;

namespace Commerce.Core.Purchase.Handlers;

public class DebitNoteCommandHandler : CommandHandler<DebitNoteCommand>
{
    private DataContext context;

    public DebitNoteCommandHandler(DataContext context)
    {
        this.context = context;
    }

    protected override async Task<int> CreateAsync(DebitNoteCommand command, CancellationToken token)
    {
        var debitNote = command.Argument as DebitNote;
        if (debitNote == null)
        {
            return 0;
        }

        context.DebitNotes.Add(debitNote);
        return await context.SaveChangesAsync();
    }

    protected override async Task<int> UpdateAsync(DebitNoteCommand command, CancellationToken token)
    {
        var debitNote = command.Argument as DebitNote;
        if (debitNote == null)
        {
            return 0;
        }

        var entity = await context.DebitNotes.FindAsync(debitNote.Id, token);
        if (entity == null)
        {
            return 0;
        }

        context.Entry(entity).State = EntityState.Detached;

        context.DebitNotes.Update(debitNote);
        return await context.SaveChangesAsync(token);
    }

    protected override async Task<int> DeleteAsync(DebitNoteCommand command, CancellationToken token)
    {
        var debitNote = await context.DebitNotes.FindAsync(command.Argument, token);
        if (debitNote == null)
        {
            return 0;
        }

        context.DebitNotes.Remove(debitNote);
        return await context.SaveChangesAsync(token);
    }
}
