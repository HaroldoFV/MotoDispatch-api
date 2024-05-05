using MotoDispatch.Domain.Entity;
using MotoDispatch.IntegrationTests.Base;

namespace MotoDispatch.IntegrationTests.Infra.Data.EF.UnitOfWork;

[CollectionDefinition(nameof(UnitOfWorkTestFixture))]
public class UnitOfWorkTestFixtureCollection
    : ICollectionFixture<UnitOfWorkTestFixture>
{
}

public class UnitOfWorkTestFixture
    : BaseFixture
{
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

    public Motorcycle GetExampleMotorcycle()
    {
        var year = GetValidYear();
        var licensePlate = GetValidLicensePlate();
        var model = GetValidModel();

        return new(year, licensePlate, model);
    }

    public List<Motorcycle> GetExampleMotorcyclesList(int length = 10)
    {
        return Enumerable.Range(1, length)
            .Select(_ => GetExampleMotorcycle()).ToList();
    }
}