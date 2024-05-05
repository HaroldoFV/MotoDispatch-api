using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MotoDispatch.Application.Interfaces;
using MotoDispatch.Domain.SeedWork;

namespace MotoDispatch.Infra.Data.EF;

public class UnitOfWork(
    MotoDispatchDbContext context
)
    : IUnitOfWork
{
    public async Task Commit(CancellationToken cancellationToken)
    {
        var aggregateRoots = context.ChangeTracker
            .Entries<AggregateRoot>()
            .Select(entry => entry.Entity);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            throw new ApplicationException("An error occurred while saving changes.", ex.InnerException);
        }
    }

    public Task Rollback(CancellationToken cancellationToken)
        => Task.CompletedTask;
}