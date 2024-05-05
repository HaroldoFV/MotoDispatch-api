using FluentAssertions;
using Moq;
using MotoDispatch.Application.Exceptions;
using MotoDispatch.UnitTests.Application.GetMotorcycle;
using UseCase = MotoDispatch.Application.UseCases.Motorcycle.GetMotorcycle;

namespace MotoDispatch.UnitTests.Application.Motorcycle.GetMotorcycle
{
    [Collection(nameof(GetMotorcycleTestFixture))]
    public class GetMotorcycleTest(GetMotorcycleTestFixture fixture)
    {
        [Fact(DisplayName = nameof(GetMotorcycle))]
        [Trait("Application ", "GetMotorcycle - Use Cases")]
        public async Task GetMotorcycle()
        {
            var motorcycleRepositoryMock = fixture.GetMotorcycleRepositoryMock();
            var exampleMotorcycle = fixture.GetExampleMotorcycle();

            motorcycleRepositoryMock.Setup(x => x.Get(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(exampleMotorcycle);
            var input = new UseCase.GetMotorcycleInput(exampleMotorcycle.Id);
            var useCase = new UseCase.GetMotorcycle(motorcycleRepositoryMock.Object);

            var output = await useCase.Handle(input, CancellationToken.None);

            motorcycleRepositoryMock.Verify(x => x.Get(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()
            ), Times.Once);

            output.Should().NotBeNull();
            output.Year.Should().Be(exampleMotorcycle.Year);
            output.LicensePlate.Should().Be(exampleMotorcycle.LicensePlate);
            output.Model.Should().Be(exampleMotorcycle.Model);
            output.Id.Should().Be(exampleMotorcycle.Id);
            output.CreatedAt.Should().Be(exampleMotorcycle.CreatedAt);
        }

        [Fact(DisplayName = nameof(NotFoundExceptionWhenMotorcycleDoesntExist))]
        [Trait("Application ", "GetMotorcycle - Use Cases")]
        public async Task NotFoundExceptionWhenMotorcycleDoesntExist()
        {
            var motorcycleRepositoryMock = fixture.GetMotorcycleRepositoryMock();
            var exampleGuid = Guid.NewGuid();

            motorcycleRepositoryMock.Setup(x => x.Get(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()
            )).ThrowsAsync(
                new NotFoundException($"Motorcycle '{exampleGuid}' not found")
            );
            var input = new UseCase.GetMotorcycleInput(exampleGuid);
            var useCase = new UseCase.GetMotorcycle(motorcycleRepositoryMock.Object);

            Func<Task> action = async ()
                => await useCase.Handle(input, CancellationToken.None);

            await action.Should().ThrowAsync<NotFoundException>();

            motorcycleRepositoryMock.Verify(x => x.Get(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()
            ), Times.Once);
        }
    }
}