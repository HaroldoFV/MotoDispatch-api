using FluentAssertions;
using Moq;
using MotoDispatch.Application.Exceptions;
using MotoDispatch.Application.UseCases.Motorcycle.Common;
using MotoDispatch.Application.UseCases.Motorcycle.UpdateMotorcycle;
using MotoDispatch.Domain.Exception;
using DomainEntity = MotoDispatch.Domain.Entity;
using UseCase = MotoDispatch.Application.UseCases.Motorcycle.UpdateMotorcycle;

namespace MotoDispatch.UnitTests.Application.Motorcycle.UpdateMotorcycle;

[Collection(nameof(UpdateMotorcycleTestFixture))]
public class UpdateMotorcycleTest(UpdateMotorcycleTestFixture fixture)
{
    [Theory(DisplayName = nameof(UpdateMotorcycle))]
    [Trait("Application", "UpdateMotorcycle - Use Cases")]
    [MemberData(
        nameof(UpdateMotorcycleTestDataGenerator.GetMotorcycleToUpdate),
        parameters: 10,
        MemberType = typeof(UpdateMotorcycleTestDataGenerator)
    )]
    public async Task UpdateMotorcycle(
        DomainEntity.Motorcycle exampleMotorcycle,
        UpdateMotorcycleInput input
    )
    {
        var motorcycleRepositoryMock = fixture.GetMotorcycleRepositoryMock();
        var unitOfWorkMock = fixture.GetUnitOfWorkMock();

        motorcycleRepositoryMock.Setup(x => x.Get(
            exampleMotorcycle.Id,
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(exampleMotorcycle);

        var useCase = new UseCase.UpdateMotorcycle(
            motorcycleRepositoryMock.Object,
            unitOfWorkMock.Object
        );

        MotorcycleModelOutput output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Year.Should().Be(input.Year);
        output.LicensePlate.Should().Be(input.LicensePlate);
        output.Model.Should().Be(input.Model);

        motorcycleRepositoryMock.Verify(x => x.Get(
                exampleMotorcycle.Id,
                It.IsAny<CancellationToken>())
            , Times.Once);

        motorcycleRepositoryMock.Verify(x => x.Update(
                exampleMotorcycle,
                It.IsAny<CancellationToken>())
            , Times.Once);

        unitOfWorkMock.Verify(x => x.Commit(
                It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact(DisplayName = nameof(ThrowWhenMotorcycleNotFound))]
    [Trait("Application", "UpdateMotorcycle - Use Cases")]
    public async Task ThrowWhenMotorcycleNotFound()
    {
        var motorcycleRepositoryMock = fixture.GetMotorcycleRepositoryMock();
        var unitOfWorkMock = fixture.GetUnitOfWorkMock();
        var input = fixture.GetValidInput();

        motorcycleRepositoryMock.Setup(x => x.Get(
            input.Id,
            It.IsAny<CancellationToken>())
        ).ThrowsAsync(new NotFoundException($"Motorcycle '{input.Id}' not found"));

        var useCase = new UseCase.UpdateMotorcycle(
            motorcycleRepositoryMock.Object,
            unitOfWorkMock.Object
        );

        var task = async ()
            => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>();

        motorcycleRepositoryMock.Verify(x => x.Get(
                input.Id,
                It.IsAny<CancellationToken>())
            , Times.Once);
    }

    [Theory(DisplayName = nameof(UpdateMotorcycleOnlyLicensePlate))]
    [Trait("Application", "UpdateMotorcycle - Use Cases")]
    [MemberData(
        nameof(UpdateMotorcycleTestDataGenerator.GetMotorcycleToUpdate),
        parameters: 10,
        MemberType = typeof(UpdateMotorcycleTestDataGenerator)
    )]
    public async Task UpdateMotorcycleOnlyLicensePlate(
        DomainEntity.Motorcycle exampleMotorcycle,
        UpdateMotorcycleInput exampleInput
    )
    {
        var input = new UpdateMotorcycleInput(
            exampleInput.Id,
            exampleInput.LicensePlate
        );
        var motorcycleRepositoryMock = fixture.GetMotorcycleRepositoryMock();
        var unitOfWorkMock = fixture.GetUnitOfWorkMock();

        motorcycleRepositoryMock.Setup(x => x.Get(
            exampleMotorcycle.Id,
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(exampleMotorcycle);

        var useCase = new UseCase.UpdateMotorcycle(
            motorcycleRepositoryMock.Object,
            unitOfWorkMock.Object
        );

        MotorcycleModelOutput output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.LicensePlate.Should().Be(input.LicensePlate);
        output.Year.Should().Be(exampleMotorcycle.Year);
        output.Model.Should().Be(exampleMotorcycle.Model);

        motorcycleRepositoryMock.Verify(x => x.Get(
                exampleMotorcycle.Id,
                It.IsAny<CancellationToken>())
            , Times.Once);

        motorcycleRepositoryMock.Verify(x => x.Update(
                exampleMotorcycle,
                It.IsAny<CancellationToken>())
            , Times.Once);

        unitOfWorkMock.Verify(x => x.Commit(
                It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Theory(DisplayName = nameof(ThrowWhenCantUpdateMotorcycle))]
    [Trait("Application", "UpdateMotorcycle - Use Cases")]
    [MemberData(
        nameof(UpdateMotorcycleTestDataGenerator.GetInvalidInputs),
        MemberType = typeof(UpdateMotorcycleTestDataGenerator)
    )]
    public async Task ThrowWhenCantUpdateMotorcycle(
        UpdateMotorcycleInput input,
        string expectedMessage
    )
    {
        var exampleMotorcycle = fixture.GetExampleMotorcycle();
        input.Id = exampleMotorcycle.Id;

        var motorcycleRepositoryMock = fixture.GetMotorcycleRepositoryMock();
        var unitOfWorkMock = fixture.GetUnitOfWorkMock();

        motorcycleRepositoryMock.Setup(x => x.Get(
            exampleMotorcycle.Id,
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(exampleMotorcycle);

        var useCase = new UseCase.UpdateMotorcycle(
            motorcycleRepositoryMock.Object,
            unitOfWorkMock.Object
        );

        var task = async ()
            => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<EntityValidationException>()
            .WithMessage(expectedMessage);

        motorcycleRepositoryMock.Verify(x => x.Get(
                exampleMotorcycle.Id,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Theory(DisplayName = nameof(UpdateDuplicateLicensePlate))]
    [Trait("Application", "UpdateMotorcycle - Use Cases")]
    [MemberData(
        nameof(UpdateMotorcycleTestDataGenerator.GetMotorcycleToUpdate),
        parameters: 10,
        MemberType = typeof(UpdateMotorcycleTestDataGenerator)
    )]
    public async Task UpdateDuplicateLicensePlate(
        DomainEntity.Motorcycle exampleMotorcycle,
        UpdateMotorcycleInput input
    )
    {
        var motorcycleRepositoryMock = fixture.GetMotorcycleRepositoryMock();
        var unitOfWorkMock = fixture.GetUnitOfWorkMock();

        motorcycleRepositoryMock.Setup(x => x.Get(
            exampleMotorcycle.Id,
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(exampleMotorcycle);

        motorcycleRepositoryMock.Setup(x => x.GetByLicensePlate(
            input.LicensePlate,
            It.IsAny<CancellationToken>())
        )!.ReturnsAsync(input.LicensePlate == exampleMotorcycle.LicensePlate
            ? null
            : new DomainEntity.Motorcycle(2022, input.LicensePlate, "Other Model"));

        var useCase = new UseCase.UpdateMotorcycle(
            motorcycleRepositoryMock.Object,
            unitOfWorkMock.Object
        );

        Func<Task> action = async () => await useCase.Handle(input, CancellationToken.None);

        await action.Should()
            .ThrowAsync<DuplicateEntityException>()
            .WithMessage("Duplicate license plate.");

        motorcycleRepositoryMock.Verify(x => x.Get(
                exampleMotorcycle.Id,
                It.IsAny<CancellationToken>()),
            Times.Once);

        motorcycleRepositoryMock.Verify(x => x.Update(
                exampleMotorcycle,
                It.IsAny<CancellationToken>()),
            Times.Never());

        unitOfWorkMock.Verify(x => x.Commit(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
}