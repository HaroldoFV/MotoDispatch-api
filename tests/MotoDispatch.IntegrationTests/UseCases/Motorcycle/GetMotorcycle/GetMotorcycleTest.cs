using FluentAssertions;
using MotoDispatch.Application.Exceptions;
using MotoDispatch.Infra.Data.EF.Repositories;
using UseCase = MotoDispatch.Application.UseCases.Motorcycle.GetMotorcycle;

namespace MotoDispatch.IntegrationTests.UseCases.Motorcycle.GetMotorcycle;

[Collection(nameof(GetMotorcycleTestFixture))]
public class GetMotorcycleTest(GetMotorcycleTestFixture fixture)
{
    [Fact(DisplayName = nameof(GetMotorcycle))]
    [Trait("Integration/Application", "GetMotorcycle - Use Cases")]
    public async Task GetMotorcycle()
    {
        var exampleMotorcycle = fixture.GetExampleMotorcycle();
        var dbContext = fixture.CreateDbContext();
        dbContext.Motorcycles.Add(exampleMotorcycle);
        await dbContext.SaveChangesAsync();
        var repository = new MotorcycleRepository(dbContext);
        var input = new UseCase.GetMotorcycleInput(exampleMotorcycle.Id);
        var useCase = new UseCase.GetMotorcycle(repository);

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Year.Should().Be(exampleMotorcycle.Year);
        output.LicensePlate.Should().Be(exampleMotorcycle.LicensePlate);
        output.Model.Should().Be(exampleMotorcycle.Model);
        output.Id.Should().Be(exampleMotorcycle.Id);
        output.CreatedAt.Should().Be(exampleMotorcycle.CreatedAt);
    }


    [Fact(DisplayName = nameof(NotFoundExceptionWhenMotorcycleDoesntExist))]
    [Trait("Integration/Application", "GetMotorcycle - Use Cases")]
    public async Task NotFoundExceptionWhenMotorcycleDoesntExist()
    {
        var exampleMotorcycle = fixture.GetExampleMotorcycle();
        var dbContext = fixture.CreateDbContext();
        dbContext.Motorcycles.Add(exampleMotorcycle);
        await dbContext.SaveChangesAsync();
        var repository = new MotorcycleRepository(dbContext);
        var input = new UseCase.GetMotorcycleInput(Guid.NewGuid());
        var useCase = new UseCase.GetMotorcycle(repository);

        var task = async ()
            => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Motorcycle '{input.Id}' not found.");
    }
}