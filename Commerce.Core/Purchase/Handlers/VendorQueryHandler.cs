using Commerce.Core.Common;
using Commerce.Core.Purchase.Entities;
using Commerce.Core.Sale.Requests;
using Commerce.Infrastructure.CQRS;
using Microsoft.EntityFrameworkCore;

namespace Commerce.Core.Purchase.Handlers;

public class VendorQueryHandler : QueryHandler<VendorQuery, Vendor>
{
    private DataContext context;

    public VendorQueryHandler(DataContext context)
    {
        this.context = context;
    }

    protected override async Task<Vendor?> FindAsync(VendorQuery query, CancellationToken token)
    {
        if (query.Id > 0)
        {
            return await context.Vendors.FindAsync(query.Id, token);
        }

        if (!string.IsNullOrEmpty(query.Email))
        {
            return await context.Vendors.FirstOrDefaultAsync(vendor => vendor.Email!.ToLower() == query.Email.ToLower(), token);
        }

        if (!string.IsNullOrEmpty(query.Phone))
        {
            return await context.Vendors.FirstOrDefaultAsync(vendor => vendor.Phone!.ToLower() == query.Phone.ToLower(), token);
        }

        return null;
    }

    protected override Task<List<Vendor>> ListAsync(VendorQuery query, CancellationToken token)
    {
        var vendors = context.Vendors.AsQueryable();
        if (!string.IsNullOrEmpty(query.Name))
        {
            vendors = vendors.Where(vendor => vendor.Name.ToLower().Contains(query.Name.ToLower()));
        }

        if (!string.IsNullOrEmpty(query.Email))
        {
            vendors = vendors.Where(vendor => vendor.Email!.ToLower().Contains(query.Email.ToLower()));
        }

        if (!string.IsNullOrEmpty(query.Phone))
        {
            vendors = vendors.Where(vendor => vendor.Phone!.ToLower().Contains(query.Phone.ToLower()));
        }

        if (!string.IsNullOrEmpty(query.Locality))
        {
            vendors = vendors.Where(vendor => vendor.Locality!.ToLower().Contains(query.Locality.ToLower()));
        }

        if (!string.IsNullOrEmpty(query.Territory))
        {
            vendors = vendors.Where(vendor => vendor.Territory!.ToLower().Contains(query.Territory.ToLower()));
        }

        if (!string.IsNullOrEmpty(query.Country))
        {
            vendors = vendors.Where(vendor => vendor.Country!.ToLower().Contains(query.Country.ToLower()));
        }

        if (!string.IsNullOrEmpty(query.Sort))
        {
            vendors = vendors.Sort(query.Sort, query.Reverse);
        }

        return vendors.Paginate(query.Page, query.Limit).ToListAsync(token);
    }
}
