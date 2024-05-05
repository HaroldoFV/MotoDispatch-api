using FluentAssertions;
using MotoDispatch.Infra.Data.EF;
using MotoDispatch.Infra.Data.EF.Repositories;
using ApplicationUseCases
    = MotoDispatch.Application.UseCases.Motorcycle.CreateMotorcycle;

namespace MotoDispatch.IntegrationTests.UseCases.Motorcycle.CreateMotorcycle;

[Collection(nameof(CreateMotorcycleTestFixture))]
public class CreateMotorcycleTest(CreateMotorcycleTestFixture fixture)
{
    [Fact(DisplayName = nameof(CreateMotorcycle))]
    [Trait("Integration/Application", "CreateMotorcycle - Use Cases")]
    public async void CreateMotorcycle()
    {
        var dbContext = fixture.CreateDbContext();
        var repository = new MotorcycleRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var useCase = new ApplicationUseCases.CreateMotorcycle(
            repository, unitOfWork
        );
        var input = fixture.GetInput();

        var output = await useCase.Handle(input, CancellationToken.None);

        var dbMotorcycle = await dbContext
            .Motorcycles.FindAsync(output.Id);
        dbMotorcycle.Should().NotBeNull();
        dbMotorcycle!.Year.Should().Be(input.Year);
        dbMotorcycle.LicensePlate.Should().Be(input.LicensePlate);
        dbMotorcycle.Model.Should().Be(input.Model);
        dbMotorcycle.CreatedAt.Should().Be(output.CreatedAt);
        output.Should().NotBeNull();
        output.Year.Should().Be(input.Year);
        output.Model.Should().Be(input.Model);
        output.Model.Should().Be(input.Model);
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBeSameDateAs(default);
    }
}