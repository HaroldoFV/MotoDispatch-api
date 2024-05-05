using Moq;
using MotoDispatch.Application.Interfaces;
using MotoDispatch.Domain.Repository;
using MotoDispatch.UnitTests.Common;
using DomainEntity = MotoDispatch.Domain.Entity;

namespace MotoDispatch.UnitTests.Application.Motorcycle.Common;

public class MotorcycleUseCasesBaseFixture
    : BaseFixture
{
    public Mock<IMotorcycleRepository> GetMotorcycleRepositoryMock()
        => new();

    public Mock<IRentalRepository> GetRentalRepositoryMock()
        => new();

    public Mock<IUnitOfWork> GetUnitOfWorkMock()
        => new();

    public string GetValidModel()
    {
        var model = Faker.Random.ArrayElement(new string[]
        {
            "Honda CBR 250R", "Yamaha R1", "Ducati Monster 821", "BMW S1000RR", "Kawasaki Ninja 650"
        });
        return model;
    }

    public string GetValidLicensePlate()
    {
        var licensePlate = Faker.Random.Replace("???-####");
        return licensePlate;
    }

    public int GetValidYear()
    {
        var year = Faker.Date.Past(10, DateTime.Now).Year;
        return year;
    }

    public DomainEntity.Motorcycle GetExampleMotorcycle()
    {
        var year = GetValidYear();
        var licensePlate = GetValidLicensePlate();
        var model = GetValidModel();

        return new(year, licensePlate, model);
    }
}