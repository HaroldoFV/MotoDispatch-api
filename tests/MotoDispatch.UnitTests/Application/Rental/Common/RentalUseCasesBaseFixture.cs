using Moq;
using MotoDispatch.Application.Interfaces;
using MotoDispatch.Domain.Repository;
using MotoDispatch.UnitTests.Common;
using DomainEntity = MotoDispatch.Domain.Entity;

namespace MotoDispatch.UnitTests.Application.Rental.Common;

public class RentalUseCasesBaseFixture
    : BaseFixture
{
    public Mock<IRentalPlanRepository> GetRentalPlanRepositoryMock()
        => new();

    public Mock<IRentalRepository> GetRentalRepositoryMock()
        => new();

    public Mock<IUnitOfWork> GetUnitOfWorkMock()
        => new();
}