using MotoDispatch.Domain.Entity;
using MotoDispatch.Domain.SeedWork;
using MotoDispatch.Domain.SeedWork.SearchableRepository;

namespace MotoDispatch.Domain.Repository;

public interface IDeliveryDriverRepository
    : IGenericRepository<DeliveryDriver>,
        ISearchableRepository<DeliveryDriver>
{
    public Task<DeliveryDriver?> GetByCNPJAsync(string cnpj, CancellationToken cancellationToken);
    public Task<DeliveryDriver?> GetByCNHNumberAsync(string cnhNumber, CancellationToken cancellationToken);
}