using Commerce.Core.Common;
using Commerce.Core.Common.Values;
using Commerce.Core.Memo.Entities;
using Commerce.Core.Memo.Queries;
using Commerce.Infrastructure.CQRS;

namespace Commerce.Core.Memo.Handlers;

public class DebitMemoQueryHandler : QueryHandler<DebitMemoQuery, DebitMemo>
{
    private DataContext context;

    public DebitMemoQueryHandler(DataContext context)
    {
        this.context = context;
    }

    protected override async Task<List<DebitMemo>> FetchAsync(DebitMemoQuery query, CancellationToken token)
    {
        var queryable = context.DebitMemos.AsQueryable();

        if (query.Parameters.ContainsKey("Id"))
        {
            queryable = queryable.Where(e => e.Id == (int)query.Parameters["Id"]);
        }

        if (query.Parameters.ContainsKey("Reference"))
        {
            queryable = queryable.Where(e => e.Reference == (string)query.Parameters["Reference"]);
        }

        if (query.Parameters.ContainsKey("Total"))
        {
            queryable = queryable.Where(e => e.Total == (decimal)query.Parameters["Total"]);
        }

        if (query.Parameters.ContainsKey("Date"))
        {
            var date = (DateTime)query.Parameters["Date"];
            queryable = queryable.Where(e => e.Date.ToString("d") == date.ToString("d"));
        }

        if (query.Parameters.ContainsKey("Status"))
        {
            queryable = queryable.Where(e => e.Status == (AdjustmentStatus)query.Parameters["Status"]);
        }

        return await ListAsync(queryable, query, token);
    }
}
