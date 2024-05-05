using MotoDispatch.Domain.Entity;
using MotoDispatch.Domain.SeedWork;
using MotoDispatch.Domain.SeedWork.SearchableRepository;

namespace MotoDispatch.Domain.Repository;

public interface IMotorcycleRepository
    : IGenericRepository<Motorcycle>,
        ISearchableRepository<Motorcycle>
{
    public Task<IReadOnlyList<Guid>> GetIdsListByIds(
        List<Guid> ids,
        CancellationToken cancellationToken
    );

    public Task<IReadOnlyList<Motorcycle>> GetListByIds(
        List<Guid> ids,
        CancellationToken cancellationToken
    );

    public Task<Motorcycle?> GetByLicensePlate(
        string inputLicensePlate,
        CancellationToken cancellationToken);
}