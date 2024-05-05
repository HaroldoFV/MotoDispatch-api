using FluentAssertions;
using Moq;
using MotoDispatch.Application.Exceptions;
using UseCase = MotoDispatch.Application.UseCases.Motorcycle.DeleteMotorcycle;

namespace MotoDispatch.UnitTests.Application.Motorcycle.DeleteMotorcycle;

[Collection(nameof(DeleteMotorcycleTestFixture))]
public class DeleteMotorcycleTest(DeleteMotorcycleTestFixture fixture)
{
    [Fact(DisplayName = nameof(DeleteMotorcycle))]
    [Trait("Application", "DeleteMotorcycle - Use Cases")]
    public async Task DeleteMotorcycle()
    {
        var motorcycleRepositoryMock = fixture.GetMotorcycleRepositoryMock();
        var rentalRepositoryMock = fixture.GetRentalRepositoryMock();
        var unitOfWorkMock = fixture.GetUnitOfWorkMock();
        var motorcycleExample = fixture.GetExampleMotorcycle();

        motorcycleRepositoryMock.Setup(x => x.Get(
            motorcycleExample.Id,
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(motorcycleExample);
        var input = new UseCase.DeleteMotorcycleInput(motorcycleExample.Id);

        var useCase = new UseCase.DeleteMotorcycle(
            motorcycleRepositoryMock.Object,
            rentalRepositoryMock.Object,
            unitOfWorkMock.Object);

        await useCase.Handle(input, CancellationToken.None);

        motorcycleRepositoryMock.Verify(x => x.Get(
            motorcycleExample.Id,
            It.IsAny<CancellationToken>()
        ), Times.Once);

        motorcycleRepositoryMock.Verify(x => x.Delete(
            motorcycleExample,
            It.IsAny<CancellationToken>()
        ), Times.Once);

        unitOfWorkMock.Verify(x => x.Commit(
            It.IsAny<CancellationToken>()
        ), Times.Once);
    }

    [Fact(DisplayName = nameof(ThrowWhenMotorcycleNotFound))]
    [Trait("Application", "DeleteMotorcycle - Use Cases")]
    public async Task ThrowWhenMotorcycleNotFound()
    {
        var motorcycleRepositoryMock = fixture.GetMotorcycleRepositoryMock();
        var rentalRepositoryMock = fixture.GetRentalRepositoryMock();
        var unitOfWorkMock = fixture.GetUnitOfWorkMock();
        var exampleGuid = Guid.NewGuid();

        motorcycleRepositoryMock.Setup(x => x.Get(
            exampleGuid,
            It.IsAny<CancellationToken>())
        ).ThrowsAsync(
            new NotFoundException($"Motorcycle '{exampleGuid}' not found")
        );
        var input = new UseCase.DeleteMotorcycleInput(exampleGuid);

        var useCase = new UseCase.DeleteMotorcycle(
            motorcycleRepositoryMock.Object,
            rentalRepositoryMock.Object,
            unitOfWorkMock.Object);

        var task = async ()
            => await useCase.Handle(input, CancellationToken.None);

        await task.Should()
            .ThrowAsync<NotFoundException>();

        motorcycleRepositoryMock.Verify(x => x.Get(
            exampleGuid,
            It.IsAny<CancellationToken>()
        ), Times.Once);
    }
}