using FluentAssertions;
using Moq;
using MotoDispatch.Application.Exceptions;
using MotoDispatch.Domain.Exception;
using ApplicationException = System.ApplicationException;
using UseCases = MotoDispatch.Application.UseCases.Motorcycle.CreateMotorcycle;

namespace MotoDispatch.UnitTests.Application.Motorcycle.CreateMotorcycle;

[Collection(nameof(CreateMotorcycleTestFixture))]
public class CreateMotorcycleTest(CreateMotorcycleTestFixture fixture)
{
    [Fact(DisplayName = nameof(CreateMotorcycle))]
    [Trait("Application", "CreateMotorcycle - Use Cases")]
    public async void CreateMotorcycle()
    {
        var motorcycleRepositoryMock = fixture.GetMotorcycleRepositoryMock();
        var unitOfWorkMock = fixture.GetUnitOfWorkMock();

        var useCase = new UseCases.CreateMotorcycle(
            motorcycleRepositoryMock.Object, unitOfWorkMock.Object
        );

        var input = fixture.GetInput();

        var output = await useCase.Handle(input, CancellationToken.None);

        motorcycleRepositoryMock.Verify(
            repository => repository.Insert(
                It.IsAny<MotoDispatch.Domain.Entity.Motorcycle>(),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );

        unitOfWorkMock.Verify(
            uow => uow.Commit(It.IsAny<CancellationToken>()),
            Times.Once
        );

        output.Should().NotBeNull();
        output.Year.Should().Be(input.Year);
        output.LicensePlate.Should().Be(input.LicensePlate);
        output.Model.Should().Be(input.Model);
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBeSameDateAs(default);
    }

    [Theory(DisplayName = nameof(ThrowWhenCantInstantiateAggregate))]
    [Trait("Application", "CreateMotorcycle - Use Cases")]
    [MemberData(
        nameof(CreateMotorcycleTestDataGenerator.GetInvalidInputs),
        MemberType = typeof(CreateMotorcycleTestDataGenerator)
    )]
    public async void ThrowWhenCantInstantiateAggregate(
        UseCases.CreateMotorcycleInput input,
        string expectedMessage
    )
    {
        var useCase = new UseCases.CreateMotorcycle(
            fixture.GetMotorcycleRepositoryMock().Object,
            fixture.GetUnitOfWorkMock().Object
        );

        Func<Task> task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should()
            .ThrowAsync<EntityValidationException>()
            .WithMessage(expectedMessage);
    }

    [Fact(DisplayName = nameof(InsertDuplicateLicensePlate))]
    [Trait("Application", "CreateMotorcycle - Use Cases")]
    public async Task InsertDuplicateLicensePlate()
    {
        var repositoryMock = fixture.GetMotorcycleRepositoryMock();
        var unitOfWorkMock = fixture.GetUnitOfWorkMock();

        repositoryMock.Setup(repository =>
                repository.Insert(
                    It.IsAny<MotoDispatch.Domain.Entity.Motorcycle>(),
                    It.IsAny<CancellationToken>()))
            .Callback((MotoDispatch.Domain.Entity.Motorcycle mc, CancellationToken ct) =>
            {
                if (mc.LicensePlate == "AAA-1234")
                    throw new DuplicateEntityException("Duplicate license plate.");
            });

        var useCase = new UseCases.CreateMotorcycle(repositoryMock.Object, unitOfWorkMock.Object);

        var input = fixture.GetInput();
        input.LicensePlate = "AAA-1234";

        Func<Task> action = async () => await useCase.Handle(input, CancellationToken.None);

        await action.Should()
            .ThrowAsync<DuplicateEntityException>()
            .WithMessage("Duplicate license plate.");

        repositoryMock.Verify(
            repository =>
                repository.Insert(
                    It.IsAny<MotoDispatch.Domain.Entity.Motorcycle>(),
                    It.IsAny<CancellationToken>()
                ),
            Times.Once);

        unitOfWorkMock.Verify(uow => uow.Commit(It.IsAny<CancellationToken>()), Times.Never());
    }
}