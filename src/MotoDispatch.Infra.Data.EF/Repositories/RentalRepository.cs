using Microsoft.EntityFrameworkCore;
using MotoDispatch.Application.Exceptions;
using MotoDispatch.Domain.Entity;
using MotoDispatch.Domain.Repository;
using MotoDispatch.Domain.SeedWork.SearchableRepository;

namespace MotoDispatch.Infra.Data.EF.Repositories;

public class RentalRepository(MotoDispatchDbContext context)
    : IRentalRepository
{
    private DbSet<Rental> _rentals => context.Rentals;

    public async Task Insert(
        Rental aggregate,
        CancellationToken cancellationToken)
    {
        await _rentals.AddAsync(aggregate, cancellationToken);
    }

    public async Task<Rental> Get(Guid id, CancellationToken cancellationToken)
    {
        var rental = await _rentals
            .Include(plan => plan.RentalPlan)
            .AsNoTracking().FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken
            );
        NotFoundException.ThrowIfNull(rental, $"Rental '{id}' not found.");
        return rental!;
    }

    public Task Update(Rental aggregate, CancellationToken cancellationToken)
    {
        return Task.FromResult(_rentals.Update(aggregate));
    }

    public Task Delete(Rental aggregate, CancellationToken cancellationToken)
    {
        return Task.FromResult(_rentals.Remove(aggregate));
    }

    public async Task<Rental?> GetExistingRentalByMotorcycle(
        Guid motorcycleId,
        CancellationToken cancellationToken
    )
    {
        var existingRental = await _rentals.AsNoTracking().FirstOrDefaultAsync(
            rental => rental.MotorcycleId == motorcycleId,
            cancellationToken
        );
        return existingRental;
    }

    public async Task<Rental?> GetRentalByMotorcycle(
        Guid motorcycleId,
        CancellationToken cancellationToken
    )
    {
        var existingRental = await _rentals.AsNoTracking().FirstOrDefaultAsync(
            rental => rental.MotorcycleId == motorcycleId,
            cancellationToken
        );
        return existingRental;
    }

    public async Task<SearchOutput<Rental>> Search(SearchInput input, CancellationToken cancellationToken)
    {
        var toSkip = (input.Page - 1) * input.PerPage;
        var query = _rentals.AsNoTracking();
        query = AddOrderToQuery(query, input.OrderBy, input.Order);

        if (!String.IsNullOrWhiteSpace(input.Search) && Guid.TryParse(input.Search, out Guid searchGuid))
            query = query.Where(x => x.DeliveryDriverId == searchGuid);

        var total = await query.CountAsync(cancellationToken: cancellationToken);

        var items = await query
            .Skip(toSkip)
            .Take(input.PerPage)
            .ToListAsync(cancellationToken: cancellationToken);
        return new(input.Page, input.PerPage, total, items);
    }

    private IQueryable<Rental> AddOrderToQuery(
        IQueryable<Rental> query,
        string orderProperty,
        SearchOrder order
    )
    {
        var orderedQuery = (orderProperty.ToLower(), order) switch
        {
            ("deliveryDriverId", SearchOrder.Asc) => query.OrderBy(x => x.DeliveryDriverId)
                .ThenBy(x => x.Id),
            ("deliveryDriverId", SearchOrder.Desc) => query.OrderByDescending(x => x.DeliveryDriverId)
                .ThenByDescending(x => x.Id),
            ("id", SearchOrder.Asc) => query.OrderBy(x => x.Id),
            ("id", SearchOrder.Desc) => query.OrderByDescending(x => x.Id),
            ("createdat", SearchOrder.Asc) => query.OrderBy(x => x.CreatedAt),
            ("createdat", SearchOrder.Desc) => query.OrderByDescending(x => x.CreatedAt),
            _ => query.OrderBy(x => x.DeliveryDriverId)
                .ThenBy(x => x.Id)
        };
        return orderedQuery;
    }
}