using MotoDispatch.Domain.Entity;
using MotoDispatch.Domain.SeedWork.SearchableRepository;
using MotoDispatch.IntegrationTests.Base;

namespace MotoDispatch.IntegrationTests.Infra.Data.EF.Repositories;

[CollectionDefinition(nameof(MotorcycleRepositoryTestFixture))]
public class MotorcycleRepositoryTestFixtureCollection
    : ICollectionFixture<MotorcycleRepositoryTestFixture>
{
}

public class MotorcycleRepositoryTestFixture
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

    public List<Motorcycle> GetExampleMotorcyclesListWithNames(List<string> names)
    {
        return names.Select(name =>
        {
            var motorcycle = GetExampleMotorcycle();
            motorcycle.Update(name);
            return motorcycle;
        }).ToList();
    }


    public List<Motorcycle> CloneMotorcyclesListOrdered(
        List<Motorcycle> motorcyclesList,
        string orderBy,
        SearchOrder order
    )
    {
        var listClone = new List<Motorcycle>(motorcyclesList);

        var orderedEnumerable = (orderBy.ToLower(), order) switch
        {
            ("licensePlate", SearchOrder.Asc) => listClone.OrderBy(x => x.LicensePlate)
                .ThenBy(x => x.Id),
            ("licensePlate", SearchOrder.Desc) => listClone.OrderByDescending(x => x.LicensePlate)
                .ThenByDescending(x => x.Id),
            ("id", SearchOrder.Asc) => listClone.OrderBy(x => x.Id),
            ("id", SearchOrder.Desc) => listClone.OrderByDescending(x => x.Id),
            ("createdat", SearchOrder.Asc) => listClone.OrderBy(x => x.CreatedAt),
            ("createdat", SearchOrder.Desc) => listClone.OrderByDescending(x => x.CreatedAt),
            _ => listClone.OrderBy(x => x.LicensePlate)
                .ThenBy(x => x.Id),
        };
        return orderedEnumerable.ToList();
    }
}