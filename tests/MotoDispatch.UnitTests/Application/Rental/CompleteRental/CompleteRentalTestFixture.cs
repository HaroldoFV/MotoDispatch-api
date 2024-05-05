using Moq;
using MotoDispatch.Application.Interfaces;
using MotoDispatch.Application.UseCases.Rental.CompleteRental;
using MotoDispatch.Domain.Entity;
using MotoDispatch.Domain.Repository;
using MotoDispatch.UnitTests.Application.Rental.Common;
using DomainEntity = MotoDispatch.Domain.Entity;

namespace MotoDispatch.UnitTests.Application.Rental.CompleteRental;

[CollectionDefinition(nameof(CompleteRentalTestFixture))]
public class CompleteRentalTestFixtureCollection : ICollectionFixture<CompleteRentalTestFixture>
{
}

public class CompleteRentalTestFixture :
    RentalUseCasesBaseFixture, IDisposable
{
    public Mock<IRentalRepository> RentalRepositoryMock { get; private set; }
    public Mock<IRentalPlanRepository> RentalPlanRepositoryMock { get; private set; }
    public Mock<IUnitOfWork> UnitOfWorkMock { get; private set; }

    public CompleteRentalTestFixture()
    {
        RentalRepositoryMock = new Mock<IRentalRepository>();
        RentalPlanRepositoryMock = new Mock<IRentalPlanRepository>();
        UnitOfWorkMock = new Mock<IUnitOfWork>();
    }

    public RentalPlan CreateValidRentalPlan()
    {
        return new RentalPlan(7, 30.00m, 0.20m, 50.00m);
    }

    public DomainEntity.Rental CreateValidRental()
    {
        var rentalPlan = CreateValidRentalPlan();
        return new DomainEntity.Rental(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), rentalPlan);
    }

    public CompleteRentalInput CreateCompleteRentalInput(Guid rentalId, DateTime returnDate)
    {
        return new CompleteRentalInput(rentalId, returnDate);
    }

    public void Dispose()
    {
        RentalRepositoryMock.Reset();
        RentalPlanRepositoryMock.Reset();
        UnitOfWorkMock.Reset();
    }
}