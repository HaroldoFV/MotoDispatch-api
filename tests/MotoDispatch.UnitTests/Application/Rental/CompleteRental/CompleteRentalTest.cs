using FluentAssertions;
using Moq;
using MotoDispatch.Application.Exceptions;
using MotoDispatch.Application.Interfaces;
using MotoDispatch.Application.UseCases.Rental.CompleteRental;
using MotoDispatch.Domain.Repository;
using DomainEntity = MotoDispatch.Domain.Entity;
using UseCases = MotoDispatch.Application.UseCases.Rental.CompleteRental;

namespace MotoDispatch.UnitTests.Application.Rental.CompleteRental;

[Collection(nameof(CompleteRentalTestFixture))]
public class CompleteRentalTest(
    CompleteRentalTestFixture fixture
)
{
    [Fact(DisplayName = nameof(CompleteRentalSuccessfully))]
    [Trait("Application", "CompleteRental - Use Cases")]
    public async Task CompleteRentalSuccessfully()
    {
        var rental = fixture.CreateValidRental();

        fixture.RentalRepositoryMock.Setup(
                x => x.Get(rental.Id,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(rental);

        var actualReturnDate = DateTime.UtcNow.AddDays(8);
        var input = fixture.CreateCompleteRentalInput(rental.Id, actualReturnDate);

        var useCase = new UseCases.CompleteRental(
            fixture.RentalRepositoryMock.Object,
            fixture.RentalPlanRepositoryMock.Object,
            fixture.UnitOfWorkMock.Object
        );

        var output = await useCase.Handle(input, CancellationToken.None);

        output.TotalCost.Should().BeGreaterThan(0);
        rental.ActualEndDate.Should().Be(actualReturnDate);

        fixture.RentalRepositoryMock.Verify(
            x => x.Update(rental, It.IsAny<CancellationToken>()),
            Times.Once);

        fixture.UnitOfWorkMock.Verify(
            x => x.Commit(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact(DisplayName = nameof(ThrowsWhenRentalNotFound))]
    [Trait("Application", "CompleteRental - Use Cases")]
    public async Task ThrowsWhenRentalNotFound()
    {
        var mockRentalRepository = new Mock<IRentalRepository>();
        var mockRentalPlanRepository = new Mock<IRentalPlanRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var rentalId = Guid.NewGuid();

        mockRentalRepository.Setup(x =>
                x.Get(rentalId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new NotFoundException($"Rental '{rentalId}' not found."));

        var input = fixture.CreateCompleteRentalInput(rentalId, DateTime.UtcNow);

        var useCase = new UseCases.CompleteRental(
            mockRentalRepository.Object,
            mockRentalPlanRepository.Object,
            mockUnitOfWork.Object
        );

        Func<Task> act = async () =>
            await useCase.Handle(input, CancellationToken.None);

        await act.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"Rental '{rentalId}' not found.");

        mockUnitOfWork.Verify(
            x => x.Commit(It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Theory(DisplayName = nameof(CompleteRentalCalculatesTotalCostCorrectly))]
    [Trait("Application", "CompleteRental - Use Cases")]
    [MemberData(nameof(RentalPlanData))]
    public async Task CompleteRentalCalculatesTotalCostCorrectly(
        int days,
        decimal dailyRate,
        decimal penaltyRate,
        decimal fixedAdditionalRate,
        int rentedDays,
        DateTime startDate,
        DateTime returnDate,
        decimal expectedCost
    )
    {
        var mockRentalRepository = new Mock<IRentalRepository>();
        var mockRentalPlanRepository = new Mock<IRentalPlanRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        var rentalPlan = new DomainEntity.RentalPlan(
            days,
            dailyRate,
            penaltyRate,
            fixedAdditionalRate
        );

        var rental = new DomainEntity.Rental(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            rentalPlan);

        mockRentalRepository.Setup(
                x => x.Get(
                    rental.Id,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(rental);

        var completeRentalUseCase = new UseCases.CompleteRental(
            mockRentalRepository.Object,
            mockRentalPlanRepository.Object,
            mockUnitOfWork.Object
        );

        var input = new CompleteRentalInput(rental.Id, returnDate);

        var output = await completeRentalUseCase.Handle(input, CancellationToken.None);

        output.TotalCost.Should().Be(expectedCost);
        rental.ActualEndDate.Should().Be(returnDate);

        mockRentalRepository.Verify(
            x => x.Update(rental, It.IsAny<CancellationToken>()),
            Times.Once);

        mockUnitOfWork.Verify(
            x => x.Commit(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact(DisplayName = nameof(CompleteRentalFor7DaysPlanReturnsCorrectTotalCost))]
    [Trait("Application", "CompleteRental - Use Cases")]
    public async Task CompleteRentalFor7DaysPlanReturnsCorrectTotalCost()
    {
        var mockRentalRepository = new Mock<IRentalRepository>();
        var mockRentalPlanRepository = new Mock<IRentalPlanRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var days = 7;
        var dailyRate = 30.00m;
        var penaltyRate = 0.20m;
        var fixedAdditionalRate = 50.00m;
        var rentedDays = 7;
        var startDate = DateTime.UtcNow.AddDays(1);
        var returnDate = startDate.AddDays(rentedDays - 1);
        var expectedCost = 210.00m;

        var rentalPlan = new DomainEntity.RentalPlan(days, dailyRate, penaltyRate, fixedAdditionalRate);
        var rental = new DomainEntity.Rental(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), rentalPlan);

        mockRentalRepository.Setup(x => x.Get(rental.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(rental);

        var completeRentalUseCase = new UseCases.CompleteRental(
            mockRentalRepository.Object,
            mockRentalPlanRepository.Object,
            mockUnitOfWork.Object
        );

        var input = new CompleteRentalInput(rental.Id, returnDate);
        var output = await completeRentalUseCase.Handle(input, CancellationToken.None);

        output.TotalCost.Should().Be(expectedCost);
        rental.ActualEndDate.Should().Be(returnDate);

        mockRentalRepository.Verify(
            x => x.Update(rental,
                It.IsAny<CancellationToken>()),
            Times.Once);

        mockUnitOfWork.Verify(
            x => x.Commit(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    public static IEnumerable<object[]> RentalPlanData()
    {
        var startDate = DateTime.UtcNow.AddDays(1); // Configura a data de início como amanhã

        // Dados para o plano de 7 dias
        yield return new object[] {7, 30.00m, 0.20m, 50.00m, 7, startDate, startDate.AddDays(6), 210.00m};
        yield return new object[] {7, 30.00m, 0.20m, 50.00m, 5, startDate, startDate.AddDays(4), 162.00m};
        yield return new object[] {7, 30.00m, 0.20m, 50.00m, 9, startDate, startDate.AddDays(8), 310.00m};

        // Dados para o plano de 15 dias
        yield return new object[] {15, 28.00m, 0.40m, 50.00m, 15, startDate, startDate.AddDays(14), 420.00m};
        yield return new object[] {15, 28.00m, 0.40m, 50.00m, 10, startDate, startDate.AddDays(9), 336.00m};
        yield return new object[] {15, 28.00m, 0.40m, 50.00m, 20, startDate, startDate.AddDays(19), 670.00m};

        // Dados para o plano de 30 dias
        yield return new object[] {30, 22.00m, 0m, 50.00m, 30, startDate, startDate.AddDays(29), 660.00m};
        yield return new object[] {30, 22.00m, 0m, 50.00m, 25, startDate, startDate.AddDays(24), 550.00m};
        yield return new object[] {30, 22.00m, 0m, 50.00m, 35, startDate, startDate.AddDays(34), 910.00m};

        // Dados para o plano de 45 dias
        yield return new object[] {45, 20.00m, 0m, 50.00m, 45, startDate, startDate.AddDays(44), 900.00m};
        yield return new object[] {45, 20.00m, 0m, 50.00m, 40, startDate, startDate.AddDays(39), 800.00m};
        yield return new object[] {45, 20.00m, 0m, 50.00m, 50, startDate, startDate.AddDays(49), 1150.00m};

        // Dados para o plano de 50 dias
        yield return new object[] {50, 18.00m, 0m, 50.00m, 50, startDate, startDate.AddDays(49), 900.00m};
        yield return new object[] {50, 18.00m, 0m, 50.00m, 45, startDate, startDate.AddDays(44), 810.00m};
        yield return new object[] {50, 18.00m, 0m, 50.00m, 55, startDate, startDate.AddDays(54), 1150.00m};
    }
}