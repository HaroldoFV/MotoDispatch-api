using MotoDispatch.Domain.Entity;
using MotoDispatch.Domain.SeedWork;

namespace MotoDispatch.Domain.Repository;

public interface IRentalPlanRepository
    : IGenericRepository<RentalPlan>
{
    public Task<RentalPlan?> GetPlanAsync(
        int days,
        CancellationToken cancellationToken);

    void AttachPlan(RentalPlan plan);
}