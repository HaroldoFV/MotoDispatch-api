using MotoDispatch.Domain.Entity;
using MotoDispatch.Domain.SeedWork;
using MotoDispatch.Domain.SeedWork.SearchableRepository;

namespace MotoDispatch.Domain.Repository;

public interface IRentalRepository
    : IGenericRepository<Rental>,
        ISearchableRepository<Rental>
{
    Task<Rental?> GetExistingRentalByMotorcycle(
        Guid motorcycleId,
        CancellationToken cancellationToken
    );

    Task<Rental?> GetRentalByMotorcycle(
        Guid motorcycleId,
        CancellationToken cancellationToken
    );
}