using Microsoft.EntityFrameworkCore;
using MotoDispatch.Application.Exceptions;
using MotoDispatch.Domain.Entity;
using MotoDispatch.Domain.Repository;

namespace MotoDispatch.Infra.Data.EF.Repositories;

public class RentalPlanRepository(MotoDispatchDbContext context)
    : IRentalPlanRepository
{
    private DbSet<RentalPlan> _rentalPlans => context.RentalPlans;

    public async Task Insert(
        RentalPlan aggregate,
        CancellationToken cancellationToken)
    {
        await _rentalPlans.AddAsync(aggregate, cancellationToken);
    }

    public async Task<RentalPlan> Get(Guid id, CancellationToken cancellationToken)
    {
        var rentalPlans = await _rentalPlans.AsNoTracking().FirstOrDefaultAsync(
            x => x.Id == id,
            cancellationToken
        );
        NotFoundException.ThrowIfNull(rentalPlans, $"RentalPlans '{id}' not found.");
        return rentalPlans!;
    }

    public Task Update(RentalPlan aggregate, CancellationToken cancellationToken)
    {
        return Task.FromResult(_rentalPlans.Update(aggregate));
    }

    public async Task<RentalPlan?> GetPlanAsync(int days, CancellationToken cancellationToken)
    {
        var plan = await _rentalPlans.AsNoTracking().FirstOrDefaultAsync(
            x => x.Days == days,
            cancellationToken
        );
        return plan;
    }

    public void AttachPlan(RentalPlan plan)
    {
        _rentalPlans.Attach(plan);
    }

    public Task Delete(RentalPlan aggregate, CancellationToken cancellationToken)
    {
        return Task.FromResult(_rentalPlans.Remove(aggregate));
    }
}