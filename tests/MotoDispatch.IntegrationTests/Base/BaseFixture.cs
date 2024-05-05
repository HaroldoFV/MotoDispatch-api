using Bogus;
using Microsoft.EntityFrameworkCore;
using MotoDispatch.Infra.Data.EF;

namespace MotoDispatch.IntegrationTests.Base;

public class BaseFixture
{
    public BaseFixture()
        => Faker = new Faker("pt_BR");

    protected Faker Faker { get; set; }

    public MotoDispatchDbContext CreateDbContext(bool preserveData = false)
    {
        var context = new MotoDispatchDbContext(
            new DbContextOptionsBuilder<MotoDispatchDbContext>()
                .UseInMemoryDatabase("integration-tests-db")
                .Options
        );

        if (preserveData == false)
            context.Database.EnsureDeleted();
        return context;
    }
}