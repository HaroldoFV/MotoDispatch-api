using MotoDispatch.UnitTests.Common;
using DomainEntity = MotoDispatch.Domain.Entity;

namespace MotoDispatch.UnitTests.Domain.Entity.Motorcycle;

public class MotorcycleTestFixture : BaseFixture
{
    public MotorcycleTestFixture()
        : base()
    {
    }

    public DomainEntity.Motorcycle GetValidMotorcycle()
    {
        var year = Faker.Date.Past(10, DateTime.Now).Year;
        var licensePlate = Faker.Random.Replace("???-####");

        var model = Faker.Random.ArrayElement(new string[]
        {
            "Honda CBR 250R", "Yamaha R1", "Ducati Monster 821", "BMW S1000RR", "Kawasaki Ninja 650"
        });

        return new DomainEntity.Motorcycle(year, licensePlate, model);
    }

    public DomainEntity.Motorcycle CreateInvalidModelMotorcycle()
    {
        var year = Faker.Date.Past(10).Year;
        var licensePlate = Faker.Random.Replace("???-####");

        var model = Faker.Random.Bool()
            ? new string('X', 51)
            : new string('X', 1);

        return new DomainEntity.Motorcycle(year, licensePlate, model);
    }
}

[CollectionDefinition(nameof(MotorcycleTestFixture))]
public class MotorcycleTestFixtureCollection
    : ICollectionFixture<MotorcycleTestFixture>
{
}