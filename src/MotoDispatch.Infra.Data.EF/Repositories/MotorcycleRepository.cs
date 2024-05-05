using Microsoft.EntityFrameworkCore;
using MotoDispatch.Application.Exceptions;
using MotoDispatch.Domain.Entity;
using MotoDispatch.Domain.Repository;
using MotoDispatch.Domain.SeedWork.SearchableRepository;

namespace MotoDispatch.Infra.Data.EF.Repositories;

public class MotorcycleRepository(MotoDispatchDbContext context)
    : IMotorcycleRepository
{
    private DbSet<Motorcycle> _motorcycles => context.Motorcycles;

    public async Task Insert(
        Motorcycle aggregate,
        CancellationToken cancellationToken)
    {
        await _motorcycles.AddAsync(aggregate, cancellationToken);
    }

    public async Task<Motorcycle> Get(Guid id, CancellationToken cancellationToken)
    {
        var motorcycle = await _motorcycles.AsNoTracking().FirstOrDefaultAsync(
            x => x.Id == id,
            cancellationToken
        );
        NotFoundException.ThrowIfNull(motorcycle, $"Motorcycle '{id}' not found.");
        return motorcycle!;
    }

    public Task Update(Motorcycle aggregate, CancellationToken cancellationToken)
    {
        return Task.FromResult(_motorcycles.Update(aggregate));
    }

    public Task Delete(Motorcycle aggregate, CancellationToken cancellationToken)
    {
        return Task.FromResult(_motorcycles.Remove(aggregate));
    }

    public async Task<SearchOutput<Motorcycle>> Search(SearchInput input, CancellationToken cancellationToken)
    {
        var toSkip = (input.Page - 1) * input.PerPage;
        var query = _motorcycles.AsNoTracking();
        query = AddOrderToQuery(query, input.OrderBy, input.Order);

        if (!String.IsNullOrWhiteSpace(input.Search))
            query = query.Where(x => x.LicensePlate.Contains(input.Search));
        var total = await query.CountAsync(cancellationToken: cancellationToken);

        var items = await query
            .Skip(toSkip)
            .Take(input.PerPage)
            .ToListAsync(cancellationToken: cancellationToken);
        return new(input.Page, input.PerPage, total, items);
    }

    private IQueryable<Motorcycle> AddOrderToQuery(
        IQueryable<Motorcycle> query,
        string orderProperty,
        SearchOrder order
    )
    {
        var orderedQuery = (orderProperty.ToLower(), order) switch
        {
            ("licensePlate", SearchOrder.Asc) => query.OrderBy(x => x.LicensePlate)
                .ThenBy(x => x.Id),
            ("licensePlate", SearchOrder.Desc) => query.OrderByDescending(x => x.LicensePlate)
                .ThenByDescending(x => x.Id),
            ("id", SearchOrder.Asc) => query.OrderBy(x => x.Id),
            ("id", SearchOrder.Desc) => query.OrderByDescending(x => x.Id),
            ("createdat", SearchOrder.Asc) => query.OrderBy(x => x.CreatedAt),
            ("createdat", SearchOrder.Desc) => query.OrderByDescending(x => x.CreatedAt),
            _ => query.OrderBy(x => x.LicensePlate)
                .ThenBy(x => x.Id)
        };
        return orderedQuery;
    }

    public async Task<IReadOnlyList<Guid>> GetIdsListByIds(
        List<Guid> ids,
        CancellationToken cancellationToken
    )
    {
        return await _motorcycles.AsNoTracking()
            .Where(category => ids.Contains(category.Id))
            .Select(category => category.Id).ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<IReadOnlyList<Motorcycle>> GetListByIds(List<Guid> ids, CancellationToken cancellationToken)
    {
        return await _motorcycles.AsNoTracking()
            .Where(category => ids.Contains(category.Id))
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<Motorcycle?> GetByLicensePlate(string inputLicensePlate, CancellationToken cancellationToken)
    {
        var motorcycle = await _motorcycles.AsNoTracking().FirstOrDefaultAsync(
            x => x.LicensePlate == inputLicensePlate,
            cancellationToken
        );
        return motorcycle;
    }
}