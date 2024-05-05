using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MotoDispatch.Domain.SeedWork;
using Moq;
using MotoDispatch.Application;
using UnitOfWorkInfra = MotoDispatch.Infra.Data.EF;

namespace MotoDispatch.IntegrationTests.Infra.Data.EF.UnitOfWork;

[Collection(nameof(UnitOfWorkTestFixture))]
public class UnitOfWorkTest(UnitOfWorkTestFixture fixture)
{
    [Fact(DisplayName = nameof(Commit))]
    [Trait("Integration/Infra.Data", "UnitOfWork - Persistence")]
    public async Task Commit()
    {
        var dbContext = fixture.CreateDbContext();
        var exampleMotorcyclesList = fixture.GetExampleMotorcyclesList();
        await dbContext.AddRangeAsync(exampleMotorcyclesList);
        var unitOfWork = new UnitOfWorkInfra.UnitOfWork(dbContext);

        await unitOfWork.Commit(CancellationToken.None);

        var assertDbContext = fixture.CreateDbContext(true);

        var savedMotorcycles = assertDbContext.Motorcycles
            .AsNoTracking().ToList();

        savedMotorcycles.Should()
            .HaveCountGreaterThan(0);
    }

    [Fact(DisplayName = nameof(Rollback))]
    [Trait("Integration/Infra.Data", "UnitOfWork - Persistence")]
    public async Task Rollback()
    {
        var dbContext = fixture.CreateDbContext();

        var unitOfWork = new UnitOfWorkInfra.UnitOfWork(dbContext);

        var task = async ()
            => await unitOfWork.Rollback(CancellationToken.None);

        await task.Should().NotThrowAsync();
    }
}