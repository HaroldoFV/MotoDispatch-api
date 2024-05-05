using Microsoft.EntityFrameworkCore;
using MotoDispatch.Application.Exceptions;
using MotoDispatch.Domain.Entity;
using MotoDispatch.Domain.Repository;
using MotoDispatch.Domain.SeedWork.SearchableRepository;

namespace MotoDispatch.Infra.Data.EF.Repositories;

public class DeliveryDriverRepository(MotoDispatchDbContext context)
    : IDeliveryDriverRepository
{
    private DbSet<DeliveryDriver> _deliveryDrivers => context.DeliveryDrivers;

    public async Task Insert(
        DeliveryDriver aggregate,
        CancellationToken cancellationToken)
    {
        await _deliveryDrivers.AddAsync(aggregate, cancellationToken);
    }

    public async Task<DeliveryDriver> Get(Guid id, CancellationToken cancellationToken)
    {
        var deliveryDriver = await _deliveryDrivers.AsNoTracking().FirstOrDefaultAsync(
            x => x.Id == id,
            cancellationToken
        );
        NotFoundException.ThrowIfNull(deliveryDriver, $"DeliveryDriver '{id}' not found.");
        return deliveryDriver!;
    }

    public Task Update(DeliveryDriver aggregate, CancellationToken cancellationToken)
    {
        return Task.FromResult(_deliveryDrivers.Update(aggregate));
    }

    public Task Delete(DeliveryDriver aggregate, CancellationToken cancellationToken)
    {
        return Task.FromResult(_deliveryDrivers.Remove(aggregate));
    }

    public async Task<SearchOutput<DeliveryDriver>> Search(SearchInput input, CancellationToken cancellationToken)
    {
        var toSkip = (input.Page - 1) * input.PerPage;
        var query = _deliveryDrivers.AsNoTracking();
        query = AddOrderToQuery(query, input.OrderBy, input.Order);

        if (!String.IsNullOrWhiteSpace(input.Search))
            query = query.Where(x => x.Name.ToLower().Contains(input.Search.ToLower()));
        var total = await query.CountAsync(cancellationToken: cancellationToken);

        var items = await query
            .Skip(toSkip)
            .Take(input.PerPage)
            .ToListAsync(cancellationToken: cancellationToken);
        return new(input.Page, input.PerPage, total, items);
    }

    private IQueryable<DeliveryDriver> AddOrderToQuery(
        IQueryable<DeliveryDriver> query,
        string orderProperty,
        SearchOrder order
    )
    {
        var orderedQuery = (orderProperty.ToLower(), order) switch
        {
            ("name", SearchOrder.Asc) => query.OrderBy(x => x.Name)
                .ThenBy(x => x.Id),
            ("name", SearchOrder.Desc) => query.OrderByDescending(x => x.Name)
                .ThenByDescending(x => x.Id),
            ("id", SearchOrder.Asc) => query.OrderBy(x => x.Id),
            ("id", SearchOrder.Desc) => query.OrderByDescending(x => x.Id),
            ("createdat", SearchOrder.Asc) => query.OrderBy(x => x.CreatedAt),
            ("createdat", SearchOrder.Desc) => query.OrderByDescending(x => x.CreatedAt),
            _ => query.OrderBy(x => x.Name)
                .ThenBy(x => x.Id)
        };
        return orderedQuery;
    }

    public async Task<DeliveryDriver?> GetByCNPJAsync(string cnpj, CancellationToken cancellationToken)
    {
        var deliveryDriver = await _deliveryDrivers.AsNoTracking().FirstOrDefaultAsync(
            x => x.CNPJ == cnpj,
            cancellationToken
        );
        return deliveryDriver;
    }

    public async Task<DeliveryDriver?> GetByCNHNumberAsync(string cnhNumber, CancellationToken cancellationToken)
    {
        var deliveryDriver = await _deliveryDrivers.AsNoTracking().FirstOrDefaultAsync(
            x => x.CNHNumber == cnhNumber,
            cancellationToken
        );
        return deliveryDriver;
    }
}